using UnityEngine;
using System.Collections;

namespace Green
{
    /// <summary>
    /// 星球类
    /// </summary>
    public class Planet : MonoBehaviour
    {

        public enum WarState
        {
            Peace,
            War
        }

        public enum CaptureState
        {
            Player,
            Enemy,
            None
        }

        /// <summary>
        /// 防御力
        /// </summary>
        public float Defence;

        /// <summary>
        /// 容量
        /// </summary>
        public float Capability;

        /// <summary>
        /// 活力值
        /// </summary>
        public float Energy;

        /// <summary>
        /// 玩家兵力
        /// </summary>
        public float PlayerForces;

        /// <summary>
        /// 敌人兵力
        /// </summary>
        public int EnemyForces;

        public Vector2 Position;


        /// <summary>
        /// 根据星球类型读取配置文件
        /// 创建prefab
        /// </summary>
        public void Init()
        {

        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}