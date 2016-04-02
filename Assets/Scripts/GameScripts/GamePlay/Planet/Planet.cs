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

        public CircleWall2D OutCircle;

        public CircleWall2D InCircle;

        public bool IsIntersection(Vector2 A,
                                   Vector2 B,
                               out float dist,
                               out Vector2 point)
        {
            bool isOut = false, isIn = false;
            isOut = OutCircle.IsIntersection(A, B, out dist, out point);
            if (isOut)
            {
                return isOut;
            }
            isIn = InCircle.IsIntersection(A, B, out dist, out point);
            if(isIn)
            {
                return isIn;
            }
            return false;
        }

        /// <summary>
        /// 切线的法线
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Vector2 GetTangentNormal(Vector2 point)
        {
            return new Vector2(point.x - _center.x, point.y - _center.y).normalized;
        }

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
            SetCircleWall();
        }

        void SetCircleWall()
        {
            var walls = GetComponentsInChildren<CircleWall2D>();
            bool inExist = false;
            bool onExist = false;
            foreach(var wall in walls)
            {
                if(wall.gameObject.name == "InCircle")
                {
                    inExist = true;
                    
                }
                if(wall.gameObject.name == "OutCircle")
                {
                    onExist = true;
                }
            }
            //内圈
            
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}