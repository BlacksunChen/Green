using UnityEngine;
using System.Collections;
using System;
using Utilities;

namespace Green
{
    public class StateAI : FSMState
    {
        public StateAI(FSM fsm, Star star) : base(star, Star.e_State.AI)
        {
            _fsm = fsm;
            //_fsm.AddState(this);
            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (star.EnemyTroops <= 1 && star.PlayerTroops > 1)
                    return true;
                else
                    return false;
            };
            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.EnemyTroops <= 1f && star.PlayerTroops <= 1f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToPlayer, neutralToPlayer));
            //_transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
        }

        public override Sprite Image
        {
            get
            {
                if (_image == null)
                {
                    _image = Resources.Load<Sprite>("Planets/坏的星球");                   
                }
                return _image;
            }
        }

        public override void OnEnter()
        {
            //改变星球动画
            //animator.SetTrigger(AnimatorState.敌人.ToString());
            _star.CrossFadeStarImage(Image);
            return;
        }

        public override void OnExit()
        {
            return;
        }

        public override void OnUpdate()
        {
            if (GameWorld.Instance.EnemyPopulation < GameWorld.Instance.EnemyMaxPopulation)
                _star.EnemyTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}
