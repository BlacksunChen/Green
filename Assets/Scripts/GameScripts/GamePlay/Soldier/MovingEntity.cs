using UnityEngine;
using System.Collections;
using Utilities;
//------------------------------------------------------------------------
//
//  Name:   MovingEntity.h
//
//  Desc:   A base class defining an entity that moves. The entity has 
//          a local coordinate system and members for defining its
//          mass and velocity.
//
//  Author: Mat Buckland 2003 (fup@ai-junkie.com)
//
//------------------------------------------------------------------------

namespace Green
{
    public class MovingEntity : Base2DEntity
    {
        #region private field
        //the steering behavior class
        Dithered _behaviors = null;
        Dithered Behaviors
        {
            get
            {
                if(_behaviors == null)
                {
                    _behaviors = GetComponent<Dithered>();
                }
                return _behaviors;
            }
        }

        [SerializeField, SetProperty("Velocity")]
        Vector2 _velocity = new Vector2(0.39f, 2.04f);

        //a normalized vector pointing in the direction the entity is heading. 
        [SerializeField, SetProperty("Heading")]
        Vector2 _heading = new Vector2(1f, 0f);

        //a vector perpendicular to the heading vector
        [SerializeField, SetProperty("Side")]
        Vector2 _side;

        [SerializeField, SetProperty("Mass")]
        float _mass = 0.13f;

        //the maximum speed this entity may travel at.
        [SerializeField, SetProperty("MaxSpeed")]
        float _maxSpeed = SteeringParams.Instance.MaxSpeed;

        //the maximum force this entity can produce to power itself 
        //(think rockets and thrust)
        [SerializeField, SetProperty("MaxForce")]
        float _maxForce = SteeringParams.Instance.MaxSteeringForce;

        //the maximum rate (radians per second)this vehicle can rotate         
        [SerializeField, SetProperty("MaxTurnRate")]
        float _maxTurnRate = SteeringParams.Instance.MaxTurnRatePerSecond;

        [SerializeField, SetProperty("World")]
        GameWorld _world;

        //some steering behaviors give jerky looking movement. The
        //following members are used to smooth the vehicle's heading
        SmootherVector _headingSmoother;

        //this vector represents the average of the vehicle's heading
        //vector smoothed over the last few frames
        Vector2 _smoothedHeading;

        //when true, smoothing is active
        bool _smoothingOn = false;

        #endregion
        // Vector2 _position;
        #region Property

        public Vector2 SmoothedHeading
        {
            get
            {
                return _smoothedHeading;
            }
            set
            {
                _smoothedHeading = value;
            }
        }

        public Dithered Steering
        {
            get
            {
                return _behaviors;
            }
            set
            {
                _behaviors = value;
            }
        }
        public GameWorld World
        {
            get
            {
                return _world;
            }
            set
            {
                _world = value;
            }
        }
        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = value;
            }
        }

        public Vector2 Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
            }
        }

		public static  float MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
            }
        }

        public float MaxForce
        {
            get
            {
                return _maxForce;
            }
            set
            {
                _maxForce = value;
            }
        }


        public bool IsSpeedMaxedOut()
        {
            return _maxSpeed * _maxSpeed >= _velocity.SqrMagnitude();
        }

        public float Speed
        {
            get
            {
                return _velocity.SqrMagnitude();
            }
        }
        public float SpeedSq
        {
            get
            {
                return _velocity.SqrMagnitude();
            }
        }

        public Vector2 Heading
        {
            get
            {
                return _heading;
            }
            //------------------------- SetHeading ----------------------------------------
            //
            //  first checks that the given heading is not a vector of zero length. If the
            //  new heading is valid this fumction sets the entity's heading and side 
            //  vectors accordingly
            //-----------------------------------------------------------------------------
            set
            {
                if ((value.SqrMagnitude() < float.Epsilon))
                {
                    Debug.LogErrorFormat("heading is a vector of zero length");
                }

                _heading = value;
                //the side vector must always be perpendicular to the heading
                _side = _heading.Perpendicular();
            }
        }

        public float MaxTurnRate
        {
            get
            {
                return _maxTurnRate;
            }
            set
            {
                _maxTurnRate = value;
            }
        }

        public BoxCollider2D _boundingCollider = null;
        public BoxCollider2D BoundingCollider
        {
            get
            {
                if (_boundingCollider != null) return _boundingCollider;
                var b = GetComponent<BoxCollider2D>();
                if (b == null)
                {
                    Debug.LogError("Need BoxCollider!");
                    return null;
                }
            
                return b;
            }
        }

        public Vector2 BoundingSize
        {
            get
            {
                return BoundingCollider.size;
            }
            set
            {
                BoundingCollider.size = value;
            }
        }
        #endregion

        void Awake()
        {
            _headingSmoother = new SmootherVector(SteeringParams.Instance.NumSamplesForSmoothing, new Vector2(0.0f, 0.0f));
            _smoothingOn = true;
        }

        
        protected virtual void Start()
        {
            SetBehaviors(Behaviors);
            // _behaviors.AddBehavior(new Seek(this), )
            Position = transform.position;
            //_scale = new Vector2(1f, 1f);
            Heading = new Vector2(1f, 0f);

            _side = _heading.Perpendicular();
            Mass = 0.2f;
            //MaxSpeed = SteeringParams.Instance.MaxSpeed;
            //MaxTurnRate = SteeringParams.Instance.MaxTurnRatePerSecond;
            //MaxForce = SteeringParams.Instance.MaxSteeringForce;
            //_steering.WallAvoidanceOn();
            //_steering.WanderOn();

            _world = GameManager.Instance.World;
        }

        void SetBehaviors(Dithered d)
        {
            d.AddBehavior(new Seek(this), .8f, 1f);
            d.AddBehavior(new WallAvoidance(this), .5f, 5.29f);
            d.AddBehavior(new Wander(this), .8f, 4.35f);
        }
        public bool RotateHeadingToFacePosition(Vector2 target)
        {
            Vector2 toTarget = (target - Position).normalized;

            //first determine the angle between the heading vector and the target
            float angle = Mathf.Acos(Vector2.Dot(_heading, toTarget));

            //return true if the player is facing the target
            if (angle < float.Epsilon) return true;

            //clamp the amount to turn to the max turn rate
            if (angle > _maxTurnRate) angle = _maxTurnRate;

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            Quaternion rotate = Quaternion.AngleAxis(angle * _heading.Sign(toTarget), Vector3.forward);

            _heading = rotate * _heading;
            _velocity = rotate * _velocity;

            //finally recreate m_vSide
            _side = _heading.Perpendicular();

            return false;
        }

        public bool IsSmoothingOn() { return _smoothingOn; }
        public void SmoothingOn() { _smoothingOn = true; }
        public void SmoothingOff() { _smoothingOn = false; }
        public void ToggleSmoothing() { _smoothingOn = !_smoothingOn; }

        public void BehaviorOn(SteeringBehavior.Type_ type)
        {
            Behaviors.GetBehavior(type).Behavior.ActiveOn();
        }

        public void BehaviorOff(SteeringBehavior.Type_ type)
        {
            Behaviors.GetBehavior(type).Behavior.ActiveOff();
        }

        public SteeringBehavior GetBehavior(SteeringBehavior.Type_ type)
        {
            return Behaviors.GetBehavior(type).Behavior;
        }

        public void ClearBehavior()
        {
            foreach(var b in Behaviors.Behaviors)
            {
                b.Behavior.ActiveOff();
            }
        }
        protected virtual void Update()
        {
            //keep a record of its old position so we can update its cell later
            //in this method
            Vector2 OldPos = Position;


            Vector2 SteeringForce = new Vector2();

            //calculate the combined force from each steering behavior in the 
            //vehicle's list
            SteeringForce = Behaviors.SummingForce();

            //Acceleration = Force/Mass
            Vector2 acceleration = SteeringForce / _mass;

            //update velocity
            _velocity += acceleration * Time.deltaTime;

            //make sure vehicle does not exceed maximum velocity
            _velocity = _velocity.Truncate(_maxSpeed);

            //把本地速度转成世界坐标
            Vector4 worldVec = new Vector4(_velocity.x, _velocity.y, 0, 1);
            //update the position
            Position += worldVec.ToVector2() * Scale * Time.deltaTime;

            //update the heading if the vehicle has a non zero velocity
            if (_velocity.sqrMagnitude > float.Epsilon)
            {
                _heading = _velocity.normalized;

                _side = _heading.Perpendicular();
            }

            //EnforceNonPenetrationConstraint(this, World()->Agents());

            /*
            //treat the screen as a toroid
            WrapAround(m_vPos, m_pWorld->cxClient(), m_pWorld->cyClient());
            */

            //update the vehicle's current cell if space partitioning is turned on
            /*
            if (Steering.IsSpacePartitioningOn())
            {
                World.CellSpace.UpdateEntity(this, OldPos);
            }
            */

            if (IsSmoothingOn())
            {
                //_smoothedHeading = _headingSmoother.Update(Heading);
            }
            transform.rotation = GetRotation();
        }

        Quaternion GetRotation()
        {
            Vector3 to = new Vector3(_heading.x, _heading.y, 0);
            Vector3 from = new Vector3(1, 0, 0);
            return Quaternion.FromToRotation(from, to);
        }

        public float TimeToDestination()
        {
            float vec = _velocity.magnitude;
            var curPos = Position;
            var seek = GetBehavior(SteeringBehavior.Type_.seek) as Seek;
            var toPos = seek.ToPlanet.InCircle.CenterInWorldSpace;
            return (toPos - curPos).magnitude / vec;
        }
    }


}