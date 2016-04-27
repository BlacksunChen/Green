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

        public override void OnEnter()
        {
            //改变星球动画
            animator.SetTrigger(AnimatorState.敌人.ToString());
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
