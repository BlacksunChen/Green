using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Green
{
    public class Formula
    {
        public const float CalculatePerTime = 1f;
        /*
        static BattleManager _instance = null;

        

        Timer _time = new Timer(1, true, true);
        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }
                return _instance;
            }
        }
        */
        /// <summary>
        /// 计算1s内防守方伤亡人数
        /// </summary>
        /// <param name="defenceCount">防守方人数</param>
        /// <param name="attackCount">攻击方人数</param>
        /// <param name="DEF">星球防御力</param>
        /// <param name="perTime">几秒算一次， 默认公式为一秒后的数值</param>
        /// <returns></returns>
        public static float CalculateDamageForDefOnePerTime(float defenceCount, float attackCount, float DEF, float perTime)
        {
            return -((attackCount / (attackCount + 4f * defenceCount) * (1 - DEF / 20f)) * perTime);
        }

        /// <summary>
        /// 计算1s内攻击方伤亡人数
        /// </summary>
        /// <param name="defenceCount"></param>
        /// <param name="attackCount"></param>
        /// <param name="DEF"></param>
        /// <param name="perTime"></param>
        /// <returns></returns>
        public static float CalculateDamageForAttackOnePerTime(float defenceCount, float attackCount, float DEF, float perTime)
        {
            return -(defenceCount / (defenceCount + 4f * attackCount));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defenceCount"></param>
        /// <param name="attackCount"></param>
        /// <param name="DEF"></param>
        /// <param name="perTime"></param>
        /// <returns></returns>
        public static float CalculateDamageForNeutralOnePerTime(float defenceCount, float attackCount, float perTime)
        {
            return -(attackCount / (attackCount + 4 * defenceCount)) * perTime;
        }

        public static float CalculateCaptureProgress(float soldierCount)
        {
            return (5*soldierCount+11f) / (310 + 10f * soldierCount);
        }

        public const float TimeSoldierIncreasePer = 40f;

        public static float CalculateSoldierIncreasePerTime(float vigour)
        {
            return vigour / TimeSoldierIncreasePer * CalculatePerTime;
        }
    }
}
