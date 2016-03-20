using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class EnhancelScrollView : MonoBehaviour
{
    // 缩放曲线
    public AnimationCurve scaleCurve;
    // 位移曲线
    public AnimationCurve positionCurve;
    // 位移系数
    public float _posCurveFactor = 500.0f;
    // y轴坐标固定值(所有的item的y坐标一致)
    public float yPositionValue;

    public GameObject Images;

    // 添加到EnhanceScrollView的目标对象
    private List<EnhanceItem> _scrollViewItems = new List<EnhanceItem>();

    private List<Image> _scrollImages = new List<Image>();

    // 当前处于中间的item
    private int centerIdx;
    private int preCenterItem;


    // 当前出移动中，不能进行点击切换
    private bool canChangeItem = true;

    // 计算差值系数
    public float dFactor = 0.2f;
    
    // 点击目标移动的横向目标值
    private float[] moveHorizontalValues;
    // 对象之间的差值数组(根据差值系数算出)
    private float[] dHorizontalValues;

    // 横向变量值
    public float horizontalValue = 0.0f;
    // 目标值
    public float horizontalTargetValue = 0.1f;

    // 移动动画参数
    private float originHorizontalValue = 0.1f;
    public float duration = 0.2f;
    private float currentDuration = 0.0f;

    private static EnhancelScrollView instance;
    public static EnhancelScrollView GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    float GetItemsPosXGap()
    {
        float min = float.MaxValue;
        int i, j;
        for(i = 0, j = i + 1; i < _scrollViewItems.Count && j < _scrollViewItems.Count; ++i, ++j)
        {
            var item1 = _scrollViewItems[i].GetComponent<RectTransform>().anchoredPosition.x;
            var item2 = _scrollViewItems[j].GetComponent<RectTransform>().anchoredPosition.x;
            if (min > Mathf.Abs(item1- item2))
            {
                min = Mathf.Abs(item1 - item2);
            }
        }
        return min;
    }

    void Start()
    {
        foreach(Transform item in Images.transform)
        {
            _scrollImages.Add(item.GetComponent<Image>());
            _scrollViewItems.Add(item.gameObject.AddComponent<EnhanceItem>());
        }

        yPositionValue = _scrollViewItems[0].GetComponent<RectTransform>().anchoredPosition.y;
        _posCurveFactor = GetItemsPosXGap();
        if ((_scrollViewItems.Count % 2) == 0)    
        {
            Debug.LogError("item count is invaild,please set odd count! just support odd count.");
        }

        if(moveHorizontalValues == null)
            moveHorizontalValues = new float[_scrollViewItems.Count];

        if(dHorizontalValues == null)
            dHorizontalValues = new float[_scrollViewItems.Count];

        int centerIndex = GetTopItemIdx();
        for (int i = 0; i < _scrollViewItems.Count; i++)
        {
            _scrollViewItems[i].scrollViewItemIndex = i;
            Image tmpTexture = _scrollViewItems[i].gameObject.GetComponent<Image>();

            dHorizontalValues[i] = dFactor * (centerIndex - i);

            dHorizontalValues[centerIndex] = 0.0f;
            moveHorizontalValues[i] = 0.5f - dHorizontalValues[i];
            _scrollViewItems[i].SetSelectColor(false);
        }
        
        centerIdx = centerIndex;
        canChangeItem = true;
    }

    private int GetTopItemIdx()
    {
        int max = int.MinValue;
        for(int i = 0; i < _scrollViewItems.Count; ++i)
        {
            if(max < _scrollViewItems[i].GetComponent<RectTransform>().GetSiblingIndex())
            {
                max = i;
            }
        }
        return max;
    }
    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < _scrollViewItems.Count; i++)
        {
            EnhanceItem itemScript = _scrollViewItems[i];
            float xValue = GetXPosValue(fValue, dHorizontalValues[i]);
            float scaleValue = GetScaleValue(fValue, dHorizontalValues[i]);
            itemScript.UpdateScrollViewItems(xValue, yPositionValue, scaleValue);
        }
    }

    void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > duration)
        {
            // 更新完毕设置选中item的对象即可
            currentDuration = duration;
            if(_scrollImages[centerIdx] == null)
                _scrollViewItems[centerIdx].SetSelectColor(true);
            if(_scrollImages[preCenterItem] != null)
                _scrollViewItems[preCenterItem].SetSelectColor(false);
            canChangeItem = true;
        }
        SortDepth();
        float percent = currentDuration / duration;
        horizontalValue = Mathf.Lerp(originHorizontalValue, horizontalTargetValue, percent);
        UpdateEnhanceScrollView(horizontalValue);
    }

    /// <summary>
    /// 缩放曲线模拟当前缩放值
    /// </summary>
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * _posCurveFactor;
        return evaluateValue;
    }

    public void SortDepth()
    {
        /*
        _scrollImages.Sort(new CompareDepthMethod());
        for (int i = 0; i < _scrollViewItems.Count; i++)
            _scrollViewItems[i].GetComponent<RectTransform>().SetSiblingIndex(i);
        */
        //以centerItem为中心，向两边递减层级
        int left, right;
        left = right = centerIdx;
        if (_scrollViewItems[centerIdx] != null)
        {
            _scrollViewItems[centerIdx].GetComponent<RectTransform>().SetAsFirstSibling();
        }
        left -= 1;
        right += 1;

        while (left < 0  && right >= _scrollViewItems.Count)
        {
            if(left >=0)
            {
                _scrollViewItems[left].GetComponent<RectTransform>().SetAsFirstSibling();
            }   
            if(right < _scrollViewItems.Count)
            {
                _scrollViewItems[right].GetComponent<RectTransform>().SetAsFirstSibling();
            }
            left -= 1;
            right += 1;
        }
    }

    /// <summary>
    /// 用于层级对比接口
    /// </summary>
    public class CompareDepthMethod : IComparer<Image>
    {
        public int Compare(Image left, Image right)
        {
            if (left.transform.localScale.x > right.transform.localScale.x)
                return 1;
            else if (left.transform.localScale.x < right.transform.localScale.x)
                return -1;
            else
                return 0;
        }
    }

    /// <summary>
    /// 获得当前要移动到中心的Item需要移动的factor间隔数
    /// </summary>
    private int GetMoveCurveFactorCount(float targetXPos)
    {
        for (int i = 0; i < _scrollViewItems.Count;i++ )
        {
            float factor = (0.5f - dFactor * (centerIdx - i));

            float tempPosX = positionCurve.Evaluate(factor) * _posCurveFactor;
            if (Mathf.Abs(targetXPos - tempPosX) < 0.01f)
                return Mathf.Abs(i - centerIdx);
        }
        return -1;
    }

    /// <summary>
    /// 设置横向轴参数，根据缩放曲线和位移曲线更新缩放和位置
    /// </summary>
    public void SetHorizontalTargetItemIndex(int itemIndex)
    {
        if (!canChangeItem)
            return;

        EnhanceItem item = _scrollViewItems[itemIndex];
        if (centerIdx == itemIndex)
            return;

        canChangeItem = false;
        preCenterItem = centerIdx;
        centerIdx = itemIndex;

        // 判断点击的是左侧还是右侧计算ScrollView中心需要移动的value
        float centerXValue = positionCurve.Evaluate(0.5f) * _posCurveFactor;
        bool isRight = false;
        if (item.GetComponent<RectTransform>().anchoredPosition.x > centerXValue)
            isRight = true;

        // 差值,计算横向值
        int moveIndexCount = GetMoveCurveFactorCount(item.GetComponent<RectTransform>().anchoredPosition.x);
        if (moveIndexCount == -1)
        {
            Debug.LogWarning("*****Move Index count is invalid.");
            moveIndexCount = 1; 
        }

        float dvalue = 0.0f;
        if (isRight)
            dvalue = -dFactor * moveIndexCount;
        else
            dvalue = dFactor * moveIndexCount;

        // 更改target数值，平滑移动
        horizontalTargetValue += dvalue;
        currentDuration = 0.0f;
        originHorizontalValue = horizontalValue;
    }

    /// <summary>
    /// 向右选择角色按钮
    /// </summary>
    public void OnBtnRightClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = centerIdx + 1;
        if (targetIndex > _scrollViewItems.Count - 1)
            targetIndex = 0;
        SetHorizontalTargetItemIndex(targetIndex);
    }

    /// <summary>
    /// 向左选择按钮
    /// </summary>
    public void OnBtnLeftClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = centerIdx - 1;
        if (targetIndex < 0)
            targetIndex = _scrollViewItems.Count - 1;
        SetHorizontalTargetItemIndex(targetIndex);
    }
}
