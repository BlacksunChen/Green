using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Green
{
    public abstract class FSMState
    {
        #region Variables

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

        public FSMState(Star.e_State state)
        {
            EnumState = state;
        }

        public FSMState NextState()
        {
            foreach(var condition in _transitions)
            {
                if(condition.Check())
                {
                    OnExit();
                    condition.NextState.OnEnter();
                    return condition.NextState;
                }
            }
            return this;
        }

        #endregion
    }
}