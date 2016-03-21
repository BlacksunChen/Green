using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AITest
{
	class Star
	{
		//Star的cor
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
		public Star(e_State state,int def,int vigour,int capacity,
			Tuple<int, int> location, Tuple<double, double> troops,double schedule)
		{
			_state = state;
			_DEF = def;
			_vigour = vigour;
			_capacity = capacity;
			_location = location;
			_troops = troops;
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
		private e_State _state;//存储状态

		private int _DEF;//防御力
		private int _vigour;//活力，增长量
		private int _capacity;//容量
		private Tuple<int, int> _location;//坐标
		private Tuple<double, double> _troops;//双方兵力，前者是玩家，后者是AI
		private double _schedule;//中立星球占领进度（中立星球的那四个状态有效）

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
		public Tuple<int, int> Location
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
		public Tuple<double, double> Troops
		{
			get
			{
				return _troops;
			}
			set
			{
				_troops = value;
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
	}
}
