using UnityEngine;
using System.Collections;
using System;
using Utilities;

namespace Green
{
    public class StateNeutralityToPlayer : FSMState
    {
        //FSM _fsm;
        public StateNeutralityToPlayer(FSM fsm, Star star) : base(star, Star.e_State.NeutralityToPlayer)
        {
            _fsm = fsm;
            _fsm.AddState(this);
            FSMTransition.CheckCondition toPlayer = () =>
            {
                if (star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToPlayer, toPlayer));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
        }

        public override void OnEnter()
        {
            //改变星球动画
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
            _star.Schedule += Formula.CalculateCaptureProgress(_star.PlayerTroops);
        }
    }
}