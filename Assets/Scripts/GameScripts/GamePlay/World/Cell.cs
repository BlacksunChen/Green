using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Green
{
    public class Cell<T>
        where T : Base2DEntity
    {
        public LinkedList<T> Members;

        public Bounds BoundBox;

        public Cell(Vector2 center, Vector2 size)
        {
            BoundBox = new Bounds(center, size);
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