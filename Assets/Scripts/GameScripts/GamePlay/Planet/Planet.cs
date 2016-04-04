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

        public CircleBorder2D OutCircle;

        public CircleBorder2D InCircle;

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
            //反正是同心圆
            var center = InCircle.Center;
            return new Vector2(point.x - center.x, point.y - center.y).normalized;
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
            var walls = GetComponentsInChildren<CircleBorder2D>();
            bool inExist = false;
            bool onExist = false;
            foreach(var wall in walls)
            {
                if(wall.gameObject.name == "In")
                {
                    inExist = true;
                    InCircle = wall;
                    
                }
                if(wall.gameObject.name == "Out")
                {
                    onExist = true;
                    OutCircle = wall;
                }
            }
            if(!InCircle && !OutCircle)
            {
                Debug.LogError("Planet need both in and out border!");
            }
            //内圈
            
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}