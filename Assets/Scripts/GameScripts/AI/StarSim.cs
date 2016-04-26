using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utilities;
using Green;
namespace Green.Ai
{
    public class StarSim
    {
        public StarSim(
            Star.e_State state,
            int def,
            int vigour,
            int capacity,
            Vector2 location,
            float enemyTroops,
            float playerTroops,
            float schedule)
        {
            SetInitState(state);
            _DEF = def;
            _vigour = vigour;
            _capacity = capacity;
            _location = location;
            _enemyTroops = enemyTroops;
            _playerTroops = playerTroops;
            _schedule = schedule;
        }


        private FSM _fsm;

        private FSMState _fsmState;

        private Star.e_State _selectedState = Star.e_State.Player;

        public FSM FiniteStateMachine
        {
            get
            {
                return _fsm;
            }
        }


        public Star.e_State SelectedState
        {
            get
            {
                return _selectedState;
            }
            set
            {
                _selectedState = value;
            }
        }

        public Star.e_State State
        {
            get
            {
                return _fsmState.EnumState;
            }
        }

        public FSMState FsmState
        {
            get
            {
                return _fsmState;
            }
        }


        private int _DEF;//防御力


        private int _vigour;//活力，增长量


        private int _capacity;//容量

        private Vector2 _location;
        //private Tuple<int, int> _location;//坐标


        //private Tuple<double, double> _troops;//双方兵力，前者是玩家，后者是AI

        private float _playerTroops;

        private float _enemyTroops;

        private float _schedule;//中立星球占领进度（中立星球的那四个状态有效）

        public float PlayerTroops
        {
            get
            {
                return _playerTroops;
            }
            set
            {
                if (value < 0f)
                    _playerTroops = 0f;
                else
                    _playerTroops = value;
            }
        }

        public float EnemyTroops
        {
            get
            {
                return _enemyTroops;
            }
            set
            {
                if (value < 0f)
                    _enemyTroops = 0f;
                else
                    _enemyTroops = value;
            }
        }
        public int DEF
        {
            get
            {
                return _DEF;
            }
            set
            {
                _DEF = value;
            }
        }
        public int Vigour
        {
            get
            {
                return _vigour;
            }
            set
            {
                _vigour = value;
            }
        }
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                _capacity = value;
            }
        }
        public Vector2 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public float Schedule
        {
            get
            {
                return _schedule;
            }
            set
            {
                _schedule = value;
            }
        }


        void SetInitState(Star.e_State state)
        {
            _fsmState = _fsm.GetState(state);
        }

        public bool IsBattleInPlanet()
        {
            //被占领 和平
            if (State == Star.e_State.AI || State == Star.e_State.Player || State == Star.e_State.NeutralityPeace)
            {
                if (_enemyTroops > 0 && _playerTroops > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void BattlePerTime(float perTime)
        {
            if (!IsBattleInPlanet()) return;

            float enemy = 0f;
            float player = 0f;

            if (State == Star.e_State.AI)
            {
                enemy += Formula.CalculateDamageForDefOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
                player += Formula.CalculateDamageForAttackOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
            }
            else if (State == Star.e_State.Player)
            {
                enemy += Formula.CalculateDamageForAttackOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
                player += Formula.CalculateDamageForDefOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
            }
            else
            {
                enemy += Formula.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
                player += Formula.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
            }
            EnemyTroops += enemy;
            PlayerTroops += player;
            DebugInConsole.LogFormat("我方损失兵力:{0} 总兵力:{1}", player, PlayerTroops);
            DebugInConsole.LogFormat("敌方损失兵力:{0} 总兵力:{1}", enemy, EnemyTroops);
            //Truncate(ref _enemyTroops, 0f);
            //Truncate(ref _playerTroops, 0f);

        }

        bool _isDuringCapture = false;

        public void StartCapture()
        {
            _schedule = 0;
            _isDuringCapture = true;
        }

        public void StopCapture()
        {
            _isDuringCapture = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateSituation()
        {
            //本状态执行更新
            _fsmState.OnUpdate();

            //战斗清算
            BattlePerTime(Formula.CalculatePerTime);

            //判断是否有到条件进入下一个状态
            _fsmState = _fsmState.NextState();
        }

        void Truncate(ref float num, float min, float max)
        {
            if (num >= max) num = max;
            if (num <= min) num = min;
        }

        void Truncate(ref float num, float min)
        {
            if (num <= min) num = min;
        }
    }
}
