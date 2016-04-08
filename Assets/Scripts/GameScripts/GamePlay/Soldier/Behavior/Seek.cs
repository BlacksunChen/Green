using System;
using UnityEngine;
using Generic;

namespace Green
{
    public class Seek : SteeringBehavior
    {
        Planet _seekToPlanet = null;

        Vector2 _destination;

        Vector2 _seekTarget;

        MovingEntity _movingEntity;

        Action _onSeekEnded;
  
        public Seek(MovingEntity movingEntity, Vector2 targetPos)
        {
            _destination = targetPos;
            _movingEntity = movingEntity;
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
            if (!IsActive()) return new Vector2(); 
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
            OnGizmosDrawCircle(new Vector3(_destination.x, _destination.y, _movingEntity.transform.position.z), 0.25f);
        }
    }
}
