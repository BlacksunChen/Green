using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Tuples;
using Utilities;

namespace Green
{
    public class Star : MonoBehaviour
    {
        //Star的cor
        /*
		public Star(Star star)
		{
			_state = star.State;
			_schedule = star.Schedule;
			_DEF = star.DEF;
			_vigour = star.Vigour;
			_capacity = star.Capacity;
			_location = star.Location;
			_troops = star.Troops;
		}
        */

        void SetProperty(
            e_State state,
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

        //星球所处状态
        public enum e_State
        {
            Player = 0,             //属于玩家的和平星球
            AI = 1,                 //属于电脑的和平星球
            NeutralityPeace = 2,    //中立
            NeutralityToPlayer = 3, //玩家正在占领的中立星球
            NeutralityToAI = 4,     //电脑正在占领的中立星球
        }

        private FSMState _fsmState;

        private e_State _state
        {
            get
            {
                return _fsmState.EnumState;
            }
        }

        [SerializeField, SetProperty("DEF")]
        private int _DEF;//防御力

        [SerializeField, SetProperty("Vigour")]
        private int _vigour;//活力，增长量

        [SerializeField, SetProperty("Capacity")]
        private int _capacity;//容量

        [SerializeField, SetProperty("Location")]
        private Vector2 _location;
        //private Tuple<int, int> _location;//坐标


        //private Tuple<double, double> _troops;//双方兵力，前者是玩家，后者是AI

        [SerializeField, SetProperty("Troops")]
        private float _playerTroops;

        [SerializeField, SetProperty("Troops")]
        private float _enemyTroops;

        [SerializeField, SetProperty("Schedule")]
        private float _schedule;//中立星球占领进度（中立星球的那四个状态有效）

        public float PlayerTroops
        {
            get
            {
                return _playerTroops;
            }
            set
            {
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

        void Start()
        {
            /*
            _state = State;
            _schedule = Schedule;
            _DEF = DEF;
            _vigour = Vigour;
            _capacity = Capacity;
            _location = Location;
            */
        }

        void SetInitState(e_State state)
        {
            switch (state)
            {
                case e_State.Player:
                    _fsmState = new StatePlayer(this);
                    break;
                case e_State.AI:
                    _fsmState = new StateAI(this);
                    break;
                case e_State.NeutralityPeace:
                    _fsmState = new StateNeutralityPeace(this);
                    break;
                case e_State.NeutralityToPlayer:
                    _fsmState = new StateNeutralityToPlayer(this);
                    break;
                case e_State.NeutralityToAI:
                    _fsmState = new StateNeutralityToAI(this);
                    break;
                default:
                    break;
            }
        }
        public bool IsBattleInPlanet()
        {
            //被占领 和平
            if (_state == e_State.AI || _state == e_State.Player || _state == e_State.NeutralityPeace)
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

            if (_state == e_State.AI)
            {
                _enemyTroops  += BattleManager.CalculateDamageForDefOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
                _playerTroops += BattleManager.CalculateDamageForAttackOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
            }
            else if (_state == e_State.Player)
            {
                _enemyTroops  += BattleManager.CalculateDamageForAttackOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
                _playerTroops += BattleManager.CalculateDamageForDefOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
            }
            else
            {
                _enemyTroops  += BattleManager.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
                _playerTroops += BattleManager.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
            }
        }

        public void UpdateStateAfterBattle()
        {
            _state = _fsmState.NextState();
    
        }

        public void StartCapture()
        {
            _schedule = 0;
        }

        public void UpdateCaptureProgress()
        {
            if (_state == e_State.NeutralityToAI)
                _schedule += BattleManager.CalculateCaptureProgress(_enemyTroops);
            else if (_state == e_State.NeutralityToPlayer)
                _schedule += BattleManager.CalculateCaptureProgress(_playerTroops);
            _schedule = Truncate(_schedule, 0f, 1f);
        }

        float Truncate(float num, float min, float max)
        {
            if (num >= max) return max;
            if (num <= min) return min;
            return num;
        }

        float Truncate(float num, float min)
        {
            if (num <= min) return min;
            return num;
        }
    }
}
