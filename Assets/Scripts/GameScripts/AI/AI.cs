using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generic.Tuples;


namespace Green
{
    public class AI
    {
		private double[,] _needs;//若玩家不再操作，此时星球A派往星球B多少时确保胜利
		private double[] _needed;//若双方不再操作，到最后星球剩下多少玩家军队
		private double[] _fimportance;//星球重要程度的f函数

        private static AI _instance = null;
        private AI() { }
        public static AI GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AI();
            }
            return _instance;
        }

        public void Run()
        {
            //执行策略方法返回的策略
            List<Tuple3<int,int,int>> strategyList = Strategy();

            //获取星球信息
            List<Star> starList = Situation.GetInstance().Stars;

            foreach (var strategy in strategyList)
            {
                //打印策略信息
                Console.WriteLine("星球{0}派{1}个人到星球{2}", strategy.Item1, strategy.Item3, strategy.Item2);
                
                //source star
                Star ss = starList[strategy.Item1];
                //terminal star
                Star ts = starList[strategy.Item2];
                //派遣人数
                int num = strategy.Item3;
                
                num = num > ss.Troops.Item2 ? (int)ss.Troops.Item2 : num;

                //从起点减去对应的人数
                ss.Troops = new Tuple<double,double>(ss.Troops.Item1, ss.Troops.Item2 - num);
               
                //将人数加到路上
                Situation.GetInstance().AIOnTheWay
                    .Add(new ArmySituation(strategy.Item1,strategy.Item2,num,0));
            }
            
        }

        

		//模拟某一星球一个周期后的状态，第一个参数是星球，第二个是AI开往这个星球的军队，第三个是玩家开往这个星球的军队
		public static void ChangeOnStar(ref Star star,ref List<ArmySituation> AIToHere,
			ref List<ArmySituation> playerToHere)
		{
			//占领中立星球进度
			if (star.State == Star.e_State.NeutralityToPlayer && star.Troops.Item1>0&&star.Troops.Item2==0) 
			{
				star.Schedule += (double)(50 * star.Troops.Item1 + 13) / (150 * star.Troops.Item1 + 3630);
			}
			if (star.State == Star.e_State.NeutralityToAI && star.Troops.Item1==0&&star.Troops.Item2>0) 
			{
				star.Schedule += (double)(50 * star.Troops.Item2 + 13) / (150 * star.Troops.Item2 + 3630);
			}
			if (star.Schedule >= 1) 
			{
				if (star.State == Star.e_State.NeutralityToPlayer)
					star.State = Star.e_State.Player;
				if (star.State == Star.e_State.NeutralityToAI)
					star.State = Star.e_State.AI;
				star.Schedule = 0;
			}
			//战斗中的消耗
			Random random=new Random();
			double playerLoss = star.Troops.Item1 > 0 ? 0.4 * star.Troops.Item2 / (star.Troops.Item1 + star.Troops.Item2 ) * random.Next (8, 12) : 0;
			double AILoss = star.Troops.Item2 > 0 ? 0.4 * star.Troops.Item1 / (star.Troops.Item1 + star.Troops.Item2 ) * random.Next (8, 12) : 0;
			if (star.Troops.Item1 < Math.Exp (-2) && star.Troops.Item2 < Math.Exp (-2)) 
			{
				playerLoss = 0;
				AILoss = 0;
			}

			if(star.State ==Star.e_State.Player)
				playerLoss = playerLoss * (1.1 - (double)star.DEF / 50);
			if (star.State == Star.e_State.AI)
				AILoss = AILoss * (1.1 - (double)star.DEF / 50);
			star.Troops=new Tuple<double, double>(//减去双方损失
				star.Troops.Item1>playerLoss?star.Troops.Item1-playerLoss:0
				,star.Troops.Item2>AILoss?star.Troops.Item2-AILoss:0);

			//战斗结束后转换状态
			if (star.Troops.Item1 == 0 && star.Troops.Item2 > 0) 
			{//AI获胜
				switch (star.State) 
				{
				case Star.e_State.NeutralityToPlayer:
					star.State = Star.e_State.NeutralityToAI;
					star.Schedule = 0;
					break;
				case Star.e_State.Player:
					star.State = Star.e_State.NeutralityToAI;
					star.Schedule = 0;
					break;
				}
			}
			if (star.Troops.Item1 > 0 && star.Troops.Item2 == 0) 
			{//玩家获胜
				switch (star.State) 
				{
				case Star.e_State.NeutralityToAI:
					star.State = Star.e_State.NeutralityToPlayer;
					star.Schedule = 0;
					break;
				case Star.e_State.AI:
					star.State = Star.e_State.NeutralityToPlayer;
					star.Schedule = 0;
					break;
				}
			}


			//行军的军队
			for(int i=0;i<AIToHere.Count;++i)
			{
				AIToHere [i] = new ArmySituation(AIToHere [i].Item1, AIToHere [i].Item2,
					AIToHere [i].Item3, AIToHere [i].Item4 + Situation.Speed);
				if(Situation.GetInstance().Distance[AIToHere[i].Item1,AIToHere[i].Item2]<=AIToHere[i].Item4)//已经抵达了
				{
					star.Troops = new Tuple<double, double> (star.Troops.Item1, star.Troops.Item2 + AIToHere [i].Item3);
					AIToHere.RemoveAt (i);
					--i;
				}
			}
			for(int i=0;i<playerToHere.Count;++i)
			{
				playerToHere [i] = new ArmySituation(playerToHere [i].Item1, playerToHere [i].Item2,
					playerToHere [i].Item3, playerToHere [i].Item4 + Situation.Speed);
				if(Situation.GetInstance().Distance[playerToHere[i].Item1,playerToHere[i].Item2]<=playerToHere[i].Item4)//已经抵达了
				{
					star.Troops = new Tuple<double, double> (star.Troops.Item1 + playerToHere [i].Item3, star.Troops.Item2 );
					playerToHere.RemoveAt (i);
					--i;
				}
			}
			//中立星球军队到达后会有状态的转换
			if (star.State == Star.e_State.NeutralityPeace && star.Troops.Item1 > 0 && star.Troops.Item2 == 0)
				star.State = Star.e_State.NeutralityToPlayer;
			if (star.State == Star.e_State.NeutralityPeace && star.Troops.Item1 == 0 && star.Troops.Item2 > 0)
				star.State = Star.e_State.NeutralityToAI;
			//增加星球上的兵力
			if (star.State == Star.e_State.AI) 
			{//电脑星球
				star.Troops=new Tuple<double, double>(star.Troops.Item1,star.Troops.Item2+(double)star.Vigour/10);
			}
			if (star.State == Star.e_State.Player) 
			{//玩家星球
				star.Troops=new Tuple<double, double>(star.Troops.Item1+(double)star.Vigour/10,star.Troops.Item2);
			}
		}

		//模拟某星球最后的状态,兵力
		public Star GetResult(int starIndex)
		{
			return GetResult (starIndex,99999);
		}
		public Star GetResult(int starIndex, int terminalTime)//参数二代表多少秒后终止
		{
			Star ref_star = Situation.GetInstance ().Stars [starIndex];
			Star star = new Star (ref_star);
			//得到所有开往此地的兵力
			List<ArmySituation> AIToHere=new List<ArmySituation>();
			List<ArmySituation> playerToHere=new List<ArmySituation>();
			for (int i = 0; i < Situation.GetInstance ().AIOnTheWay.Count; ++i) 
			{
				if (Situation.GetInstance ().AIOnTheWay [i].Item2 == starIndex) {
					AIToHere.Add (Situation.GetInstance ().AIOnTheWay [i]);
				}
			}
			for (int i = 0; i < Situation.GetInstance ().PlayerOnTheWay.Count; ++i) 
			{
				if (Situation.GetInstance ().PlayerOnTheWay [i].Item2 == starIndex) 
				{
					playerToHere.Add (Situation.GetInstance ().PlayerOnTheWay [i]);
				}
			}

			int count = 1;
			while (((AIToHere.Count > 0 || playerToHere.Count > 0 || star.Troops.Item1 * star.Troops.Item2 > 0) && terminalTime>1000)
				|| (count <= terminalTime && terminalTime<=1000)) 
			{//战斗还没结束或是还有兵没到
				//每一次变化
				AI.ChangeOnStar(ref star,ref AIToHere,ref playerToHere);
				count++;
				/*
				Console.WriteLine ("第{0}秒:", count);
				Console.WriteLine ("此时星球上的兵力{0}:{1}",star.Troops.Item1,star.Troops.Item2);
				Console.WriteLine ("此时路上玩家的兵力:");
				if (star.State == Star.e_State.NeutralityToAI || star.State == Star.e_State.NeutralityToPlayer)
					Console.WriteLine ("中立星球 占领进度:{0}",star.Schedule);
				foreach (Tuple<int,int,int,int> troop in playerToHere) 
				{
					Console.WriteLine ("从{0}到{1}，人数{2}，已经走了{3}距离",troop.Item1,troop.Item2,troop.Item3,troop.Item4);
				}
				Console.WriteLine ("此时路上电脑的兵力:");
				foreach (Tuple<int,int,int,int> troop in AIToHere) 
				{
					Console.WriteLine ("从{0}到{1}，人数{2}，已经走了{3}距离",troop.Item1,troop.Item2,troop.Item3,troop.Item4);
				}
				Console.WriteLine ();
				*/
			}
			return star;


		}



        //策略
        private List<Tuple3<int,int,int>> Strategy()
        {
			//统计指标值
			Tuple<double,double> total_troops = Situation.GetInstance().GetTotalFight();//双方总兵力
			Tuple<int,int> total_vigour = Situation.GetInstance().GetTotalVigour();//双方总活力
			Tuple<double,double> real_rise = Situation.GetInstance().GetRealLose();//双方净增长


			Console.WriteLine ("双方总兵力({0}:{1})", total_troops.Item1, total_troops.Item2);
			Console.WriteLine ("双方总活力({0}:{1})", total_vigour.Item1, total_vigour.Item2);
			Console.WriteLine ("双方净增长({0}:{1})", real_rise.Item1, real_rise.Item2);

			//计算每个星球的需求
			int num_stars = Situation.GetInstance ().Stars.Count;

			_needs = Situation.GetInstance().GetNeeds();//若玩家不再操作，此时星球A派往星球B多少时确保胜利
			_needed = Situation.GetInstance().GetNeeded();//若双方不再操作，到最后星球剩下多少玩家军队


			//选择策略
			if (total_troops.Item2 > total_troops.Item1 && total_vigour.Item2 > total_vigour.Item1) {//绝对优势
				if (real_rise.Item2 > real_rise.Item1) {
					return Strategy1 ();
				} else {
					return Strategy2 ();
				}
			} else if (total_troops.Item2 <= total_troops.Item1 && total_vigour.Item2 > total_vigour.Item1) {//相对优势
				if (total_troops.Item2 >= total_troops.Item1 * 0.8 || real_rise.Item2 > real_rise.Item1) {//当AI战力大于等于玩家战力乘0.8或电脑总净增长大于等于玩家总净增长时，AI此时应该采取以静制动策略（策略3）
					return Strategy3 ();
					
				} else {
					return Strategy4 ();
				}
			} else if (total_troops.Item2 > total_troops.Item1 && total_vigour.Item2 <= total_vigour.Item1) {
				if (real_rise.Item2 > real_rise.Item1) {
					return Strategy5 ();
				} else {
					return Strategy6 ();
				}
			} else {
				if (real_rise.Item2 > real_rise.Item1) {
					return Strategy3 ();
				} else{
					return Strategy6 ();
				}
			}		
		}

		private double f(double x)
		{
			return 800.0/(15*x+800);
		}

		//策略1
		private List<Tuple3<int,int,int>> Strategy1()
		{
			Console.WriteLine ("策略1");
			int sourceIndex=-1, terminalIndex=-1;
			double obj = 999999;

			_fimportance = new double[Situation.GetInstance().Stars.Count];

			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0) continue;
				_fimportance [i] = f (Situation.GetInstance ().GetImportance (i));
				for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
				{
					if (i == j || _needs[i,j] < 0) continue;
					if(_needed[i]+_needs[i,j]*2 < obj) 
					{
						obj = _needed [i] * _fimportance[i] + _needs [i, j] * 2;
						sourceIndex = i;
						terminalIndex = j;
					}
				}
			}
				
			List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
			if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
			return res;
		}

		//策略2
		private List<Tuple3<int,int,int>> Strategy2()
		{
			Console.WriteLine ("策略2");
			int sourceIndex=-1, terminalIndex=-1;
			double obj = 999999;

			_fimportance = new double[Situation.GetInstance().Stars.Count];

			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0) continue;
				_fimportance [i] = f (Situation.GetInstance ().GetImportance (i));
				for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
				{
					if (i == j || _needs[i,j] < 0) continue;
					Star sourceStar = AI.GetInstance ().GetResult (i);//i星球若不采取行动，最后的状态
					if(sourceStar.State != Star.e_State.AI) continue;
					Star finalStar = AI.GetInstance().GetResult(j,
						(int)(Situation.GetInstance().Distance[i,j]/Situation.Speed));//i的部队到达j时的星球
					if(finalStar.State==Star.e_State.Player) continue;//不扩大战争了
					if(_needed[i]+_needs[i,j]*2 < obj) 
					{
						obj = _needed [i] * _fimportance[i] + _needs [i, j] * 2;
						sourceIndex = i;
						terminalIndex = j;
					}
				}
			}

			List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
			if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
			return res;



	
		}

		//策略3,选取出发点_needed[i]为负且最小(弱目标)，_needs[i,j]为正且最小（强目标），派遣MIN(_needed[i],_needs[i,j])
		private List<Tuple3<int,int,int>> Strategy3()
		{
			Console.WriteLine ("策略3");
			int sourceIndex=-1, terminalIndex=-1;
			double obj = 999999;
			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0) continue;
				for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
				{
					if (i == j || _needs[i,j] <0) continue;
					Star finalStar = AI.GetInstance().GetResult(j,
						(int)(Situation.GetInstance().Distance[i,j]/Situation.Speed));//i的部队到达j时的星球
					if(finalStar.State==Star.e_State.Player) continue;//不扩大战争了
					if(_needed[i]+_needs[i,j]*2 < obj) 
					{
						obj = _needed [i] + _needs [i, j] * 2;
						sourceIndex = i;
						terminalIndex = j;
					}
				}
			}


			List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
			if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
			return res;
		}

		//策略4
		private List<Tuple3<int,int,int>> Strategy4()
		{
			Console.WriteLine ("策略4");
			int sourceIndex=-1, terminalIndex=-1;
			double obj = 999999;
			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0) continue;
				for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
				{
					if (i == j || _needs[i,j] <0) continue;
					Star finalStar = AI.GetInstance().GetResult(j,
                        (int)(Situation.GetInstance().Distance[i,j]/Situation.Speed));//i的部队到达j时的星球
					if(finalStar.State==Star.e_State.Player) continue;//不扩大战争了
					if(_needed[i]+_needs[i,j]*2 < obj) 
					{
						obj = _needed [i] + _needs [i, j] * 2;
						sourceIndex = i;
						terminalIndex = j;
					}
				}
			}


			List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
			if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
			return res;
		}

		//策略5
		private List<Tuple3<int,int,int>> Strategy5()
		{
			Console.WriteLine ("策略5");
			int sourceIndex=-1, terminalIndex=-1;
			double obj = 999999;
			double sum_needed = 0;
			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0) sum_needed+=_needed[i];
			}
			for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
			{
				if (_needed [i] >= 0 && _needed[i]<1.5*sum_needed/Situation.GetInstance().Stars.Count) continue;
				if (Situation.GetInstance().Stars[i].Troops.Item2 == 0 && _needed [i] >= 0) continue;
				for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
				{
					if (i == j || _needs[i,j] <0) continue;
					Star finalStar = AI.GetInstance().GetResult(j,
                        (int)(Situation.GetInstance().Distance[i,j]/Situation.Speed));//i的部队到达j时的星球
					if(finalStar.State==Star.e_State.Player) continue;//不扩大战争了
					if((_needed[i]>0?(0-_needed[i]*_fimportance[terminalIndex]):(_needed[i]+_needs[i,j]*2*_fimportance[terminalIndex])) < obj) 
					{
						obj = _needed [i] > 0 ? -_needed [i] * _fimportance [terminalIndex] : _needed [i] + _needs [i, j] * 2 *_fimportance [terminalIndex];
						sourceIndex = i;
						terminalIndex = j;
					}
				}
			}


			List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
			if(_needed[sourceIndex]>=0) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Floor(Situation.GetInstance().Stars[sourceIndex].Troops.Item2)));
			else if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
			return res;
		}

		//策略6
		private List<Tuple3<int,int,int>> Strategy6()
		{
			Console.WriteLine ("策略6");
					int sourceIndex=-1, terminalIndex=-1;
					double obj = 999999;
					double sum_needed = 0;
					for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
					{
						if (_needed [i] >= 0) sum_needed+=_needed[i];
					}
					for (int i = 0; i < Situation.GetInstance ().Stars.Count; ++i) 
					{
						if (_needed [i] >= 0 && _needed[i]<1.5*sum_needed/Situation.GetInstance().Stars.Count) continue;
						if (Situation.GetInstance().Stars[i].Troops.Item2 == 0 && _needed [i] >= 0) continue;
						for (int j = 0; j < Situation.GetInstance ().Stars.Count; ++j) 
						{
							if (i == j || _needs[i,j] <0) continue;
							Star finalStar = AI.GetInstance().GetResult(j,
                                (int)(Situation.GetInstance().Distance[i,j]/Situation.Speed));//i的部队到达j时的星球
							if(finalStar.State==Star.e_State.Player) continue;//不扩大战争了
					if((_needed[i]>0?-_needed[i]*_fimportance[terminalIndex]:_needed[i]+_needs[i,j]*2*_fimportance[terminalIndex]) < obj) 
							{
								obj = _needed [i] > 0 ? -_needed [i] * _fimportance [terminalIndex] : _needed [i] + _needs [i, j] * 2 *_fimportance [terminalIndex];
								sourceIndex = i;
								terminalIndex = j;
							}
						}
					}


					List<Tuple3<int,int,int>> res= new List<Tuple3<int,int,int>>();
					if(_needed[sourceIndex]>=0) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
				(int)Math.Floor(Situation.GetInstance().Stars[sourceIndex].Troops.Item2)));
						else if(sourceIndex!=-1) res.Add (new Tuple3<int,int,int> (sourceIndex, terminalIndex, 
							(int)Math.Min(Math.Abs(_needed[sourceIndex]),_needs[sourceIndex,terminalIndex])));
						return res;
		}

		//若玩家不再操作，此时星球A派往星球B多少时确保胜利
		public Tuple<double,double> GetNeeded(int srouceIndex,int terminalIndex)
		{
			int counts = (int)(Situation.GetInstance ().Distance [srouceIndex, terminalIndex] / Situation.Speed);
			return AI.GetInstance ().GetResult (terminalIndex, counts).Troops;
		}




    }

}
