using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic;
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
            Members = new LinkedList<T>();
        }
        
    }
}