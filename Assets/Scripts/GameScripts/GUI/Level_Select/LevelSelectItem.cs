using System;
using UnityEngine;
using System.Collections;
using System.Security.Policy;
using DG.Tweening;
using UnityEngine.UI;
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

        void Start()
        {
            LevelNameText = GameObject.Find("LevelName").GetComponent<Text>();
            LevelNumberText = GameObject.Find("LevelNumber").GetComponent<Text>();
            LevelImage = GetComponent<Image>();
            LevelNameText.text = LevelName;
            LevelNumberText.text = LevelNumber;

            OriginScale = LevelImage.GetComponent<RectTransform>().localScale.x;
            var button = gameObject.AddComponent<Button>();
            button.targetGraphic = LevelImage;
            button.onClick.AddListener(OnClickItem);

        }

        // Update is called once per frame
        void Update()
        {

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
            Menu.gotoItem(this);
        }
    }
}