using UnityEngine;
using System.Collections;
using Generic;

public class CircleWall2D : MonoBehaviour
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

    /// <summary>
    /// 切线的法线
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public virtual Vector2 GetTangentNormal(Vector2 point)
    {
        return new Vector2(point.x - _center.x, point.y - _center.y).normalized;
    }

}
