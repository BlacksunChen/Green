using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Tuples;
using Utilities;

namespace Green
{
	public class Star:MonoBehaviour
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
            double enemyTroops,
            double playerTroops,
            double schedule)
		{
			_state = state;
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
			Player = 0,//属于玩家的和平星球
			AI = 1,//属于电脑的和平星球
			NeutralityPeace = 2,//中立
			NeutralityToPlayer = 3,//玩家正在占领的中立星球
			NeutralityToAI = 4//电脑正在占领的中立星球
		}
        [SerializeField, SetProperty("State")]
		private e_State _state;//存储状态

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
        private double _playerTroops;

        [SerializeField, SetProperty("Troops")]
        private double _enemyTroops;

        [SerializeField, SetProperty("Schedule")]
        private double _schedule;//中立星球占领进度（中立星球的那四个状态有效）

        public double PlayerTroops
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

        public double EnemyTroops
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

		public double Schedule
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
		public Star.e_State State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
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
	}
}
