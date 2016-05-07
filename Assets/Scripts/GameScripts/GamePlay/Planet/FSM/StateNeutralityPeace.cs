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
            //_fsm.AddState(this);
            FSMTransition.CheckCondition neutralToAI = () =>
            {
                if (star.EnemyTroops >= 1f && star.PlayerTroops < 1f)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutralToPlayer = () =>
            {
                if (star.EnemyTroops < 1f && star.PlayerTroops >= 1f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToAI, neutralToAI));
            _transitions.Add(new FSMTransition(Star.e_State.NeutralityToPlayer, neutralToPlayer));
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
            //animator.SetTrigger(AnimatorState.中立.ToString());
            //animator.CrossFade(AnimatorState.中立.ToString(), 2f);
            _star.CrossFadeStarImage(Image);
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