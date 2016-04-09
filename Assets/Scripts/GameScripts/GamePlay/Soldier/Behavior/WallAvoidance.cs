using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Generic;

namespace Green
{
    public class WallAvoidance : SteeringBehavior
    {
        public float AvoidanceForceScale = 4f;

        public float 侧向制动比例
        {
            get { return _侧向制动比例; }
            set
            {
                _侧向制动比例 = value;
                _反向制动比例 = 1 - value;
            }
        }
        public float 反向制动比例
        {
            get { return _反向制动比例; }
            set
            {
                _反向制动比例 = value;
                _侧向制动比例 = 1 - value;
            }
        }

        [SerializeField, SetProperty("侧向制动比例")]
        float _侧向制动比例 = 0.35f;
        [SerializeField, SetProperty("反向制动比例")]
        float _反向制动比例;

        float _wallDetectionFeelerLength = 1.8f;
        Vector2 _wallAvoidanceForce;

        //a vertex buffer to contain the feelers rqd for wall avoidance  
        List<Vector2> _feelers;

        public WallAvoidance(MovingEntity entity): base(entity, "WallAvoidance")
        {

        }

        public override Vector2 CalculateForce()
        {
            //the feelers are contained in a std::vector, m_Feelers
            CreateFeelers();

            float DistToThisIP = 0.0f;
            float DistToClosestIP = float.MaxValue;


            //this will hold an index into the vector of walls
            //Circle ClosestWall = -1;

            Vector2 SteeringForce = new Vector2(),
                      point = new Vector2(),         //used for storing temporary info
                      ClosestPoint = new Vector2();  //holds the closest intersection point

            //examine each feeler in turn
            foreach (var flr in _feelers)
            {
                Planet inPlanet = _movingEntity.GetComponent<Soldier>().InPlanet;
                if (inPlanet == null) Debug.LogError("Need Script Soldier!");
                //run through each wall checking for any intersection points
                //foreach(var w in walls)

                //for(int i = 0; i < planets.Count; ++i)
                //{
                CircleBorder2D.Area closestWallType;
                if (inPlanet.IsIntersection(_movingEntity.Position,
                                     flr,
                                 out DistToThisIP,
                                 out point,
                                 out closestWallType))
                {
                    //is this the closest found so far? If so keep a record
                    if (DistToThisIP < DistToClosestIP)
                    {
                        DistToClosestIP = DistToThisIP;

                        ClosestPoint = point;
                    }
                    //calculate by what distance the projected position of the agent
                    //will overshoot the wall
                    if (closestWallType == CircleBorder2D.Area.InCircle)
                    {
                        if (!Geometry.PointInCircle(flr, inPlanet.InCircle.CenterInWorldSpace, inPlanet.InCircle.Radius))
                            return new Vector2(0, 0);
                    }
                    else if (closestWallType == CircleBorder2D.Area.OutCircle)
                    {
                        if (Geometry.PointInCircle(flr, inPlanet.OutCircle.CenterInWorldSpace, inPlanet.OutCircle.Radius))
                        {
                            return new Vector2(0, 0);
                        }
                    }

                    Vector2 OverShoot = flr - ClosestPoint;

                    //create a force in the direction of the wall normal, with a 
                    //magnitude of the overshoot
                    SteeringForce = inPlanet.GetTangentNormal(ClosestPoint) * OverShoot.magnitude * AvoidanceForceScale;
                }
                else //有可能在很里面或者很外面，回来
                {
                    //内圈 向外走 1/2的侧向制动力加1/2的往回制动力
                    if (Geometry.PointInCircle(flr, inPlanet.InCircle.CenterInWorldSpace, inPlanet.InCircle.Radius))
                    {
                        var length = inPlanet.InCircleRad - Vector2.Distance(inPlanet.InCircle.transform.position, _movingEntity.Position);
                        Vector2 vecToCenter = _movingEntity.Position - inPlanet.InCircle.CenterInWorldSpace;
                        SteeringForce += vecToCenter.normalized * length * 反向制动比例 * AvoidanceForceScale;
                        if (_movingEntity.Heading.x > 0) //向右拐
                        {
                            SteeringForce += _movingEntity.Heading.PerpendicularRight().normalized * length * 侧向制动比例 * AvoidanceForceScale;
                        }
                        else //向左拐
                        {
                            SteeringForce += _movingEntity.Heading.Perpendicular().normalized * length * 侧向制动比例 * AvoidanceForceScale;
                        }

                    }
                    else if (!Geometry.PointInCircle(flr, inPlanet.OutCircle.CenterInWorldSpace, inPlanet.OutCircle.Radius))
                    {
                        var length = Vector2.Distance(inPlanet.InCircle.transform.position, _movingEntity.Position) - inPlanet.InCircleRad;
                        Vector2 vecToCenter = inPlanet.InCircle.CenterInWorldSpace - _movingEntity.Position;
                        SteeringForce += vecToCenter.normalized * length * 反向制动比例 * AvoidanceForceScale;
                        if (_movingEntity.Heading.x > 0) //向右拐
                        {
                            SteeringForce += _movingEntity.Heading.PerpendicularRight().normalized * length * 侧向制动比例 * AvoidanceForceScale;
                        }
                        else
                        {
                            SteeringForce += _movingEntity.Heading.Perpendicular().normalized * length * 侧向制动比例 * AvoidanceForceScale;
                        }

                    }
                }
            }//next feeler
            _wallAvoidanceForce = SteeringForce;
            return SteeringForce;
        }

        //creates the antenna utilized by the wall avoidance behavior
        void CreateFeelers()
        {
            _feelers.Clear();
            //feeler pointing straight in front
            _feelers.Add(_movingEntity.Position + _wallDetectionFeelerLength * _movingEntity.Heading);

            //feeler to left
            Vector2 temp = _movingEntity.Heading;
            temp.RotateAroundOrigin(Mathf.PI / 2 * 3.5f);
            _feelers.Add(_movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp);

            /*
            //feeler to right
            Vector2 temp1 = _movingEntity.Heading;
            temp1.RotateAroundOrigin(Mathf.PI / 2 * 0.5f);
            _feelers.Add(_movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp1);
            */
        }
        
    }
}
