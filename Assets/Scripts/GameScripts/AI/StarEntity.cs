using System;
using UnityEngine;

namespace Green
{
	public class StarEntity
	{
		public StarEntity(Star star)
		{
			SetProperty (star.State, star.DEF, star.Vigour, star.Capacity, star.Location,
				star.EnemyTroops, star.PlayerTroops, star.Schedule);
		}

		public StarEntity(StarEntity star)
		{
			SetProperty (star.SelectedState, star.DEF, star.Vigour, star.Capacity, star.Location,
				star.EnemyTroops, star.PlayerTroops, star.Schedule);
		}

		void SetProperty(
			Star.e_State state,
			int def,
			int vigour,
			int capacity,
			Vector2 location,
			float enemyTroops,
			float playerTroops,
			float schedule)
		{
			_selectedState = state;
			_DEF = def;
			_vigour = vigour;
			_capacity = capacity;
			_location = location;
			_enemyTroops = enemyTroops;
			_playerTroops = playerTroops;
			_schedule = schedule;
		}




		private Star.e_State _selectedState = Star.e_State.Player;


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



		[Range(0f, 10f)]
		private int _DEF;//防御力

		[Range(0f, 10f)]
		private int _vigour;//活力，增长量

		[Range(0f, 20f)]
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

		public StarEntity()
		{
		}


	}
}

