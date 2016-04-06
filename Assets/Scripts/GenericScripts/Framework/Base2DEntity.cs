using UnityEngine;

namespace Generic
{
    public class Base2DEntity : MonoBehaviour
    {
        //this is a generic flag. 
        bool m_bTag;


        [SerializeField, SetProperty("Scale")]
        protected float _scale = 0.05f;

        //used by the constructor to give each entity a unique ID
        //each entity has a unique ID
        private int _id;
        private static int NextID = 0;
        private int NextValidID() { return NextID++; }

        public int ID
        {
            get { return _id; }
        }
        public Base2DEntity() : base()
        {
            _id = NextValidID();
        }



        public float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
        /*
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
        */
        public Vector2 Position
        {
            get
            {
                return this.transform.position;
            }
            set
            {
                var z = transform.position.z;
                this.transform.position = new Vector3(value.x, value.y, z);
            }
        }

        public bool IsTagged() { return m_bTag; }
        public void Tag() { m_bTag = true; }
        public void UnTag() { m_bTag = false; }

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