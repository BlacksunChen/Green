using System;
using System.Collections.Generic;
using UnityEngine;

namespace Green
{
    public abstract class SummingMethod: MonoBehaviour 
    {
        public abstract Vector2 SummingForce();

        public string Name { get; set; }

        public SummingMethod(string name) : base()
        {

            Name = name;
        }
    }

}
