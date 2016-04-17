using System;
using UnityEngine;
using System.Collections;
using Green;

namespace Utilities
{
    [Serializable]
    public class FSMTransition
    {
        string Name;

        //[SerializeField]
        //TransitionCondition condition;
        public delegate bool CheckCondition();

        public CheckCondition Check;

        public Star.e_State NextState
        {
            get
            {
                return _toState;
            }
        }

        Star.e_State _toState;
        public FSMTransition(Star.e_State nextState, CheckCondition callback)
        {
            _toState = nextState;
            Check = callback;
        }
    }
}