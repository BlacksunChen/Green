using UnityEngine;
using System.Collections;

namespace Green
{
    [ExecuteInEditMode]
    public class LevelSelectItem : MonoBehaviour
    {

        public GameObject LevelName;
        public GameObject LevelImage;
        public GameObject LevelBar;

        //GameObject _levelName;
        //GameObject _levelImage;
        //GameObject _levelBar;
        // Use this for initialization
        void Start()
        {
            foreach (Transform t in transform)
            {
                switch (t.name)
                {
                    case "level_bar":
                        LevelBar = t.gameObject;
                        break;
                    case "level_image":
                        LevelImage = t.gameObject;
                        break;
                    case "level_name":
                        LevelName = t.gameObject;
                        break;
                }
            }
            if (!LevelName || !LevelBar || !LevelImage)
            {
                Debug.LogErrorFormat("Component in {0} missing!", gameObject.name);
            }
            if (LevelImage.GetComponent<ClickTouchScript>() == null)
            {
                LevelImage.AddComponent<ClickTouchScript>().LevelItem = this;
            }
            if (LevelImage.GetComponent<BoxCollider>() == null)
            {
                LevelImage.AddComponent<BoxCollider>();
            }
            LevelName.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public SpriteRenderer GetImage()
        {
            return LevelImage.GetComponentInChildren<SpriteRenderer>();
        }
    }
}