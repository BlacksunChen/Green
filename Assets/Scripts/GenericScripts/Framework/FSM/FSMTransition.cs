using System;
using UnityEngine;
using System.Collections;

namespace Green
{
    [Serializable]
    public class FSMTransition
    {
        [SerializeField]
        string Name;

        [SerializeField]
        FSMState _toState;

        //[SerializeField]
        //TransitionCondition condition;
        public delegate bool CheckCondition();

        public CheckCondition Check;

        public FSMState NextState
        {
            get
            {
                return _toState;
            }
        }

        public FSMTransition(FSMState nextState, CheckCondition callback)
        {
            _toState = nextState;
            Check = callback;
        }
    }
}