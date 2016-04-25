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
			return _instance;
		}

		/**
		 * 接口设定
		 * 在GameWorld里Planets获取所有星球
		 * 在Planet类里GetProperty返回star star里面有双方兵力以及各属性
		 * star里面PlayerTroops和EnemyTroops表示双方兵力
		 *
		 */
		private struct soldierOnTheWay
		{
			int m_origin;//起点
			int m_terminal;//终点
			float m_needTime;//还剩几秒到达目的地;
		}

		//玩家在路上的兵力及还有多少秒到达目的地
		private List<soldierOnTheWay> _playerOnTheWay;

		//AI在路上的兵力及还有多少秒到达目的地
		private List<soldierOnTheWay> _aiOnTheWay;

		//所有星球的情况 复制过来 因为会模拟导致会改动
		private List<Star> _starCopys;

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

		}



	}
}

