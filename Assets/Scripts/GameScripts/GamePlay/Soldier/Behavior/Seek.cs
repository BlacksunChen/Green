using System;
using UnityEngine;
using Utilities;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Green
{
    public class Seek : SteeringBehavior
    {
        Planet _seekToPlanet = null;

        Vector2 _destination;

        Vector2 _seekTarget;

        Action _onSeekEnded;

        public Planet ToPlanet
        {
            get { return _seekToPlanet; }
        }

        public Seek(MovingEntity movingEntity): base(movingEntity, "Seek", Type_.seek)
        {
        }

        public Vector2 SetDistination(Planet p, Action onSeekEnded)
        {
            Reset();
            _onSeekEnded = onSeekEnded;
            _seekToPlanet = p;
            Vector2 pointToSeek = Geometry.GetRamdomPointOnRing(p.OutCircleRad, p.InCircleRad, p.transform.position);
            _destination = pointToSeek;
            return _destination;
        }

        void Reset()
        {
            _seekToPlanet = null;
            _destination = _movingEntity.Position;
            _onSeekEnded = () => { };
            _seekTarget = _movingEntity.Position;
        }

        public override Vector2 CalculateForce()
        {
            if (!Active) return new Vector2(); 
            if (Geometry.PointInCircle(_movingEntity.Position, _seekToPlanet.OutCircle.CenterInWorldSpace, _seekToPlanet.OutCircle.Radius))
            {
                _onSeekEnded();
                return new Vector2();
            }
            Vector2 DesiredVelocity = (_destination - _movingEntity.Position).normalized
                            * _movingEntity.MaxSpeed;

            return (DesiredVelocity - _movingEntity.Velocity);
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            OnGizmosDrawCircleInWorldSpace(new Vector3(_destination.x, _destination.y, _movingEntity.transform.position.z), 0.25f);
        }

        public override void OnGUI()
        {
            base.OnGUI();
            
        }

#if UNITY_EDITOR
        
        public override void OnDrawInspector()
        {
            base.OnDrawInspector();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.Vector2Field("Destination: ", _destination);
                EditorGUILayout.ObjectField("To Planet: ", _seekToPlanet, typeof(Planet), true);
            }
            GUILayout.EndVertical();
        }
#endif
    }

}
