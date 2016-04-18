using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Green
{
    //TODO 重构进Utilities里面 作为基类
    public class FSM
    {
        private Dictionary<Star.e_State, FSMState> _stateDic = new Dictionary<Star.e_State, FSMState>();

        public FSM(Star star)
        {
            _stateDic.Add(Star.e_State.AI, new StateAI(this, star));
            _stateDic.Add(Star.e_State.NeutralityPeace, new StateNeutralityPeace(this, star));
            _stateDic.Add(Star.e_State.NeutralityToAI, new StateNeutralityToAI(this, star));
            _stateDic.Add(Star.e_State.NeutralityToPlayer, new StateNeutralityToPlayer(this, star));
            _stateDic.Add(Star.e_State.Player, new StatePlayer(this, star));
        }

        public void AddState<T>(T state)
            where T : FSMState
        {
            if(!_stateDic.ContainsKey(state.EnumState))
                _stateDic.Add(state.EnumState, state);
        }

        public FSMState GetState(Star.e_State state)
        {
            if(!_stateDic.ContainsKey(state))
            {
                Debug.LogErrorFormat("FSM GetState Error!");
            }
            return _stateDic[state];
        }
    }
}
