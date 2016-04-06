using System;
using UnityEngine;
using System.Collections;


namespace Green
{
    public class Soldier : MonoBehaviour
    {
        //在那个星球上
        public Planet InPlanet;

        public SoldierType Bloc = SoldierType.Player;

        public enum StateType
        {
            Patrol, //Wander + 墙
            Move,   //Arrive + Wander
            Attack, //Wander + 墙 + 战斗动画
        }

        public StateType CurrentType = StateType.Patrol;

        SteeringBehaviors _behaviors;

        MovingEntity _movingEntity;
        public void UpdateState(StateType state)
        {
            CurrentType = state;
            _behaviors.ClearFlags();
            switch (state)
            {
                case StateType.Patrol:
                    PatrolOn();
                    break;
                case StateType.Move:
                    MoveOn();
                    break;
                case StateType.Attack:
                    break;
                default:
                    break;
            }
        }
        
        void PatrolOn()
        {
            _behaviors.WanderOn();
            _behaviors.WallAvoidanceOn();
            //set Patrol Param
            _behaviors.WanderScale = 12.34f;
            _behaviors.SeekScale = 1f;
            _movingEntity.MaxForce = 14.97f;    
        }

        void MoveOn()
        {
            _behaviors.SeekOn();
            _behaviors.WanderOn();
            _behaviors.SeekScale = 25f;
            _behaviors.WanderScale = 2f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="onSeekEnded">
        /// 上一层Seek结束后发生的动作
        /// </param>
        public void SetSeekDestination(Planet p, Action onSeekEnded)
        {
            _behaviors.SetDistination(p,
                () =>
                {
                    SetPlanet(p);
                    OnSeekEnded(onSeekEnded);
                });
        }

        public void SetPlanet(Planet p)
        {
            InPlanet = p;
        }

        public void OnSeekEnded(Action onSeekEnded)
        {
            onSeekEnded();
            UpdateState(StateType.Patrol);
        }
        // Use this for initialization
        void Start()
        {
            InPlanet.PlayerSoldiers.Add(this);
            _behaviors = GetComponent<SteeringBehaviors>();
            
            if (_behaviors == null)
            {
                Debug.LogError("Need Script: SteeringBehaviors");
            }

            _movingEntity = GetComponent<MovingEntity>();
            if (_movingEntity == null)
            {
                Debug.LogError("Need Script: MovingEntity");
            }

            UpdateState(StateType.Patrol);

        }
        
        // Update is called once per frame
        void Update()
        {

        }
    }
}
