using UnityEngine;
using System.Collections;
using System;
using Utilities;

namespace Green
{
    public class StateNeutralityToAI : FSMState
    {
        //FSM _fsm;
        public StateNeutralityToAI(FSM fsm, Star star) : base(star, Star.e_State.NeutralityToAI)
        {
            _fsm = fsm;
            _fsm.AddState(this);
            FSMTransition.CheckCondition toAI = () =>
            {
                if (star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.EnemyTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.AI, toAI));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
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