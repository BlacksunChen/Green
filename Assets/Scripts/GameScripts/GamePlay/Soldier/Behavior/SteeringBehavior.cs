using UnityEngine;

namespace Green
{
    /// <summary>
    /// 行为抽象类
    /// </summary>
    public abstract class SteeringBehavior
    {
        public SteeringBehaviors.behavior_type Type;
        public abstract Vector2 CalculateForce();
        public virtual void OnDrawGizmos() { }
        public virtual void OnGUI(){ }

        protected bool _active;

        public bool IsActive() { return _active; }
        public void ActiveOn() { _active = true; }
        public void ActiveOff() { _active = false; }

        protected void OnGizmosDrawCircle(Vector3 center, float radius)
        {
            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            //Gizmos.matrix = transform.localToWorldMatrix;

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
            //Gizmos.matrix = defaultMatrix;
        }
    }
}
