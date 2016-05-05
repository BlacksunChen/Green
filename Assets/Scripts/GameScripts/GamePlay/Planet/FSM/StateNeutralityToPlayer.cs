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
            //_fsm.AddState(this);
            FSMTransition.CheckCondition toPlayer = () =>
            {
                if (star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.PlayerTroops < 1f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.Player, toPlayer));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityPeace, neutral));
        }

        public override Sprite Image
        {
            get
            {
                if (_image == null)
                {
                    _image = Resources.Load<Sprite>("Planets/中立星球");
                }
                return _image;
            }
        }

        public override void OnEnter()
        {
            //改变星球动画
            //animator.SetTrigger(AnimatorState.中立_玩家.ToString());
            //animator.CrossFade(AnimatorState.中立_玩家.ToString(), 2f);
            _star.CrossFadeStarImage(Image);
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