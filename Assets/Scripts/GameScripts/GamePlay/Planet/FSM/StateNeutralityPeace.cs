using UnityEngine;
using System.Collections;
using System;

namespace Green
{
    public class StateNeutralityPeace : FSMState
    {
        public Star.e_State State;

        private Star _star;
        public StateNeutralityPeace(Star _star) : base(Star.e_State.NeutralityPeace)
        {
            FSMTransition.CheckCondition neutralToAI = () =>
            {
                if (_star.EnemyTroops > 0f && _star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (_star.EnemyTroops <= 0f && _star.PlayerTroops > 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(new StateNeutralityToAI(_star), neutralToAI));
            _transitions.Add(new FSMTransition(new StateNeutralityToPlayer(_star), neutralToPlayer));
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
            return;
        }
    }
}