using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Green;

namespace Utilities
{
    public abstract class FSMState
    {
        Animator _animator;

        protected enum AnimatorState
        {
            中立,
            中立_玩家,
            中立_敌人,
            玩家,
            敌人
        }
        protected Animator animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = _star.GetComponent<Animator>();
                }
                return _animator;
            }
        }
        #region Variables

        protected FSM _fsm;
        [SerializeField]
        protected List<FSMTransition> _transitions = new List<FSMTransition>();
        #endregion

        #region AbstractFunctions

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnUpdate();
        #endregion


        #region Functions

        public readonly Star.e_State EnumState;

        protected Star _star;

        public FSMState(Star star, Star.e_State state)
        {
            _fsm = star.FiniteStateMachine;
            _star = star;
            EnumState = state;
        }

        public FSMState NextState()
        {
            foreach(var condition in _transitions)
            {
                if(condition.Check())
                {
                    OnExit();
                    var nextState = _fsm.GetState(condition.NextState);
                    nextState.OnEnter();
                    return nextState;
                }
            }
            return this;
        }

        #endregion
    }
}