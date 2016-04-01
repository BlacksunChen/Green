using System;
using System.Collections.Generic;
using UnityEngine;
using Generic.Extensions;
using Generic.Util;
namespace Green
{
    public class SteeringBehaviors
    {
        public enum summing_method
        {
            weighted_average,
            prioritized,
            dithered
        };

        [Flags]
        private enum behavior_type : int
        {
            none = 1 << 0,
            seek = 1 << 1,
            flee = 1 << 2,
            arrive = 1 << 3,
            wander = 1 << 4,
            cohesion = 1 << 5,
            separation = 1 << 6,
            allignment = 1 << 7,
            obstacle_avoidance = 1 << 8,
            wall_avoidance = 1 << 9,
            follow_path = 1 << 10,
            pursuit = 1 << 11,
            evade = 1 << 12,
            interpose = 1 << 13,
            hide = 1 << 14,
            flock = 1 << 15,
            offset_pursuit = 1 << 16,
        }


        //a pointer to the owner of this instance
        MovingEntity _movingEntity;

        //the steering force created by the combined effect of all
        //the selected behaviors
        Vector2 _steeringForce;

        //these can be used to keep track of friends, pursuers, or prey
        MovingEntity _targetAgent1;
        MovingEntity _targetAgent2;

        //the current target
        Vector2 _target;

        //length of the 'detection box' utilized in obstacle avoidance
        float _detectionBoxLength;


        //a vertex buffer to contain the feelers rqd for wall avoidance  
        List<Vector2> _feelers;

        //the length of the 'feeler/s' used in wall detection
        float _wallDetectionFeelerLength;



        //the current position on the wander circle the agent is
        //attempting to steer towards
        Vector2 _wanderTarget;

        //explained above
        float _wanderJitter;
        float _wanderRadius;
        float _wanderDistance;


        //multipliers. These can be adjusted to effect strength of the  
        //appropriate behavior. Useful to get flocking the way you require
        //for example.
        float _weightSeparation;
        float _weightCohesion;
        float _weightAlignment;
        float _weightWander;
        float _weightObstacleAvoidance;
        float _weightWallAvoidance;
        float _weightSeek;
        float _weightFlee;
        float _weightArrive;
        float _weightPursuit;
        float _weightOffsetPursuit;
        float _weightInterpose;
        float _weightHide;
        float _weightEvade;
        float _weightFollowPath;

        //how far the agent can 'see'
        float _viewDistance;

        //pointer to any current path
        //Path* m_pPath;

        //the distance (squared) a MovingEntity has to be from a path waypoint before
        //it starts seeking to the next waypoint
        float _waypointSeekDistSq;


        //any offset used for formations or offset pursuit
        Vector2 _offset;



        //binary flags to indicate whether or not a behavior should be active
        behavior_type _flags;


        //Arrive makes use of these to determine how quickly a MovingEntity
        //should decelerate to its target
        enum Deceleration
        {
            slow = 3,
            normal = 2,
            fast = 1
        };

        //default
        Deceleration _deceleration;

        //is cell space partitioning to be used or not?
        bool _cellSpaceOn;

        //what type of method is used to sum any active behavior
        summing_method _summingMethod;


        //this function tests if a specific bit of m_iFlags is set
        bool On(behavior_type bt) { return (_flags & bt) == bt; }

        bool AccumulateForce(Vector2 runningTot, Vector2 forceToAdd)
        {
            //calculate how much steering force the MovingEntity has used so far
            float MagnitudeSoFar = runningTot.magnitude;

            //calculate how much steering force remains to be used by this MovingEntity
            float MagnitudeRemaining = _movingEntity.MaxForce - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            float MagnitudeToAdd = forceToAdd.magnitude;

            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this MovingEntity, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                runningTot += forceToAdd;
            }

            else
            {
                //add it to the steering force
                runningTot += forceToAdd.normalized * MagnitudeRemaining;
            }

            return true;
        }

        //creates the antenna utilized by the wall avoidance behavior
        void CreateFeelers()
        {
            //feeler pointing straight in front
            _feelers[0] = _movingEntity.Position + _wallDetectionFeelerLength * _movingEntity.Heading;

            //feeler to left
            Vector2 temp = _movingEntity.Heading;
            temp.RotateAroundOrigin(Mathf.PI / 2 * 3.5f);
            _feelers[1] = _movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp;

            //feeler to right
            temp = _movingEntity.Heading;
            temp.RotateAroundOrigin(Mathf.PI / 2 * 0.5f);
            _feelers[2] = _movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp;
        }



        /* .......................................................

                         BEGIN BEHAVIOR DECLARATIONS

           .......................................................*/


        /// <summary>
        ///  Given a target, this behavior returns a steering force which will
        /// direct the agent towards the target
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns></returns>
        Vector2 Seek(Vector2 TargetPos)
        {
            Vector2 DesiredVelocity = (TargetPos - _movingEntity.Position).normalized
                            * _movingEntity.MaxSpeed;

            return (DesiredVelocity - _movingEntity.Velocity);
        }

        /// <summary>
        /// Does the opposite of Seek
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns></returns>
        Vector2 Flee(Vector2 TargetPos)
        {
            Vector2 DesiredVelocity = (_movingEntity.Position - TargetPos).normalized
                           * _movingEntity.MaxSpeed;

            return (DesiredVelocity - _movingEntity.Velocity);
        }

        //this behavior is similar to seek but it attempts to arrive 
        //at the target position with a zero velocity
        Vector2 Arrive(Vector2 TargetPos,
                        Deceleration deceleration)
        {
            Vector2 ToTarget = TargetPos - _movingEntity.Position;

            //calculate the distance to the target
            float dist = ToTarget.magnitude;

            if (dist > 0)
            {
                //because Deceleration is enumerated as an int, this value is required
                //to provide fine tweaking of the deceleration..
                const float DecelerationTweaker = 0.3f;

                //calculate the speed required to reach the target given the desired
                //deceleration
                float speed = dist / ((float)deceleration * DecelerationTweaker);

                //make sure the velocity does not exceed the max
                speed = Mathf.Min(speed, _movingEntity.MaxSpeed);

                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2 DesiredVelocity = ToTarget * speed / dist;

                return (DesiredVelocity - _movingEntity.Velocity);
            }

            return new Vector2(0, 0);
        }

        /// <summary>
        /// this behavior creates a force that steers the agent towards the 
        ///evader
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Vector2 Pursuit(MovingEntity evader)
        {
            //if the evader is ahead and facing the agent then we can just seek
            //for the evader's current position.
            Vector2 ToEvader = evader.Position - _movingEntity.Position;

            float RelativeHeading = Vector2.Dot(_movingEntity.Heading, evader.Heading);

            if ((Vector2.Dot(ToEvader, _movingEntity.Heading) > 0) &&
                 (RelativeHeading < -0.95f))  //acos(0.95)=18 degs
            {
                return Seek(evader.Position);
            }

            //Not considered ahead so we predict where the evader will be.

            //the lookahead time is propotional to the distance between the evader
            //and the pursuer; and is inversely proportional to the sum of the
            //agent's velocities
            float LookAheadTime = ToEvader.magnitude /
                                  (_movingEntity.MaxSpeed + evader.Speed);

            //now seek to the predicted future position of the evader
            return Seek(evader.Position + evader.Velocity * LookAheadTime);
        }

        /// <summary>
        /// this behavior maintains a position, in the direction of offset
        /// from the target MovingEntity
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="offset"></param>
        /// <returns></returns>

        Vector2 OffsetPursuit(MovingEntity leader, Vector2 offset)
        {
            //calculate the offset's position in world space
            Vector2 WorldOffsetPos = offset.ToWorldSpace(leader.Heading,
                                                         leader.Position);

            Vector2 ToOffset = WorldOffsetPos - _movingEntity.Position;

            //the lookahead time is propotional to the distance between the leader
            //and the pursuer; and is inversely proportional to the sum of both
            //agent's velocities
            float LookAheadTime = ToOffset.magnitude /
                                  (_movingEntity.MaxSpeed + leader.Speed);

            //now Arrive at the predicted future position of the offset
            return Arrive(WorldOffsetPos + leader.Velocity * LookAheadTime, Deceleration.fast);
        }

        /// <summary>
        /// this behavior attempts to evade a pursuer
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Vector2 Evade(MovingEntity pursuer)
        {
            /* Not necessary to include the check for facing direction this time */

            Vector2 ToPursuer = pursuer.Position - _movingEntity.Position;

            //uncomment the following two lines to have Evade only consider pursuers 
            //within a 'threat range'
            const float ThreatRange = 100.0f;
            if (ToPursuer.sqrMagnitude > ThreatRange * ThreatRange) return new Vector2();

            //the lookahead time is propotional to the distance between the pursuer
            //and the pursuer; and is inversely proportional to the sum of the
            //agents' velocities
            float LookAheadTime = ToPursuer.magnitude /
                                   (_movingEntity.MaxSpeed + pursuer.Speed);

            //now flee away from predicted future position of the pursuer
            return Flee(pursuer.Position + pursuer.Velocity * LookAheadTime);
        }

        /// <summary>
        /// this behavior makes the agent wander about randomly
        /// </summary>
        /// <returns></returns>
        Vector2 Wander()
        {
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            float JitterThisTimeSlice = _wanderJitter * Time.deltaTime;

            //first, add a small random vector to the target's position
            _wanderTarget += new Vector2(UnityEngine.Random.Range(-1, 1) * JitterThisTimeSlice,
                                         UnityEngine.Random.Range(-1, 1) * JitterThisTimeSlice);

            //reproject this new vector back on to a unit circle
            _wanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            _wanderTarget *= _wanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = _wanderTarget + new Vector2(_wanderDistance, 0);

            //project the target into world space
            Vector2 Target = target.ToWorldSpace(_movingEntity.Heading,
                                                 _movingEntity.Position);

            //and steer towards it
            return Target - _movingEntity.Position;
        }

        /// <summary>
        /// this returns a steering force which will attempt to keep the agent 
        /// away from any obstacles it may encounter
        /// </summary>
        /// <param name="obstacles"></param>
        /// <returns></returns>
        SteeringParams _params = SteeringParams.Instance;
        Vector2 ObstacleAvoidance(List<Base2DEntity> obstacles)
        {
            /*
            //the detection box length is proportional to the agent's velocity
            _boxLength = _params.MinDetectionBoxLength +
                            (_movingEntity.Speed / _movingEntity.MaxSpeed) *
                            _params.MinDetectionBoxLength;

            //tag all obstacles within range of the box for processing
            _movingEntity.World().TagObstaclesWithinViewRange(_movingEntity, _boxLength);

            //this will keep track of the closest intersecting obstacle (CIB)
            BaseGameEntity* ClosestIntersectingObstacle = NULL;

            //this will be used to track the distance to the CIB
            double DistToClosestIP = MaxDouble;

            //this will record the transformed local coordinates of the CIB
            Vector2 LocalPosOfClosestObstacle;

            std::vector<BaseGameEntity*>::const_iterator curOb = obstacles.begin();

            while (curOb != obstacles.end())
            {
                //if the obstacle has been tagged within range proceed
                if ((*curOb).IsTagged())
                {
                    //calculate this obstacle's position in local space
                    Vector2 LocalPos = PointToLocalSpace((*curOb).Pos(),
                                                           _movingEntity.Heading(),
                                                           _movingEntity.Side(),
                                                           _movingEntity.Pos());

                    //if the local position has a negative x value then it must lay
                    //behind the agent. (in which case it can be ignored)
                    if (LocalPos.x >= 0)
                    {
                        //if the distance from the x axis to the object's position is less
                        //than its radius + half the width of the detection box then there
                        //is a potential intersection.
                        double ExpandedRadius = (*curOb).BRadius() + _movingEntity.BRadius();

                        if (fabs(LocalPos.y) < ExpandedRadius)
                        {
                            //now to do a line/circle intersection test. The center of the 
                            //circle is represented by (cX, cY). The intersection points are 
                            //given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
                            //We only need to look at the smallest positive value of x because
                            //that will be the closest point of intersection.
                            double cX = LocalPos.x;
                            double cY = LocalPos.y;

                            //we only need to calculate the sqrt part of the above equation once
                            double SqrtPart = sqrt(ExpandedRadius * ExpandedRadius - cY * cY);

                            double ip = cX - SqrtPart;

                            if (ip <= 0.0)
                            {
                                ip = cX + SqrtPart;
                            }

                            //test to see if this is the closest so far. If it is keep a
                            //record of the obstacle and its local coordinates
                            if (ip < DistToClosestIP)
                            {
                                DistToClosestIP = ip;

                                ClosestIntersectingObstacle = *curOb;

                                LocalPosOfClosestObstacle = LocalPos;
                            }
                        }
                    }
                }

                ++curOb;
            }

            //if we have found an intersecting obstacle, calculate a steering 
            //force away from it
            Vector2 SteeringForce;

            if (ClosestIntersectingObstacle)
            {
                //the closer the agent is to an object, the stronger the 
                //steering force should be
                double multiplier = 1.0 + (_dBoxLength - LocalPosOfClosestObstacle.x) /
                                    _dBoxLength;

                //calculate the lateral force
                SteeringForce.y = (ClosestIntersectingObstacle.BRadius() -
                                   LocalPosOfClosestObstacle.y) * multiplier;

                //apply a braking force proportional to the obstacles distance from
                //the vehicle. 
                const double BrakingWeight = 0.2;

                SteeringForce.x = (ClosestIntersectingObstacle.BRadius() -
                                   LocalPosOfClosestObstacle.x) *
                                   BrakingWeight;
            }

            //finally, convert the steering vector from local to world space
            return VectorToWorldSpace(SteeringForce,
                                      _movingEntity.Heading(),
                                      _movingEntity.Side());
                                */
            return new Vector2();
        }

        /// <summary>
        /// this returns a steering force which will keep the agent away from any
        /// walls it may encounter
        /// </summary>
        /// <param name="walls"></param>
        /// <returns></returns>
        Vector2 WallAvoidance(List<Wall2D> walls)
        {
            //the feelers are contained in a std::vector, m_Feelers
            CreateFeelers();

            float DistToThisIP = 0.0f;
            float DistToClosestIP = float.MaxValue;

            //this will hold an index into the vector of walls
            int ClosestWall = -1;

            Vector2 SteeringForce = new Vector2(),
                      point = new Vector2(),         //used for storing temporary info
                      ClosestPoint = new Vector2();  //holds the closest intersection point

            //examine each feeler in turn
            foreach(var flr in _feelers)
            {
                //run through each wall checking for any intersection points
                //foreach(var w in walls)
                for(int i = 0; i < walls.Count; ++i)
                {
                    if (walls[i].IsIntersection(_movingEntity.Position,
                                         flr,
                                     out DistToThisIP,
                                     out point))
                    {
                        //is this the closest found so far? If so keep a record
                        if (DistToThisIP < DistToClosestIP)
                        {
                            DistToClosestIP = DistToThisIP;

                            ClosestWall = i;

                            ClosestPoint = point;
                        }
                    }
                }//next wall


                //if an intersection point has been detected, calculate a force  
                //that will direct the agent away
                if (ClosestWall >= 0)
                {
                    //calculate by what distance the projected position of the agent
                    //will overshoot the wall
                    Vector2 OverShoot = flr - ClosestPoint;

                    //create a force in the direction of the wall normal, with a 
                    //magnitude of the overshoot
                    SteeringForce = walls[ClosestWall].GetTangentNormal(ClosestPoint) * OverShoot.magnitude;
                }

            }//next feeler

            return SteeringForce;
        }


        /// <summary>
        /// given a series of Vector2Ds, this method produces a force that will
        /// move the agent along the waypoints in order
        /// </summary>
        /// <returns></returns>
        Vector2 FollowPath()
        {
            return new Vector2();
        }


        /// <summary>
        /// this results in a steering force that attempts to steer the MovingEntity
        ///to the center of the vector connecting two moving agents.
        /// </summary>
        /// <param name="AgentA"></param>
        /// <param name="AgentB"></param>
        /// <returns></returns>
        Vector2 Interpose(MovingEntity AgentA,
                          MovingEntity AgentB)
        {
            //first we need to figure out where the two agents are going to be at 
            //time T in the future. This is approximated by determining the time
            //taken to reach the mid way point at the current time at at max speed.
            Vector2 MidPoint = (AgentA.Position + AgentB.Position) / 2.0f;

            float TimeToReachMidPoint = Vector2.Distance(_movingEntity.Position, MidPoint) /
                                         _movingEntity.MaxSpeed;

            //now we have T, we assume that agent A and agent B will continue on a
            //straight trajectory and extrapolate to get their future positions
            Vector2 APos = AgentA.Position + AgentA.Velocity * TimeToReachMidPoint;
            Vector2 BPos = AgentB.Position + AgentB.Velocity * TimeToReachMidPoint;

            //calculate the mid point of these predicted positions
            MidPoint = (APos + BPos) / 2.0f;

            //then steer to Arrive at it
            return Arrive(MidPoint, Deceleration.fast);
        }

        /// <summary>
        /// given another agent position to hide from and a list of BaseGameEntitys this
        /// method attempts to put an obstacle between itself and its opponent
        /// </summary>
        /// <param name="hunter"></param>
        /// <param name="obstacles"></param>
        /// <returns></returns>

        Vector2 Hide(MovingEntity hunter, List<Base2DEntity> obstacles)
        {
            float DistToClosest = float.MaxValue;
            Vector2 BestHidingSpot = new Vector2();

            // List<GameObject> curOb = obstacles.begin();
            //std::vector<BaseGameEntity*>::const_iterator closest;

            Base2DEntity closest;
            foreach (var curOb in obstacles)
            {
                //calculate the position of the hiding spot for this obstacle
                Vector2 HidingSpot = GetHidingPosition(curOb.Position,
                                                       curOb.BoundingRadius,
                                                       hunter.Position);

                //work in distance-squared space to find the closest hiding
                //spot to the agent
                float dist = HidingSpot.SqrDistance(_movingEntity.Position);

                if (dist < DistToClosest)
                {
                    DistToClosest = dist;

                    BestHidingSpot = HidingSpot;

                    closest = curOb;
                }
            }//end while

            //if no suitable obstacles found then Evade the hunter
            if (DistToClosest == float.MaxValue)
            {
                return Evade(hunter);
            }

            //else use Arrive on the hiding spot
            return Arrive(BestHidingSpot, Deceleration.fast);
        }

        /// <summary>
        /// Given the position of a hunter, and the position and radius of
        /// an obstacle, this method calculates a position DistanceFromBoundary 
        /// away from its bounding radius and directly opposite the hunter
        /// </summary>
        /// <returns></returns>
        Vector2 GetHidingPosition(Vector2 posOb,
                                  float radiusOb,
                                  Vector2 posHunter)
        {
            //calculate how far away the agent is to be from the chosen obstacle's
            //bounding radius
            const float DistanceFromBoundary = 30.0f;
            float DistAway = radiusOb + DistanceFromBoundary;

            //calculate the heading toward the object from the hunter
            Vector2 ToOb = (posOb - posHunter).normalized;

            //scale it to size and add to the obstacles position to get
            //the hiding spot.
            return (ToOb * DistAway) + posOb;
        }

        // -- Group Behaviors -- //

        Vector2 Cohesion(List<MovingEntity> neighbors)
        {
            //first find the center of mass of all the agents
            Vector2 CenterOfMass = new Vector2();
            Vector2 SteeringForce = new Vector2();

            int NeighborCount = 0;

            //iterate through the neighbors and sum up all the position vectors
            for (int a = 0; a < neighbors.Count; ++a)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined is close enough ***also make sure it doesn't
                //include the evade target ***
                if ((neighbors[a] != _movingEntity) && neighbors[a].IsTagged() &&
                  (neighbors[a] != _targetAgent1))
                {
                    CenterOfMass += neighbors[a].Position;

                    ++NeighborCount;
                }
            }

            if (NeighborCount > 0)
            {
                //the center of mass is the average of the sum of positions
                CenterOfMass /= (float)NeighborCount;

                //now seek towards that position
                SteeringForce = Seek(CenterOfMass);
            }

            //the magnitude of cohesion is usually much larger than separation or
            //allignment so it usually helps to normalize it.
            return SteeringForce.normalized;
        }

        Vector2 Separation(List<MovingEntity> neighbors)
        {
            Vector2 SteeringForce = new Vector2();

            for (int a = 0; a < neighbors.Count; ++a)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough. ***also make sure it doesn't
                //include the evade target ***
                if ((neighbors[a] != _movingEntity) && neighbors[a].IsTagged() &&
                  (neighbors[a] != _targetAgent1))
                {
                    Vector2 ToAgent = _movingEntity.Position - neighbors[a].Position;

                    //scale the force inversely proportional to the agents distance  
                    //from its neighbor.
                    SteeringForce += ToAgent.normalized / ToAgent.magnitude;
                }
            }

            return SteeringForce;
        }

        Vector2 Alignment(List<MovingEntity> agents)
        {
            //This will record the average heading of the neighbors
            Vector2 AverageHeading = new Vector2();

            //This count the number of vehicles in the neighborhood
            float NeighborCount = 0.0f;

            //iterate through the neighbors and sum up all the position vectors
            foreach (var pV in _movingEntity.World.CellSpace)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined  is close enough
                if (pV != _movingEntity)
                {
                    AverageHeading += pV.Heading;

                    ++NeighborCount;
                }

            }

            //if the neighborhood contained one or more vehicles, average their
            //heading vectors.
            if (NeighborCount > 0.0)
            {
                AverageHeading /= NeighborCount;

                AverageHeading -= _movingEntity.Heading;
            }

            return AverageHeading;
        }

        //the following three are the same as above but they use cell-space
        //partitioning to find the neighbors

        /// <summary>
        /// returns a steering force that attempts to move the agent towards the
        /// center of mass of the agents in its immediate area
        /// USES SPACIAL PARTITIONING
        /// </summary>
        /// <param name="agents"></param>
        /// <returns></returns>
        Vector2 CohesionPlus(List<MovingEntity> agents)
        {
            //first find the center of mass of all the agents
            Vector2 CenterOfMass = new Vector2(),
                   SteeringForce = new Vector2();

            int NeighborCount = 0;

            //iterate through the neighbors and sum up all the position vectors
            foreach (var pV in _movingEntity.World.CellSpace)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined is close enough
                if (pV != _movingEntity)
                {
                    CenterOfMass += pV.Position;

                    ++NeighborCount;
                }
            }

            if (NeighborCount > 0)
            {
                //the center of mass is the average of the sum of positions
                CenterOfMass /= (float)NeighborCount;

                //now seek towards that position
                SteeringForce = Seek(CenterOfMass);
            }

            //the magnitude of cohesion is usually much larger than separation or
            //allignment so it usually helps to normalize it.
            return (SteeringForce).normalized;
        }

        Vector2 SeparationPlus(List<MovingEntity> agents)
        {
            Vector2 SteeringForce = new Vector2();

            //iterate through the neighbors and sum up all the position vectors
            foreach (var pV in _movingEntity.World.CellSpace)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough
                if (pV != _movingEntity)
                {
                    Vector2 ToAgent = _movingEntity.Position - pV.Position;

                    //scale the force inversely proportional to the agents distance  
                    //from its neighbor.
                    SteeringForce += ToAgent.normalized / ToAgent.magnitude;
                }
            }
            return SteeringForce;
        }

        /// <summary>
        /// returns a force that attempts to align this agents heading with that
        /// of its neighbors
        /// USES SPACIAL PARTITIONING
        /// </summary>
        /// <param name="agents"></param>
        /// <returns></returns>
        Vector2 AlignmentPlus(List<MovingEntity> agents)
        {
            //This will record the average heading of the neighbors
            Vector2 AverageHeading = new Vector2();

            //This count the number of vehicles in the neighborhood
            float NeighborCount = 0.0f;

            //iterate through the neighbors and sum up all the position vectors
            foreach (var pV in _movingEntity.World.CellSpace)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined  is close enough
                if (pV != _movingEntity)
                {
                    AverageHeading += pV.Heading;

                    ++NeighborCount;
                }
            }
            //if the neighborhood contained one or more vehicles, average their
            //heading vectors.
            if (NeighborCount > 0.0)
            {
                AverageHeading /= NeighborCount;

                AverageHeading -= _movingEntity.Heading;
            }

            return AverageHeading;
        }

        /// <summary>
        /// calculates and sums the steering forces from any active behaviors
        /// </summary>
        /// <returns></returns>
        Vector2 CalculateWeightedSum()
        {
            if (On(behavior_type.wall_avoidance))
            {
                _steeringForce += WallAvoidance(_movingEntity.World.Walls) *
                                     _weightWallAvoidance;
            }

            /*
            if (On(behavior_type.obstacle_avoidance))
            {
              _steeringForce += ObstacleAvoidance(_movingEntity.World.Obstacles()) *
                        _WeightObstacleAvoidance;
            }
             */

            if (On(behavior_type.evade))
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("Evade target not assigned");
                }

                _steeringForce += Evade(_targetAgent1) * _weightEvade;
            }


            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)
            if (!isSpacePartitioningOn())
            {
                if (On(behavior_type.separation))
                {
                    _steeringForce += Separation(_movingEntity.World.Agents) * _weightSeparation;
                }

                if (On(behavior_type.allignment))
                {
                    _steeringForce += Alignment(_movingEntity.World.Agents) * _weightAlignment;
                }

                if (On(behavior_type.cohesion))
                {
                    _steeringForce += Cohesion(_movingEntity.World.Agents) * _weightCohesion;
                }
            }
            else
            {
                if (On(behavior_type.separation))
                {
                    _steeringForce += SeparationPlus(_movingEntity.World.Agents) * _weightSeparation;
                }

                if (On(behavior_type.allignment))
                {
                    _steeringForce += AlignmentPlus(_movingEntity.World.Agents) * _weightSeparation;
                }

                if (On(behavior_type.cohesion))
                {
                    _steeringForce += CohesionPlus(_movingEntity.World.Agents) * _weightSeparation;
                }
            }


            if (On(behavior_type.wander))
            {
                _steeringForce += Wander() * _weightWander;
            }

            if (On(behavior_type.seek))
            {
                _steeringForce += Seek(_movingEntity.World.Crosshair) * _weightSeek;
            }

            if (On(behavior_type.flee))
            {
                _steeringForce += Flee(_movingEntity.World.Crosshair) * _weightFlee;
            }

            if (On(behavior_type.arrive))
            {
                _steeringForce += Arrive(_movingEntity.World.Crosshair, _deceleration) * _weightArrive;
            }

            if (On(behavior_type.pursuit))
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("pursuit target not assigned");
                }
                //assert(m_pTargetAgent1 && "pursuit target not assigned");

                _steeringForce += Pursuit(_targetAgent1) * _weightPursuit;
            }

            if (On(behavior_type.offset_pursuit))
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("pursuit target not assigned");
                }
                if (_offset.magnitude == 0f)
                {
                    Debug.LogError("No offset assigned");
                }

                _steeringForce += OffsetPursuit(_targetAgent1, _offset) * _weightOffsetPursuit;
            }

            if (On(behavior_type.interpose))
            {
                if (_targetAgent1 == null || _targetAgent2 == null)
                {
                    Debug.LogError("Interpose agents not assigned");
                }

                _steeringForce += Interpose(_targetAgent1, _targetAgent2) * _weightInterpose;
            }
            /*
            if (On(behavior_type.hide))
            {
              if(_targetAgent1 == null)
              {
                  Debug.LogError("Hide target not assigned");
              }

              _steeringForce += Hide(_targetAgent1, _movingEntity.World.Obstacles) * _WeightHide;
            }
            */
            if (On(behavior_type.follow_path))
            {
                _steeringForce += FollowPath() * _weightFollowPath;
            }

            _steeringForce.Truncate(_movingEntity.MaxForce);

            return _steeringForce;
        }

        /// <summary>
        /// random float between 0 and 1
        /// </summary>
        /// <returns></returns>
        float RandFloat()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

        Vector2 CalculatePrioritized()
        {
            Vector2 force;

            if (On(behavior_type.wall_avoidance))
            {
                force = WallAvoidance(_movingEntity.World.Walls) *
                        _weightWallAvoidance;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            /*
            if (On(behavior_type.obstacle_avoidance))
            {
                force = ObstacleAvoidance(_movingEntity.World.Obstacles()) *
                        _weightObstacleAvoidance;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }
            */
            if (On(behavior_type.evade))
            {
                if(_targetAgent1 == null)
                {
                    Debug.LogError("Evade target not assigned");
                }
                force = Evade(_targetAgent1) * _weightEvade;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(behavior_type.flee))
            {
                force = Flee(_movingEntity.World.Crosshair) * _weightFlee;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }



            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)
            if (!isSpacePartitioningOn())
            {
                if (On(behavior_type.separation))
                {
                    force = Separation(_movingEntity.World.Agents) * _weightSeparation;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }

                if (On(behavior_type.allignment))
                {
                    force = Alignment(_movingEntity.World.Agents) * _weightAlignment;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }

                if (On(behavior_type.cohesion))
                {
                    force = Cohesion(_movingEntity.World.Agents) * _weightCohesion;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }
            }

            else
            {

                if (On(behavior_type.separation))
                {
                    force = SeparationPlus(_movingEntity.World.Agents) * _weightSeparation;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }

                if (On(behavior_type.allignment))
                {
                    force = AlignmentPlus(_movingEntity.World.Agents) * _weightAlignment;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }

                if (On(behavior_type.cohesion))
                {
                    force = CohesionPlus(_movingEntity.World.Agents) * _weightCohesion;

                    if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
                }
            }

            if (On(behavior_type.seek))
            {
                force = Seek(_movingEntity.World.Crosshair) * _weightSeek;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }


            if (On(behavior_type.arrive))
            {
                force = Arrive(_movingEntity.World.Crosshair, _deceleration) * _weightArrive;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(behavior_type.wander))
            {
                force = Wander() * _weightWander;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(behavior_type.pursuit))
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("pursuit target not assigned");
                }
                force = Pursuit(_targetAgent1) * _weightPursuit;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(behavior_type.offset_pursuit))
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("pursuit target not assigned");
                }
                if (_offset.IsZero())
                {
                    Debug.LogError("No offset assigned");
                }

                force = OffsetPursuit(_targetAgent1, _offset);

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            if (On(behavior_type.interpose))
            {
                if (_targetAgent1 == null || _targetAgent2 == null)
                {
                    Debug.LogError("Interpose agents not assigned");
                }

                force = Interpose(_targetAgent1, _targetAgent2) * _weightInterpose;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            /*
            if (On(behavior_type.hide))
            {
                assert(m_pTargetAgent1 && "Hide target not assigned");

                force = Hide(m_pTargetAgent1, _movingEntity.World.Obstacles()) * _weightHide;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }
            */

            if (On(behavior_type.follow_path))
            {
                force = FollowPath() * _weightFollowPath;

                if (!AccumulateForce(_steeringForce, force)) return _steeringForce;
            }

            return _steeringForce;
        }
        Vector2 CalculateDithered()
        {
            //reset the steering force
            _steeringForce.Zero();

            if (On(behavior_type.wall_avoidance) && RandFloat() < SteeringParams.Instance.prWallAvoidance)
            {
                _steeringForce = WallAvoidance(_movingEntity.World.Walls) *
                                     _weightWallAvoidance / SteeringParams.Instance.prWallAvoidance;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            /*
            if (On(behavior_type.obstacle_avoidance) && RandFloat() < SteeringParams.Instance.prObstacleAvoidance)
            {
                _steeringForce += ObstacleAvoidance(_movingEntity.World.Obstacles()) *
                        _weightObstacleAvoidance / SteeringParams.Instance.prObstacleAvoidance;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }
            */
            if (!isSpacePartitioningOn())
            {
                if (On(behavior_type.separation) && RandFloat() < SteeringParams.Instance.prSeparation)
                {
                    _steeringForce += Separation(_movingEntity.World.Agents) *
                                        _weightSeparation / SteeringParams.Instance.prSeparation;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }
            }

            else
            {
                if (On(behavior_type.separation) && RandFloat() < SteeringParams.Instance.prSeparation)
                {
                    _steeringForce += SeparationPlus(_movingEntity.World.Agents) *
                                        _weightSeparation / SteeringParams.Instance.prSeparation;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }
            }


            if (On(behavior_type.flee) && RandFloat() < SteeringParams.Instance.prFlee)
            {
                _steeringForce += Flee(_movingEntity.World.Crosshair) * _weightFlee / SteeringParams.Instance.prFlee;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(behavior_type.evade) && RandFloat() < SteeringParams.Instance.prEvade)
            {
                if (_targetAgent1 == null)
                {
                    Debug.LogError("Evade target not assigned");
                }

                _steeringForce += Evade(_targetAgent1) * _weightEvade / SteeringParams.Instance.prEvade;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }


            if (!isSpacePartitioningOn())
            {
                if (On(behavior_type.allignment) && RandFloat() < SteeringParams.Instance.prAlignment)
                {
                    _steeringForce += Alignment(_movingEntity.World.Agents) *
                                        _weightAlignment / SteeringParams.Instance.prAlignment;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }

                if (On(behavior_type.cohesion) && RandFloat() < SteeringParams.Instance.prCohesion)
                {
                    _steeringForce += Cohesion(_movingEntity.World.Agents) *
                                        _weightCohesion / SteeringParams.Instance.prCohesion;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }
            }
            else
            {
                if (On(behavior_type.allignment) && RandFloat() < SteeringParams.Instance.prAlignment)
                {
                    _steeringForce += AlignmentPlus(_movingEntity.World.Agents) *
                                        _weightAlignment / SteeringParams.Instance.prAlignment;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }

                if (On(behavior_type.cohesion) && RandFloat() < SteeringParams.Instance.prCohesion)
                {
                    _steeringForce += CohesionPlus(_movingEntity.World.Agents) *
                                        _weightCohesion / SteeringParams.Instance.prCohesion;

                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }
            }

            if (On(behavior_type.wander) && RandFloat() < SteeringParams.Instance.prWander)
            {
                _steeringForce += Wander() * _weightWander / SteeringParams.Instance.prWander;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(behavior_type.seek) && RandFloat() < SteeringParams.Instance.prSeek)
            {
                _steeringForce += Seek(_movingEntity.World.Crosshair) * _weightSeek / SteeringParams.Instance.prSeek;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            if (On(behavior_type.arrive) && RandFloat() < SteeringParams.Instance.prArrive)
            {
                _steeringForce += Arrive(_movingEntity.World.Crosshair, _deceleration) *
                                    _weightArrive / SteeringParams.Instance.prArrive;

                if (!_steeringForce.IsZero())
                {
                    _steeringForce.Truncate(_movingEntity.MaxForce);

                    return _steeringForce;
                }
            }

            return _steeringForce;
        }
          





        public const float WanderRad = 1.2f;
        //distance the wander circle is projected in front of the agent
        public const float WanderDist = 2.0f;
        //the maximum amount of displacement along the circle each frame
        public const float WanderJitterPerSec = 80.0f;

        //used in path following
        public const float WaypointSeekDist = 20;

        public void Init(MovingEntity agent)
        {
            _movingEntity = agent;
            _flags = 0;
            _detectionBoxLength = SteeringParams.Instance.MinDetectionBoxLength;
            _weightCohesion = SteeringParams.Instance.CohesionWeight;
            _weightAlignment = SteeringParams.Instance.AlignmentWeight;
            _weightSeparation = SteeringParams.Instance.SeparationWeight;
            _weightObstacleAvoidance = SteeringParams.Instance.ObstacleAvoidanceWeight;
            _weightWander = SteeringParams.Instance.WanderWeight;
            _weightWallAvoidance = SteeringParams.Instance.WallAvoidanceWeight;
            _viewDistance = SteeringParams.Instance.ViewDistance;
            _wallDetectionFeelerLength = SteeringParams.Instance.WallDetectionFeelerLength;
            _feelers = new List<Vector2>(3);
            _deceleration = Deceleration.normal;
            _targetAgent1 = null;
            _targetAgent2 = null;
            _wanderDistance = WanderDist;
            _wanderJitter = WanderJitterPerSec;
            _wanderRadius = WanderRad;
            _waypointSeekDistSq = WaypointSeekDist * WaypointSeekDist;
            _weightSeek = SteeringParams.Instance.SeekWeight;
            _weightFlee = SteeringParams.Instance.FleeWeight;
            _weightArrive = SteeringParams.Instance.ArriveWeight;
            _weightPursuit = SteeringParams.Instance.PursuitWeight;
            _weightOffsetPursuit = SteeringParams.Instance.OffsetPursuitWeight;
            _weightInterpose = SteeringParams.Instance.InterposeWeight;
            _weightHide = SteeringParams.Instance.HideWeight;
            _weightEvade = SteeringParams.Instance.EvadeWeight;
            _weightFollowPath = SteeringParams.Instance.FollowPathWeight;
            _cellSpaceOn = false;
            _summingMethod = summing_method.prioritized;
        }

        //calculates and sums the steering forces from any active behaviors
        public Vector2 Calculate()
        {
            //reset the steering force
            _steeringForce.Set(0f, 0f);

            //use space partitioning to calculate the neighbours of this vehicle
            //if switched on. If not, use the standard tagging system
            if (!isSpacePartitioningOn())
            {
                //tag neighbors if any of the following 3 group behaviors are switched on
                if (On(behavior_type.separation) || On(behavior_type.allignment) || On(behavior_type.cohesion))
                {
                    _movingEntity.World.TagVehiclesWithinViewRange(_movingEntity, _viewDistance);
                }
            }
            else
            {
                //calculate neighbours in cell-space if any of the following 3 group
                //behaviors are switched on
                if (On(behavior_type.separation) || On(behavior_type.allignment) || On(behavior_type.cohesion))
                {
                    _movingEntity.World.CellSpace.CalculateNeighbors(_movingEntity.Position, _viewDistance);
                }
            }

            switch (_summingMethod)
            {
                case summing_method.weighted_average:

                    _steeringForce = CalculateWeightedSum(); break;

                case summing_method.prioritized:

                    _steeringForce = CalculatePrioritized(); break;

                case summing_method.dithered:

                    _steeringForce = CalculateDithered(); break;

                default:
                    _steeringForce = new Vector2(0, 0);
                    break;

            }//end switch

            return _steeringForce;
        }

        /// <summary>
        /// calculates the component of the steering force that is parallel
        //with the MovingEntity heading
        /// </summary>
        /// <returns></returns>
        public float ForwardComponent()
        {
            return Vector2.Dot(_movingEntity.Heading, _steeringForce);
        }

        /// <summary>
        /// calculates the component of the steering force that is perpendicuar
        /// with the MovingEntity heading
        /// </summary>
        /// <returns></returns>

        public float SideComponent()
        {
            return Vector2.Dot(_movingEntity.Side, _steeringForce);
        }


        /// <summary>
        /// renders visual aids and info for seeing how each behavior is
        /// calculated
        /// </summary>

        public void RenderAids()
        {

        }

        public void SetTarget(Vector2 t) { _target = t; }

        public void SetTargetAgent1(MovingEntity Agent) { _targetAgent1 = Agent; }
        public void SetTargetAgent2(MovingEntity Agent) { _targetAgent1 = Agent; }

        public void SetOffset(Vector2 offset) { _offset = offset; }
        public Vector2 GetOffset() { return _offset; }

        /*
        public void SetPath(LinkedList<Vector2> new_path)
        {
            _path.Set(new_path);
        }
        public void CreateRandomPath(int num_waypoints, int mx, int my, int cx, int cy)
        {
            _path.CreateRandomPath(num_waypoints, mx, my, cx, cy);
        }
        */

        public Vector2 Force() { return _steeringForce; }

        public void ToggleSpacePartitioningOnOff() { _cellSpaceOn = !_cellSpaceOn; }
        public bool IsSpacePartitioningOn() { return _cellSpaceOn; }

        public void SetSummingMethod(summing_method sm) { _summingMethod = sm; }


        public void FleeOn() { _flags |= behavior_type.flee; }
        public void SeekOn() { _flags |= behavior_type.seek; }
        public void ArriveOn() { _flags |= behavior_type.arrive; }
        public void WanderOn() { _flags |= behavior_type.wander; }
        public void PursuitOn(MovingEntity v) { _flags |= behavior_type.pursuit; _targetAgent1 = v; }
        public void EvadeOn(MovingEntity v) { _flags |= behavior_type.evade; _targetAgent1 = v; }
        public void CohesionOn() { _flags |= behavior_type.cohesion; }
        public void SeparationOn() { _flags |= behavior_type.separation; }
        public void AlignmentOn() { _flags |= behavior_type.allignment; }
        public void ObstacleAvoidanceOn() { _flags |= behavior_type.obstacle_avoidance; }
        public void WallAvoidanceOn() { _flags |= behavior_type.wall_avoidance; }
        public void FollowPathOn() { _flags |= behavior_type.follow_path; }
        public void InterposeOn(MovingEntity v1, MovingEntity v2) { _flags |= behavior_type.interpose; _targetAgent1 = v1; _targetAgent2 = v2; }
        public void HideOn(MovingEntity v) { _flags |= behavior_type.hide; _targetAgent1 = v; }
        public void OffsetPursuitOn(MovingEntity v1, Vector2 offset) { _flags |= behavior_type.offset_pursuit; _offset = offset; _targetAgent1 = v1; }
        public void FlockingOn() { CohesionOn(); AlignmentOn(); SeparationOn(); WanderOn(); }

        public void FleeOff() { if (On(behavior_type.flee)) _flags ^= behavior_type.flee; }
        public void SeekOff() { if (On(behavior_type.seek)) _flags ^= behavior_type.seek; }
        public void ArriveOff() { if (On(behavior_type.arrive)) _flags ^= behavior_type.arrive; }
        public void WanderOff() { if (On(behavior_type.wander)) _flags ^= behavior_type.wander; }
        public void PursuitOff() { if (On(behavior_type.pursuit)) _flags ^= behavior_type.pursuit; }
        public void EvadeOff() { if (On(behavior_type.evade)) _flags ^= behavior_type.evade; }
        public void CohesionOff() { if (On(behavior_type.cohesion)) _flags ^= behavior_type.cohesion; }
        public void SeparationOff() { if (On(behavior_type.separation)) _flags ^= behavior_type.separation; }
        public void AlignmentOff() { if (On(behavior_type.allignment)) _flags ^= behavior_type.allignment; }
        public void ObstacleAvoidanceOff() { if (On(behavior_type.obstacle_avoidance)) _flags ^= behavior_type.obstacle_avoidance; }
        public void WallAvoidanceOff() { if (On(behavior_type.wall_avoidance)) _flags ^= behavior_type.wall_avoidance; }
        public void FollowPathOff() { if (On(behavior_type.follow_path)) _flags ^= behavior_type.follow_path; }
        public void InterposeOff() { if (On(behavior_type.interpose)) _flags ^= behavior_type.interpose; }
        public void HideOff() { if (On(behavior_type.hide)) _flags ^= behavior_type.hide; }
        public void OffsetPursuitOff() { if (On(behavior_type.offset_pursuit)) _flags ^= behavior_type.offset_pursuit; }
        public void FlockingOff() { CohesionOff(); AlignmentOff(); SeparationOff(); WanderOff(); }

        public bool isFleeOn() { return On(behavior_type.flee); }
        public bool isSeekOn() { return On(behavior_type.seek); }
        public bool isArriveOn() { return On(behavior_type.arrive); }
        public bool isWanderOn() { return On(behavior_type.wander); }
        public bool isPursuitOn() { return On(behavior_type.pursuit); }
        public bool isEvadeOn() { return On(behavior_type.evade); }
        public bool isCohesionOn() { return On(behavior_type.cohesion); }
        public bool isSeparationOn() { return On(behavior_type.separation); }
        public bool isAlignmentOn() { return On(behavior_type.allignment); }
        public bool isObstacleAvoidanceOn() { return On(behavior_type.obstacle_avoidance); }
        public bool isWallAvoidanceOn() { return On(behavior_type.wall_avoidance); }
        public bool isFollowPathOn() { return On(behavior_type.follow_path); }
        public bool isInterposeOn() { return On(behavior_type.interpose); }
        public bool isHideOn() { return On(behavior_type.hide); }
        public bool isOffsetPursuitOn() { return On(behavior_type.offset_pursuit); }


        public float BoxLength { get { return _detectionBoxLength; } }
        List<Vector2> GetFeelers() { return _feelers; }


        float WanderJitter() { return _wanderJitter; }
        float WanderDistance() { return _wanderDistance; }
        float WanderRadius() { return _wanderRadius; }

        float SeparationWeight() { return _weightSeparation; }
        float AlignmentWeight() { return _weightAlignment; }
        float CohesionWeight() { return _weightCohesion; }

    }
}
