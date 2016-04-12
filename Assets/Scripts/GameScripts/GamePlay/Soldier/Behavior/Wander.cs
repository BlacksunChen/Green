using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Green;
using UnityEditor;

namespace Green
{
    public class Wander : SteeringBehavior
    {
        //explained above
        [SerializeField, SetProperty("WanderJitterPerSec")]
        float _wanderJitter = 25.7f;
        [SerializeField, SetProperty("WanderRad")]
        float _wanderRadius = 0.93f;
        [SerializeField, SetProperty("WanderDist")]
        float _wanderDistance = 0.88f;

        //the current position on the wander circle the agent is
        //attempting to steer towards
        Vector2 _wanderTarget = new Vector2();

        public float WanderRad// = 1.2f;
        {
            get
            {
                return _wanderRadius;
            }
            set
            {
                _wanderRadius = value;
            }
        }

        //distance the wander circle is projected in front of the agent
        public float WanderDist// = 2.0f;
        {
            get
            {
                return _wanderDistance;
            }
            set
            {
                _wanderDistance = value;
            }
        }

        //the maximum amount of displacement along the circle each frame
        public float WanderJitterPerSec //= 80.0f;
        {
            get
            {
                return _wanderJitter;
            }
            set
            {
                _wanderJitter = value;
            }
        }

        public Wander(MovingEntity entity)
            : base(entity, "Wander", Type_.wander)
        {

        }

        public override Vector2 CalculateForce()
        {
            if (!Active) return new Vector2();
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            float JitterThisTimeSlice = WanderJitterPerSec * Time.deltaTime;

            //first, add a small random vector to the target's position
            _wanderTarget += new Vector2(UnityEngine.Random.Range(-1f, 1f) * JitterThisTimeSlice,
                                         UnityEngine.Random.Range(-1f, 1f) * JitterThisTimeSlice);

            //reproject this new vector back on to a unit circle
            _wanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            _wanderTarget *= WanderRad;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = _wanderTarget + new Vector2(WanderDist, 0);

            //project the target into world space
            Vector2 Target = target.ToWorldSpace(_movingEntity.Side,
                                                 _movingEntity.Position);
            WanderTargetToDraw = Target - _movingEntity.Position;
            //and steer towards it
            return WanderTargetToDraw;
        }

        Vector2 WanderTargetToDraw = new Vector2();

        public override void OnDrawGizmos()
        {
            base.OnGUI();

            var xy = _wanderDistance * _movingEntity.Heading.normalized;
            Vector3 length = new Vector3(xy.x, xy.y, 0);

            Vector3 center = _movingEntity.transform.position + length;

            OnGizmosDrawCircleInWorldSpace(center, _wanderRadius);
            OnGizmosDrawCircleInLocalSpace(WanderTargetToDraw, 0.025f);
        }

        public override void OnDrawInspector()
        {
            base.OnDrawInspector();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                WanderRad = EditorGUILayout.FloatField("Wader Radius: ", WanderRad);
                WanderDist = EditorGUILayout.FloatField("Wader Distance: ", WanderDist);
                WanderJitterPerSec = EditorGUILayout.FloatField("Wader Jitter: ", WanderJitterPerSec);
            }
            GUILayout.EndVertical();
        }
    }
}
