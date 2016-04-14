using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Green
{
    public class BattleManager
    {
        static BattleManager _instance = null;
        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }
                return _instance;
            }
        }
    }
}
