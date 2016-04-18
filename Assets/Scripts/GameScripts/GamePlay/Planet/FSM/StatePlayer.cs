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
                if (star.EnemyTroops > 0 && star.PlayerTroops <= 0)
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
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToAI, neutralToAI));
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
            if (GameWorld.Instance.PlayerPopulation < GameWorld.Instance.PlayerMaxPopulation)
                _star.PlayerTroops += Formula.CalculateSoldierIncreasePerTime(_star.Vigour);
        }
    }
}