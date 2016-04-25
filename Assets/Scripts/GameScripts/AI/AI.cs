using System;
using System.Collections.Generic;

/**
 * 
 * AI类 实现AI的相关功能
 * 
 */

namespace Green
{
	public class AI
	{
		//单例类
		private AI()
		{
			//统计situation里面士兵的分布情况
			StatistDistribution();
			//计算双方的属性和
			caculateTotalProperty ();
		}
		//实例
		private AI _instance;

		//获取实例
		public static AI GetInstance()
		{
			if (_instance == null)
			{
				_instance = new AI ();
			}

			//统计situation里面士兵的分布情况
			StatistDistribution();
			//计算双方的属性和
			caculateTotalProperty ();
			return _instance;
		}

		/**
		 * 接口设定
		 * 在GameWorld里Planets获取所有星球
		 * 在Planet类里GetProperty返回star star里面有双方兵力以及各属性
		 * star里面PlayerTroops和EnemyTroops表示双方兵力
		 * 每个Solidier里面TimeToDestination是他到目的地所需时间，如果他的状态是在路上的话
		 */
		struct SoldierOnTheWay
		{
			public int m_origin;//起点
			public int m_terminal;//终点
			public float m_needTime;//还剩几秒到达目的地;
		}

		//玩家在路上的兵力及还有多少秒到达目的地
		private List<SoldierOnTheWay> _playerOnTheWay;

		//AI在路上的兵力及还有多少秒到达目的地
		private List<SoldierOnTheWay> _aiOnTheWay;

		//所有星球的情况 复制过来 因为会模拟导致会改动
		private List<Star> _starCopys;

		//玩家总活力
		private int _playerTotalVigour;

		//AI总活力
		private int _aiTotalVigour;

		//玩家总容量
		private int _playerTotalCapacity;

		//AI总容量
		private int _aiTotalCapacity;

		//获取situation里面的双方兵力分布情况
		private void StatistDistribution()
		{
			_starCopys = new List<Star> ();
			//获取GameWorld里面Star的list
			foreach(Star star in GameWorld.Instance.Planets)
			{
				_starCopys.Add (star);
			}
			//获取在路上的士兵信息
			_playerOnTheWay = new List<SoldierOnTheWay>();
			_aiOnTheWay = new List<SoldierOnTheWay> ();
			foreach(Soldier soldier in GameWorld.Instance.Soldiers)
			{
				if (soldier.CurrentType == Soldier.StateType.Move) 
				{//若士兵的状态是正在移动的状态
					SoldierOnTheWay soldierOnTheWay = new SoldierOnTheWay();
					soldierOnTheWay.m_needTime = soldier.TimeToDestination;//还有多少秒到达目的地
					soldierOnTheWay.m_origin = soldier.FromPlanetID;
					soldierOnTheWay.m_terminal = soldier.ToPlanetID;

					//判断这个士兵是哪一方的就加到对应的list
					if (soldier.Bloc == SoldierType.Player) 
					{
						_playerOnTheWay.Add (soldierOnTheWay);
					} else if(soldier.Bloc == SoldierType.Enemy)
					{
						_aiOnTheWay.Add (soldierOnTheWay);
					}
				}
			}
		}

		//获取双方属性和（容量和and活力和）
		private void caculateTotalProperty()
		{
			//双方容量和
			_aiTotalCapacity = GameWorld.Instance.EnemyMaxPopulation;
			_playerTotalCapacity = GameWorld.Instance.PlayerMaxPopulation;
			//双方活力和
			_aiTotalVigour = 0;
			_playerTotalVigour = 0;
			foreach(Star star in _starCopys) 
			{
				//加到对应里面
				if (star.State == Star.e_State.AI) 
				{
					_aiTotalVigour += star.Vigour;
				} else if (star.State == Star.e_State.Player) 
				{
					_playerTotalVigour += star.Vigour;
				}
			}

		}

		/*
		 * 模拟双方都不派兵的情况下某个星球t秒后状态 t=-1时为无穷时间
		 * 公式都放在Formula里面
		 * 
		 * 
		 */
		private Star CalculateFuture(int planetId,int t)
		{
			
		}


		/**
		 * 
		 * AI相关计算公式
		 * 
		 * 
		 */


		/**
		 * 2 to 0.5递减的缓和公式
		 * 输入是AI和玩家某属性的比值
		 * 输出是属性的权值（也就是属性的重要程度）
		 * 此函数是为了使AI越在某个属性上占劣势，这个属性越重要
		 */
		float AttributeImportanceFormula(float attributeRate)
		{
			return (attributeRate + 2) / (2 * attributeRate + 1);
		}

		/**
		 * 0.5 to 1递增的缓和公式
		 * 输入是星球三个属性的加权求和
		 * 输出是星球的最终重要程度（没有算上地理位置和实际情况我很抱歉）
		 * 此函数是为了使最重要的星球也不会比最不重要的差别太大
		 */
		float StarImportanceFormula(float attributeSum)
		{
			return (2 * attributeSum + 1) / (2 * attributeSum + 2);
		}

		/**
		 * 过剩兵力系数公式
		 * 1 to 0.5递减的缓和公式
		 * 输入是星球重要程度
		 * 输出是过剩兵力系数
		 * 此函数是为了重要的星球留较多的人 不怎么重要的星球留较少的人
		 */
		float ExcessForceCoefficientFormula(float starImportance)
		{
			return 1.5 - starImportance;
		}

		//计算某星球过剩兵力
		int CaculateExcessForceCoefficient(int starIndex)
		{
			//获取星球最后情况
			Star finalStar = CalculateFuture(starIndex,-1);
			return Math.Floor(finalStar.EnemyTroops - finalStar.PlayerTroops) //剩下的兵力
				* ExcessForceCoefficientFormula (StarImportance (starIndex)); //过剩兵力系数
		}

		//计算某星球的重要程度
		float StarImportance(Star star)
		{
			//f(g(双方总容量比)*星球容量+g(双方总活力比)*星球活力+星球防御力)
			return StarImportanceFormula (AttributeImportanceFormula ((float)_aiTotalCapacity / _playerTotalCapacity) * star.Capacity
			+ AttributeImportanceFormula ((float)_aiTotalVigour / _playerTotalVigour) * star.Vigour
			+ star.DEF);
		}

		float  StarImportance(int starIndex)
		{
			return StarImportance (_starCopys[starIndex]);
		}
	}
}

