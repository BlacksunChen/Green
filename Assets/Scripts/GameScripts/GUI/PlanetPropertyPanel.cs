using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace  Green
{
    public class PlanetPropertyPanel : MonoBehaviour
    {
        public Text 活力;
        public Text 防御力;
        public Text 兵力;
        public Text 容量;

        // Use this for initialization
        void Start()
        {
            var texts = GetComponentsInChildren<Text>();
            foreach (var t in texts)
            {
                switch (t.name)
                {
                    case "活力":
                        活力 = t;
                        break;
                    case "防御力":
                        防御力 = t;
                        break;
                    case "兵力":
                        兵力 = t;
                        break;
                    case "容量":
                        容量 = t;
                        break;
                }
            }
            if (!活力 || !防御力 || !兵力 || !容量)
            {
                Debug.LogError("PlanetPropertyPanel: 获取属性Text失败!");
            }
        }

        Star _star = null;
        public void Show(Star star)
        {
            _star = star;
        }

        
        // Update is called once per frame
        void Update()
        {
            if (_star != null)
            {
                活力.text = _star.Vigour.ToString();
                防御力.text = _star.DEF.ToString();
                兵力.text = ((int) _star.PlayerTroops).ToString();
                容量.text = _star.Capacity.ToString();
            }
        }
    }
}

