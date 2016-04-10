using System;
using System.Collections.Generic;
using UnityEngine;

namespace Green
{
    /// <summary>
    /// 所有行为组合计算
    /// </summary>
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
