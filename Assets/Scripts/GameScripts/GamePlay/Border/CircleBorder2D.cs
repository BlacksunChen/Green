using UnityEngine;
using System.Collections;
using Generic;

public class CircleBorder2D : MonoBehaviour
{
    public enum Area
    {
        InCircle,
        OutCircle
    }
    [SerializeField, SetProperty("Radius")]
    protected float   _radius;
    [SerializeField, SetProperty("Center")]
    protected Vector2 _center;
    //protected Area    _areaType;
    
    public float Radius
    {
        get
        {
            return _radius;
        }
        set
        {
            _radius = value;
        }
    }

    public Vector2 Center
    {
        get { return _center; }
        set { _center = value; }
    }
    public void Init(float radius, Vector2 center)
    {
        _radius = radius;
        _center = center;
        //_areaType = type;
    }

    public virtual bool IsIntersection(Vector2 A, 
                                       Vector2 B,
                                   out float dist,
                                   out Vector2 point)
    {
        dist = 0f;
        point = new Vector2();

        dist = Vector2.Distance(A, point);
        return Geometry.GetLineSegmentCircleClosestIntersectionPoint(
                    A, B, _center, _radius, ref point);

    }


    public float CircleTheta = 0.1f; // 值越低圆环越平滑
    public Color CircleColor = Color.green; // 线框颜色

    void OnDrawGizmos()
    {
        if (transform == null) return;
        if (CircleTheta < 0.0001f) CircleTheta = 0.0001f;

        // 设置矩阵
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        // 设置颜色
        Color defaultColor = Gizmos.color;
        Gizmos.color = CircleColor;

        // 绘制圆环
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += CircleTheta)
        {
            float x = Radius * Mathf.Cos(theta) + Center.x;
            float y = Radius * Mathf.Sin(theta) + Center.y;
            Vector3 endPoint = new Vector3(x, y, 0);
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
