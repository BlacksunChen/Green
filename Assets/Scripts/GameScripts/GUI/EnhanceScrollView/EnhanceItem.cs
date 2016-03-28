using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EnhanceItem : MonoBehaviour {

    // 在ScrollViewitem中的索引
    // 定位当前的位置和缩放
    public int scrollViewItemIndex = 0;
    public bool inRightArea = false;

    private Vector2 targetPos = Vector2.one;
    private Vector3 targetScale = Vector3.one;

    private RectTransform _transform;
    private Image mTexture;
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _transform = this.GetComponent<RectTransform>();
        mTexture = this.GetComponent<Image>();
    }

    void Start()
    {
        this.gameObject.AddComponent<Button>().onClick.AddListener(OnClickScrollViewItem);
    }

    // 当点击Item，将该item移动到中间位置
    private void OnClickScrollViewItem()
    {
        EnhancelScrollView.GetInstance().SetHorizontalTargetItemIndex(scrollViewItemIndex);
    }

    /// <summary>
    /// 更新该Item的缩放和位移
    /// </summary>
    public void UpdateScrollViewItems(float xValue, float yValue, float scaleValue)
    {
        targetPos.x = xValue;
        targetPos.y = yValue;
        targetScale.x = targetScale.y = scaleValue;

        _transform.anchoredPosition = targetPos;
        _transform.localScale = targetScale;
    }

    public void SetSelectColor(bool isCenter)
    {
        if (mTexture == null)
            mTexture = this.GetComponent<Image>();

        if (isCenter)
            mTexture.color = Color.white;
        else
            mTexture.color = Color.gray;
    }
}
