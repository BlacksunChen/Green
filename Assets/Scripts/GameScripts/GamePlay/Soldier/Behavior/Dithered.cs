using System;
using System.Collections.Generic;
using UnityEngine;
using Generic;

namespace Green
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MovingEntity))]
    public class Dithered : SummingMethod
    {
        public Dithered(string name) : base("Dithered")
        {
            
        }

        void Awake()
        {
            _behaviors = new List<BehaviorWrapper>();
        }
        void Start()
        {
            _movingEntity = GetComponent<MovingEntity>();
        }
        MovingEntity _movingEntity;

        public MovingEntity Target
        {
            get
            {
                if(_movingEntity == null)
                {
                    _movingEntity = GetComponent<MovingEntity>();
                }
                return _movingEntity;
            }
        }
        Vector2 _steeringForce = new Vector2();

        List<BehaviorWrapper> _behaviors;
        
        public List<BehaviorWrapper> Behaviors
        {
            get
            {
                if(_behaviors == null)
                {
                    _behaviors = new List<BehaviorWrapper>();
                }
                return _behaviors;
            }
        }

        public SteeringBehavior GetBehavior(SteeringBehavior.Type_ type)
        {
            foreach(var b in Behaviors)
            {
                if(b.Type == type)
                {
                    return b.Behavior;
                }
            }
            Debug.LogErrorFormat("Get Behavior Error:{0}", type.ToString());
            return null;
        }
        /// <summary>
        /// random float between 0 and 1
        /// </summary>
        /// <returns></returns>
        float RandFloat()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

        public void AddBehavior(SteeringBehavior behavior, float priority, float weight)
        {
            if(behavior == null)
            {
                Debug.LogError("Behavior = null");
            }
            bool isContain = false;
            foreach (var b in Behaviors)
            {
                if (b.Type == behavior.Type)
                {
                    Debug.LogError("Has Behavior: " + behavior.Type.ToString());
                    return;
                }
            }
            Behaviors.Add(new BehaviorWrapper(behavior, priority, weight));
        }

        public void AddEmptyBehavior()
        {
            Behaviors.Add(new BehaviorWrapper());
        }
        public override Vector2 SummingForce()
        {
            _steeringForce.Zero();
            foreach (var behavior in Behaviors)
            {
                var b = behavior;
                if (b.Behavior.Active && RandFloat() < b.Priority)
                {
                    _steeringForce += b.Behavior.CalculateForce() * b.Weight / b.Priority;
                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce = _steeringForce.Truncate(Target.MaxForce);

                        return _steeringForce;
                    }
                }                
            }
            return _steeringForce;
        }

       public  void RemoveLastBehavior()
        {
            Behaviors.RemoveAt(Behaviors.Count - 1);
        }
        void OnGUI()
        {
            foreach(var behavior in Behaviors)
            {
                behavior.Behavior.OnGUI();
            }
        }

        void OnDrawGizmos()
        {
            foreach (var behavior in Behaviors)
            {
                behavior.Behavior.OnDrawGizmos();
            }
        }

        public class BehaviorWrapper
        {
            public SteeringBehavior Behavior;
            public float Priority;
            public float Weight;

            public SteeringBehavior.Type_ Type
            {
                get
                {
                    if (Behavior == null)
                        return SteeringBehavior.Type_.none;
                    else
                        return Behavior.Type;
                }
            }

            /// <summary>
            /// 检查
            /// </summary>
            public void UpdateBehaviorType(MovingEntity entity, SteeringBehavior.Type_ type)
            {
                if (type == Type) return;
                Behavior = SteeringBehavior.Create(entity, type);                
            }

            public BehaviorWrapper(SteeringBehavior behavior, float pri, float weight)
            {
                Behavior = behavior;
                Priority = pri;
                Weight = weight;
            }
            public BehaviorWrapper(): this(null, 0f, 0f)
            {
            }
        }
    }
}
