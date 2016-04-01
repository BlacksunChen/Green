using UnityEngine;
using System.Collections;

public class CircleWall2D : MonoBehaviour
{
    protected float   _radius;
    protected Vector2 _center;

    public void Init(float radius, Vector2 center)
    {
        _radius = radius;
        _center = center;
    }

    public virtual bool IsIntersection(Vector2 A, 
                                       Vector2 B,
                                   out float dist,
                                   out Vector2 point)
    {
        dist = 0f;
        point = new Vector2();
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
