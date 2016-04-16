using UnityEngine;
using System.Collections;
using System;

namespace Green
{
    public class StateAI : FSMState
    {
        private Star _star;
        public StateAI(Star _star): base(Star.e_State.AI)
        {
            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (_star.EnemyTroops <= 0 && _star.PlayerTroops > 0)
                    return true;
                else
                    return false;
            };
            FSMTransition.CheckCondition neutral = () =>
            {
                if (_star.EnemyTroops <= 0f && _star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(new StateNeutralityToPlayer(_star), neutralToPlayer));
            _transitions.Add(new FSMTransition(new StateNeutralityPeace(_star), neutral));
        }

        public override void OnEnter()
        {
            //改变星球动画
            return;
        }

        public override void OnExit()
        {
            return;
        }

        public override void OnUpdate()
        {
            _star.EnemyTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}
