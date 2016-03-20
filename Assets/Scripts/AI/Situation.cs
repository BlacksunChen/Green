using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITest
{
    class Situation
    {
        private static Situation _instance = null;

        private Situation() {
            this._AIOnTheWay = new List<Tuple<int, int, int, int>>();
            this._distance = new int[20,20];
            this._playerOnTheWay = new List<Tuple<int, int, int, int>>();
            this._stars = new List<Star>();
        }
        public static Situation GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Situation();
            }
            return _instance;
        }
        //加入星球获取实例
        public static void CreateInstance(List<Star> stars)
        {
            if (_instance == null)
            {
                _instance = new Situation();
            }
            _instance._stars = stars;
            for (int i= 0;i<stars.Count;++i)
            {
                for (int j = 0; j < stars.Count; ++j)
                {
                    _instance._distance[i, j] = (int)Math.Sqrt((stars[i].Location.Item1 - stars[j].Location.Item1) * (stars[i].Location.Item1 - stars[j].Location.Item1)
                        + (stars[i].Location.Item2 - stars[j].Location.Item2) * (stars[i].Location.Item2 - stars[j].Location.Item2));
                }
            }
        }

        private List<Star> _stars;//星球们
        private int[,] _distance;//星球的邻接矩阵（各星球间距离）
        private List<Tuple<int, int, int, int>> _playerOnTheWay;//玩家在路上的军队
        private List<Tuple<int, int, int, int>> _AIOnTheWay;//电脑在路上的军队
		private static int _speed = 25;//双方行军速度
		//每个星球对于AI的需求
		private double[,] _needs;
		private double[] _needed;

		//获取总兵力
		public Tuple<double,double> GetTotalTroops ()
		{
			Tuple<double,double> total_troops = new Tuple<double, double> (0,0);//双方总兵力
			foreach (Star star in Situation.GetInstance().Stars) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1 + star.Troops.Item1,
					total_troops.Item2 + star.Troops.Item2);
			
			}
			foreach (Tuple<int,int,int,int> troopOnWay in Situation.GetInstance().PlayerOnTheWay) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1 + troopOnWay.Item3,
					total_troops.Item2);
			}
			foreach (Tuple<int,int,int,int> troopOnWay in Situation.GetInstance().AIOnTheWay) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1,
					total_troops.Item2 + troopOnWay.Item3);
			}
			return total_troops;
		}

		//获取总活力
		public Tuple<int,int> GetTotalVigour ()
		{
			Tuple<int,int> total_vigour = new Tuple<int, int>(0,0);//双方总活力
			foreach (Star star in Situation.GetInstance().Stars) 
			{
				total_vigour = new Tuple<int, int> (total_vigour.Item1 + star.Vigour * (star.State == Star.e_State.Player ? 1 : 0),
					total_vigour.Item2 + star.Vigour * (star.State == Star.e_State.AI ? 1 : 0));
			}
			return total_vigour;
		}

		//获取总战力
		public Tuple<double,double> GetTotalFight ()
		{
			Tuple<double,double> total_troops = new Tuple<double, double> (0,0);//双方总兵力
			foreach (Star star in Situation.GetInstance().Stars) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1 + star.Troops.Item1 / (1.1-(double)star.DEF/50)*(star.State == Star.e_State.Player ? 1 : 0),
					total_troops.Item2 + star.Troops.Item2 / (1.1-(double)star.DEF/50)*(star.State == Star.e_State.AI ? 1 : 0));

			}
			foreach (Tuple<int,int,int,int> troopOnWay in Situation.GetInstance().PlayerOnTheWay) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1 + troopOnWay.Item3,
					total_troops.Item2);
			}
			foreach (Tuple<int,int,int,int> troopOnWay in Situation.GetInstance().AIOnTheWay) 
			{
				total_troops = new Tuple<double, double> (total_troops.Item1,
					total_troops.Item2 + troopOnWay.Item3);
			}
			return total_troops;
		}

		//获取净增长
		public Tuple<double,double> GetRealLose ()
		{
			Tuple<double,double> real_rise = new Tuple<double, double>(0,0);//双方净增长
			foreach (Star star in Situation.GetInstance().Stars) 
			{
				//战斗中的消耗
				double playerLoss = star.Troops.Item1 > 0 ? 4 * star.Troops.Item2 / (star.Troops.Item1 + star.Troops.Item2 ) : 0;
				double AILoss = star.Troops.Item2 > 0 ? 4 * star.Troops.Item1 / (star.Troops.Item1 + star.Troops.Item2 ) : 0;
				if (star.Troops.Item1 < Math.Exp (-2) && star.Troops.Item2 < Math.Exp (-2)) 
				{
					playerLoss = 0;
					AILoss = 0;
				}
				if(star.State ==Star.e_State.Player)
					playerLoss = playerLoss * (1.1 - (double)star.DEF / 50);
				if (star.State == Star.e_State.AI)
					AILoss = AILoss * (1.1 - (double)star.DEF / 50);
				real_rise = new Tuple<double, double> (real_rise.Item1 + star.Vigour * (star.State == Star.e_State.Player ? 1 : 0) - playerLoss,
					real_rise.Item2 + star.Vigour * (star.State == Star.e_State.AI ? 1 : 0) - AILoss);
			}
			return real_rise;
		}

		//若双方不再操作，到最后星球剩下多少玩家军队
		public double[] GetNeeded()
		{
			int num_stars = Situation.GetInstance ().Stars.Count;
			double[] _needed = new double[num_stars];
			for (int i = 0; i < num_stars; ++i) 
			{
				Tuple<double,double> final = AI.GetInstance ().GetResult (i).Troops;
				_needed [i] = final.Item1 / (1.1 - (double)Situation.GetInstance ().Stars [i].DEF / 50) - final.Item2;
			}
			return _needed;

		}

		//若玩家不再操作，此时星球A派往星球B多少时确保胜利
		public double[,] GetNeeds()
		{
			int num_stars = Situation.GetInstance ().Stars.Count;
			double[,] _needs = new double[num_stars,num_stars];//若玩家不再操作，此时星球A派往星球B多少时确保胜利
			for (int i = 0; i < num_stars; ++i) 
			{
				for (int j = 0; j < num_stars; ++j) 
				{
					Tuple<double,double> final = AI.GetInstance ().GetNeeded (i, j);
					_needs [i, j] = final.Item1 / (1.1 - (double)Situation.GetInstance ().Stars [j].DEF / 50) - final.Item2;
				}
			}
			return _needs;

		}

		//获取星球对电脑的重要程度
		public double GetImportance(int starIndex)
		{
			Star star = Situation.GetInstance ().Stars [starIndex];
			Tuple<int,int> total_vigour = Situation.GetInstance ().GetTotalVigour();
			double importance = Situation.GetInstance ().GetTotalTroops ().Item2 / (double)Situation.GetInstance ().GetTotalVigour ().Item2 * (double)star.Capacity / 5
			+ 2 * (double)total_vigour.Item1 / (total_vigour.Item1 + total_vigour.Item2) * star.Vigour
			+ star.DEF;

			return importance;

		}

        //get and set
        public List<Star> Stars
        {
            get
            {
                return _stars;
            }
            set
            {
                _stars = value;
            }
        }
        public int[,] Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                _distance = value;
            }
        }
        public List<Tuple<int, int, int, int>> PlayerOnTheWay
        {
            get
            {
                return _playerOnTheWay;
            }
            set
            {
                _playerOnTheWay = value;
            }
        }
        public List<Tuple<int, int, int, int>> AIOnTheWay
        {
            get
            {
                return _AIOnTheWay;
            }
            set
            {
                _AIOnTheWay = value;
            }
        }
		public static int Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
			}
		}

    }
}
