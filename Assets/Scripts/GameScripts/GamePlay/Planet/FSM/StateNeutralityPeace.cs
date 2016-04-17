using UnityEngine;
using System.Collections;
using System;
using Utilities;

namespace Green
{
    public class StateNeutralityPeace : FSMState
    {
        public StateNeutralityPeace(FSM fsm, Star star) : base(star, Star.e_State.NeutralityPeace)
        {
            _fsm = fsm;
            _fsm.AddState(this);
            FSMTransition.CheckCondition neutralToAI = () =>
            {
                if (star.EnemyTroops > 0f && star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (star.EnemyTroops <= 0f && star.PlayerTroops > 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToAI, neutralToAI));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToPlayer, neutralToPlayer));
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