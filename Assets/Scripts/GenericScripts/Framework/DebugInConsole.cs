using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utilities
{
    public class DebugInConsole
    {
        public static void LogFormat(string str, params object[] p)
        {
            //System.Console.WriteLine(str, p);
            //Debug.LogFormat(str, p);
        }

        public static void Log(string str)
        {
            //System.Console.WriteLine(str);
            //Debug.Log(str);
        }
    }
}
