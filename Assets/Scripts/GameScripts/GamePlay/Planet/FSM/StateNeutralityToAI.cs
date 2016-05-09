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
            //_fsm.AddState(this);
            FSMTransition.CheckCondition toAI = () =>
            {
                if (star.Schedule >= 1)
                    return true;
                else
                    return false;
            };

            FSMTransition.CheckCondition neutral = () =>
            {
                if (star.EnemyTroops < 1f)
                    return true;
                else
                    return false;
            };
            _transitions.Add(new FSMTransition(Star.e_State.AI, toAI));
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
            //animator.SetTrigger(AnimatorState.中立_敌人.ToString());
            //animator.CrossFade(AnimatorState.中立_敌人.ToString(), 2f);
            _star.CrossFadeStarImage(Image);

            _star.StartCapture();
            return;
        }

        public override void OnExit()
        {
            _star.StopCapture();
            _star.SetProgress(0f);
            return;
        }

        public override void OnUpdate()
        {
            float dtProgress = Formula.CalculateCaptureProgress(_star.EnemyTroops);
            _star.Schedule += dtProgress;
            _star.SetProgress(_star.Schedule);
        }
    }
}