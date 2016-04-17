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
            _fsm.AddState(this);
            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (star.EnemyTroops <= 0 && star.PlayerTroops > 0)
                    return true;
                else
                    return false;
            };
            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.EnemyTroops <= 0f && star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToPlayer, neutralToPlayer));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
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
            if (_star.Capacity > _star.EnemyTroops)
                _star.EnemyTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}
