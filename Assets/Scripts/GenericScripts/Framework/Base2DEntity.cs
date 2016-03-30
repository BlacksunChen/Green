using UnityEngine;
using System.Collections;

public class Base2DEntity : MonoBehaviour
{
    //this is a generic flag. 
    bool m_bTag;

    protected float _boundingRadius;

    protected Vector2 _scale;

    public float BoundingRadius
    {
        get
        {
            return _boundingRadius;
        }
        set
        {
            _boundingRadius = value;
        }
    }

    public Vector2 Scale
    {
        get
        {
            return this.transform.localScale;
        }
        set
        {
            float z = transform.localScale.z;
            this.transform.localScale = new Vector3(value.x, value.y, z);
        }
    }

    public void SetScale(Vector2 val)
    {
        _boundingRadius *= Mathf.Max(val.x, val.y) / Mathf.Max(Scale.x, Scale.y);
        Scale = val;
    }

    public void SetScale(float val)
    {
        _boundingRadius *= (val / Mathf.Max(_scale.x, _scale.y));
        _scale = new Vector2(val, val);
    }

    public Vector2 Position
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            float z = transform.position.z;
            this.transform.position = new Vector3(value.x, value.y, z);
        }
    }

    public bool IsTagged()  { return m_bTag;  }
    public void Tag()       { m_bTag = true;  }
    public void UnTag()     { m_bTag = false; }

// Use this for initialization
void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
