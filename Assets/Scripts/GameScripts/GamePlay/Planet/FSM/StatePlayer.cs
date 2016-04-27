using UnityEngine;
using System.Collections;
using System;
using Utilities;

namespace Green
{
    public class StatePlayer : FSMState
    {
        //FSM _fsm;
        public StatePlayer(FSM fsm, Star star) : base(star, Star.e_State.Player)
        {
            _fsm = fsm;
            //_fsm.AddState(this);
            FSMTransition.CheckCondition neutralToAI = () =>
            {
                if (star.EnemyTroops >= 1f && star.PlayerTroops < 1f)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.EnemyTroops < 1f && star.PlayerTroops < 1f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToAI, neutralToAI));
            //_transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
        }

        public override void OnEnter()
        {
            //改变星球动画
            animator.SetTrigger(AnimatorState.玩家.ToString());
            return;
        }

        public override void OnExit()
        {
            return;
        }

        public override void OnUpdate()
        {
            if (GameWorld.Instance.PlayerPopulation < GameWorld.Instance.PlayerMaxPopulation)
                _star.PlayerTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}