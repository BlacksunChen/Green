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

        public override Sprite Image
        {
            get
            {
                if (_image == null)
                {
                    _image = Resources.Load<Sprite>("Planets/好的星球");
                }
                return _image;
            }
        }

        public override void OnEnter()
        {
            //改变星球动画
            //animator.SetTrigger(AnimatorState.玩家.ToString());
            //animator.CrossFade(AnimatorState.玩家.ToString(), 2f);
            _star.CrossFadeStarImage(Image);
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