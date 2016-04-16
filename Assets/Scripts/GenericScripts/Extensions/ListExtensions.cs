using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utilities
{
    public class ListExtensions
    {
        //need test
        public static List<int> RandomPickRange<T>(List<T> list, int range)
        {
            int count = list.Count;
            HashSet<int> intSet = new HashSet<int>();
            while(intSet.Count < range)
            {
                intSet.Add(UnityEngine.Random.Range(0, count));
            }
            return intSet.ToList();
        }
    }
}
