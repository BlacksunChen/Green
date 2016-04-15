using UnityEngine;
using System.Collections;

namespace Green
{
    public class StateNeutralityToPlayer : FSMState
    {

        public Star.e_State State;

        private Star _star;
        public StateNeutralityToPlayer(Star _star): base(Star.e_State.NeutralityToPlayer)
        {
            FSMTransition.CheckCondition toPlayer = () =>
            {
                if (_star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (_star.PlayerTroops <= 0f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(new StateNeutralityToPlayer(_star), toPlayer));
            _transitions.Add(new FSMTransition(new StateNeutralityPeace(_star), neutral));
        }

        public override void OnEnter()
        {
            //改变星球动画
            return;
        }

        protected override void OnExit()
        {
            return;
        }
    }
}