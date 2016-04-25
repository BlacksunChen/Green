using System;
using UnityEngine;

namespace Green
{
    /// <summary>
    /// 行为抽象类
    /// </summary>
    public abstract partial class SteeringBehavior
    {

        [Flags]
        public enum Type_ : int
        {
            none = 1 << 0,
            seek = 1 << 1,
            flee = 1 << 2,
            arrive = 1 << 3,
            wander = 1 << 4,
            cohesion = 1 << 5,
            separation = 1 << 6,
            allignment = 1 << 7,
            obstacle_avoidance = 1 << 8,
            wall_avoidance = 1 << 9,
            follow_path = 1 << 10,
            pursuit = 1 << 11,
            evade = 1 << 12,
            interpose = 1 << 13,
            hide = 1 << 14,
            flock = 1 << 15,
            offset_pursuit = 1 << 16,
        }
        public Type_ Type;

        public string BehaviorName
        {
            get; set;
        }

        protected MovingEntity _movingEntity;

        protected bool _active;

        
        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SteeringBehavior Create(MovingEntity entity, SteeringBehavior.Type_ type)
        {
            if(type == Type_.seek)
            {
                return new Seek(entity);
            }
            else if(type == Type_.wall_avoidance)
            {
                return new WallAvoidance(entity);
            }
            else if(type == Type_.wander)
            {
                return new Wander(entity);
            }
            return null;
        }
        

        /// <summary>
        /// 计算该行为所用到的力
        /// </summary>
        public abstract Vector2 CalculateForce();

        /// <summary>
        /// 画辅助图像
        /// </summary>
        public virtual void OnDrawGizmos() { }

        /// <summary>
        /// 画辅助界面
        /// </summary>
        public virtual void OnGUI(){ }

        /// <summary>
        /// 激活该行为
        /// </summary>
        public bool Active {  get { return _active; }  set { _active = value; } }
        public void ActiveOn() { _active = true; }
        public void ActiveOff() { _active = false; }

#if UNITY_EDITOR
        /// <summary>
        /// 用于编辑脚本面板
        /// </summary>
        public virtual void OnDrawInspector()
        {

        }
#endif

        protected SteeringBehavior(MovingEntity entity, string name, SteeringBehavior.Type_ type)
        {
            _movingEntity = entity;
            BehaviorName = name;
            Type = type;
        }

        protected void OnGizmosDrawCircleInLocalSpace(Vector3 center, float radius)
        {
            OnGizmosDrawCircle(_movingEntity.transform.localToWorldMatrix, center, radius);
        }

        protected void OnGizmosDrawCircleInWorldSpace(Vector3 center, float radius)
        {
            OnGizmosDrawCircle(Gizmos.matrix, center, radius);
        }

        protected void OnGizmosDrawCircle(Matrix4x4 matrix, Vector3 center, float radius)
        {
            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = matrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = Color.red;

            // 绘制圆环
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
            {
                float x = radius * Mathf.Cos(theta) + center.x;
                float y = radius * Mathf.Sin(theta) + center.y;
                Vector3 endPoint = new Vector3(x, y, center.z);
                if (theta == 0)
                {
                    firstPoint = endPoint;
                }
                else
                {
                    Gizmos.DrawLine(beginPoint, endPoint);
                }
                beginPoint = endPoint;
            }

            // 绘制最后一条线段
            Gizmos.DrawLine(firstPoint, beginPoint);

            // 恢复默认颜色
            Gizmos.color = defaultColor;

            // 恢复默认矩阵
            Gizmos.matrix = defaultMatrix;
        }

    }
}
