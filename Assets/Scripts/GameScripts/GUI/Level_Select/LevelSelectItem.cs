using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Green
{
    //[ExecuteInEditMode]
    public class LevelSelectItem : MonoBehaviour//, IComparable<LevelSelectItem>
    {
        public Text LevelNameText;
        public Text LevelNumberText;
        public Image LevelImage;
        public string LevelNumber;
        public string LevelName;
        public LevelSelectMenu Menu;
        public int PositionInItems = 0;
        public int ChapterNum = 0;

        manage_menu_uGUI gui = null;

        void Start()
        {
            Menu = transform.parent.parent.GetComponent<LevelSelectMenu>();
            var texts = Menu.GetComponentsInChildren<Text>();
            foreach (var t in texts)
            {
                if (t.name == "LevelName")
                {
                    LevelNameText = t;
                }
                else if (t.name == "LevelNumber")
                {
                    LevelNumberText = t;
                }
            }
           // LevelNameText = GameObject.Find("LevelName").GetComponent<Text>();
           // LevelNumberText = GameObject.Find("LevelNumber").GetComponent<Text>();
            LevelImage = GetComponentInChildren<Image>();
            LevelImage.raycastTarget = false;
            LevelNameText.text = LevelName;
            CreateButton();

            OriginScale = LevelImage.GetComponent<RectTransform>().localScale.x;

            gui = GameObject.FindObjectOfType<manage_menu_uGUI>();
        }

        public  void GenerateLevelNumber()
        {
            LevelNumberText.text = "第" + LevelNumber + "节";
            LevelNameText.text = LevelName;
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        void CreateButton()
        {
            var btnGo = Resources.Load<GameObject>("Prefabs/GUI/Button");
            btnGo = GameObject.Instantiate(btnGo);
            var btn = btnGo.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogError("Need Button Prefab in Level_Sclect_Item");
            }
            btn.targetGraphic = LevelImage;
            btn.onClick.AddListener(OnClickItem);
            var t = btn.GetComponent<RectTransform>();
            t.SetParent(transform);
            t.sizeDelta = new Vector2(180f, 180f);
            t.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            t.localScale = new Vector3(1f,1f,1f);
        }
        public SpriteRenderer GetImage()
        {
            return LevelImage.GetComponentInChildren<SpriteRenderer>();
        }


        public float OriginScale;

        public void DoScaleLarge(float largeScale)
        {
            LevelImage.GetComponent<RectTransform>().DOScale(largeScale, 0.3f);
        }

        public void DoScaleOrigin()
        {
            LevelImage.GetComponent<RectTransform>().DOScale(OriginScale, 0.3f);
        }

        public void OnClickItem()
        {
            if (Menu.itemsList[Menu.CurrentItem] == this)
            {
                gui.loading_screen.gameObject.SetActive(true);
                var sceneName = ChapterNum.ToString() + "_" + LevelNumber.ToString();
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                LevelNameText.text = LevelName;
                GenerateLevelNumber();
                Menu.gotoItem(this);
            }
            
        }
    }
}