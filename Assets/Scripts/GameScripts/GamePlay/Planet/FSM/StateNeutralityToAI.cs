using UnityEngine;
using System.Collections;
using System;

namespace Green
{
    public class StateNeutralityToAI : FSMState
    {
        public Star.e_State State;

        private Star _star;
        public StateNeutralityToAI(Star _star) : base(Star.e_State.NeutralityToAI)
        {
            FSMTransition.CheckCondition toAI = () =>
            {
                if (_star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (_star.EnemyTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(new StateAI(_star), toAI));
            _transitions.Add(new FSMTransition(new StateNeutralityPeace(_star), neutral));
        }

        public override void OnEnter()
        {
            _star.StartCapture();
            return;
        }

        public override void OnExit()
        {
            _star.StopCapture();
            return;
        }

        public override void OnUpdate()
        {
            _star.Schedule += Formula.CalculateCaptureProgress(_star.EnemyTroops);
        }
    }
}