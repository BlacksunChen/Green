using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Generic;

namespace Green
{
    public class Dithered : SummingMethod
    {
        public Dithered(MovingEntity entity, string name) : base(name)
        {
            _movingEntity = entity;
            _behaviors = new List<BehaviorWrapperDithered>();
        }

        MovingEntity _movingEntity;

        Vector2 _steeringForce = new Vector2();

        List<BehaviorWrapperDithered> _behaviors;

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
            _behaviors.Add(new BehaviorWrapperDithered(behavior, priority, weight));
        }

        public override Vector2 SummingForce()
        {
            _steeringForce.Zero();
            foreach (var behavior in _behaviors)
            {
                if (behavior.Behavior.Active && RandFloat() < behavior.Priority)
                {
                    _steeringForce += behavior.Behavior.CalculateForce() * behavior.Weight / behavior.Priority;
                    if (!_steeringForce.IsZero())
                    {
                        _steeringForce = _steeringForce.Truncate(_movingEntity.MaxForce);

                        return _steeringForce;
                    }
                }                
            }
            return _steeringForce;
        }

        void OnGUI()
        {
            foreach(var behavior in _behaviors)
            {
                behavior.Behavior.OnGUI();
            }
        }

        void OnDrawGizmos()
        {
            foreach (var behavior in _behaviors)
            {
                behavior.Behavior.OnDrawGizmos();
            }
        }

        public class BehaviorWrapperDithered
        {
            public SteeringBehavior Behavior;
            public float Priority;
            public float Weight;

            public BehaviorWrapperDithered(SteeringBehavior behavior, float pri, float weight)
            {
                Behavior = behavior;
                Priority = pri;
                Weight = weight;
            }
        }
    }
}
