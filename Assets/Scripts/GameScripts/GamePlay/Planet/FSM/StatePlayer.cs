using UnityEngine;
using System.Collections;
using System;

namespace Green
{
    public class StatePlayer : FSMState
    {
        public Star.e_State State;

        private Star _star;
        public StatePlayer(Star _star): base(Star.e_State.Player)
        {
            FSMTransition.CheckCondition neutralToAI = () =>
            {
                if (_star.EnemyTroops > 0 && _star.PlayerTroops <= 0)
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
            _transitions.Add(new FSMTransition(new StateNeutralityToAI(_star), neutralToAI));
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
            _star.PlayerTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}