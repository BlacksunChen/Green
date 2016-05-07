using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterSelector : MonoBehaviour
{
    public int CurrentChapterNumber;
    public Dictionary<int, LevelSelectChapter> Chapters = new Dictionary<int, LevelSelectChapter>();
    public Sprite IndicatorLargeImage;
    public Sprite IndicatorSmallImage;
    public Dictionary<int, Image> Indicators = new Dictionary<int, Image>();

   
    void Awake()
    {
        
    }

    public Button NextBtn;
    public Button BackBtn;

    public bool TouchSlide = false;
    // Use this for initialization
    void Start()
    {
        var cs = GetComponentsInChildren<LevelSelectChapter>();
        foreach (var c in cs)
        {
            Chapters.Add(c.ChapterNumber, c);
        }
        CurrentChapterNumber = 0;
        var indicators  = GameObject.Find("ChapterIndicator").GetComponentsInChildren<Image>().ToList();
        if(indicators == null)
            Debug.LogError("Missing Chapter Indicators");

        foreach (var i in indicators)
        {
            Indicators.Add(int.Parse(i.name), i);
        }
        foreach (var c in Chapters)
        {
            c.Value.SetActive(false);
        }
        //SetCurrentChapterWithoutAnim(0);
        Chapters[0].SetActive(true);
        SetIndicator(0);
        UpdateChapterInfo();
        SetLevelInfo();
    }

    void SetCurrentChapterWithoutAnim(int number)
    {
        if (number < 0 || number >= Chapters.Count)
        {
            Debug.LogError("Wrong Chapter Number!");
        }
        var cur = Chapters[number];
        foreach (var c in Chapters)
        {
            if (c.Value != cur)
            {
                //c.Value.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, -180f);
                c.Value.SetRotateToUp();
            }
        }
        //cur.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 0f);
        cur.SetRotateToDown();
        SetIndicator(number);

    }

    void SetIndicator(int i)
    {
        if(!Indicators.ContainsKey(i))
            Debug.LogErrorFormat("Wrong Indicator Number {0}", i);

        foreach (var indicator in Indicators)
        {
            indicator.Value.sprite = IndicatorSmallImage;
            indicator.Value.SetNativeSize();
        }
        Indicators[i].sprite = IndicatorLargeImage;
        Indicators[i].SetNativeSize();
    }

    public void OnChangeChapter()
    {
        
    }

    void NextChapter()
    {
        
    }

    Vector3? _lastTouchPos;
    Vector3? _curTouchPos;

    enum TouchType
    {
        None,
        Left,
        Right
    }

    public float TouchSlideSpeed = 2f;
    TouchType _touchType = TouchType.None;
    bool _dragged = false;
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            return;
        }

        if (!TouchSlide) return; 
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            if (_lastTouchPos.HasValue == false)
            {
                _lastTouchPos = mousePosition;
                return;
            }
            else
            {

                _curTouchPos = mousePosition;
                if (Vector3.Distance(_curTouchPos.Value, _lastTouchPos.Value) > 0.01f)
                {
                    if (!_dragged)
                    {
                        _dragged = true;
                        OnDragStart();
                    }


                    if (_curTouchPos.Value.x < _lastTouchPos.Value.x)
                    {
                        _touchType = TouchType.Left;
                    }
                    else if (_curTouchPos.Value.x > _lastTouchPos.Value.x)
                    {
                        _touchType = TouchType.Right;
                    }
                    OnDrag();

                }
                else
                {
                    if (_dragged) OnDrag();
                }
            }
        }
        else
        {
            if (_dragged)
            {
                _dragged = false;
                OnDragEnd();
            }
        }
    }

    void OnDragStart()
    {
        _touchType = TouchType.None;
    }

    LevelSelectChapter GetNextChapter()
    {
        if (CurrentChapterNumber >= Chapters.Count-1)
            return null;
        return Chapters[CurrentChapterNumber + 1];
    }

    LevelSelectChapter GetLastChapter()
    {
        if (CurrentChapterNumber <= 0)
            return null;
        return Chapters[CurrentChapterNumber - 1];
    }
    void OnDrag()
    {
        if (_rotateAnim) return;
        if (_touchType == TouchType.Left)
        {
            var speed = TouchSlideSpeed * Vector3.Distance(_curTouchPos.Value, _lastTouchPos.Value);
            
            var next = GetNextChapter();
            if (next != null)
            {
                var cur = Chapters[CurrentChapterNumber];
                var angleCur = Mathf.Lerp(0f, -180f, speed / 180f);
                cur.AddRotateZ(angleCur);
                next.AddRotateZ(angleCur);
            }
        }
        else if(_touchType == TouchType.Right)
        {
            var speed = TouchSlideSpeed * Vector3.Distance(_curTouchPos.Value, _lastTouchPos.Value);
            
            var last = GetLastChapter();
            if (last != null)
            {
                var cur = Chapters[CurrentChapterNumber];
                var angleCur = Mathf.Lerp(0, 180f, speed / 180f);
                cur.AddRotateZ(angleCur);
                last.AddRotateZ(angleCur);
            }
        }
    }

    bool _rotateAnim = false;

    void OnDragEnd()
    {
        if (_rotateAnim) return;
        if (Mathf.Abs(Chapters[CurrentChapterNumber].transform.localEulerAngles.z % 180f) > 60f)
        {
            var curT = Chapters[CurrentChapterNumber];
            
            if (_touchType == TouchType.Left)
            {
                _rotateAnim = true;
                if (CurrentChapterNumber >= Chapters.Count - 1) return;
                var nextT = Chapters[CurrentChapterNumber + 1];
                curT.DoRotateClockWise(2f, OnRotateAnimComplete);
                nextT.DoRotateClockWise(2f, OnRotateAnimComplete);
                CurrentChapterNumber++;
                //curT.DORotate(new Vector3(0, 0, 180f), 2f, RotateMode.LocalAxisAdd);
                //nextT.DORotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.LocalAxisAdd).OnComplete(OnRotateAnimComplete);
            }
            else if (_touchType == TouchType.Right)
            {
                _rotateAnim = true;
                if (CurrentChapterNumber < 0) return;
                var nextT = Chapters[CurrentChapterNumber - 1];
                curT.DoRotateCounterClockWise(2f, OnRotateAnimComplete);
                nextT.DoRotateCounterClockWise(2f, OnRotateAnimComplete);
                CurrentChapterNumber--;
            }
        }
        else
        {
            var curT = Chapters[CurrentChapterNumber];

            if (_touchType == TouchType.Left)
            {
                _rotateAnim = true;
                if (CurrentChapterNumber >= Chapters.Count - 1) return;
                var nextT = Chapters[CurrentChapterNumber + 1];
                //curT.DoRotateOrigin(2f, OnRotateAnimComplete);
               // nextT.DoRotateOrigin(2f, OnRotateAnimComplete);
                //curT.DORotate(new Vector3(0, 0, 180f), 2f, RotateMode.LocalAxisAdd);
                //nextT.DORotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.LocalAxisAdd).OnComplete(OnRotateAnimComplete);
            }
            else if (_touchType == TouchType.Right)
            {
                _rotateAnim = true;
                if (CurrentChapterNumber < 0) return;
                var nextT = Chapters[CurrentChapterNumber - 1];
               // curT.DoRotateOrigin(2f, OnRotateAnimComplete);
               // nextT.DoRotateOrigin(2f, OnRotateAnimComplete);
            }
        }
    }

    void OnRotateAnimComplete()
    {
        _rotateAnim = false;
        SetIndicator(CurrentChapterNumber);
        //SetCurrentChapterWithoutAnim(CurrentChapterNumber);
    }

    public void OnBtnNext()
    {
        if (CurrentChapterNumber >= Chapters.Count - 1)
        {
            return;
        }
        Chapters[CurrentChapterNumber++].SetActive(false);
        Chapters[CurrentChapterNumber].SetActive(true);
        SetIndicator(CurrentChapterNumber);
        UpdateChapterInfo();
        SetLevelInfo();
    }

    public void OnBtnBack()
    {
        if (CurrentChapterNumber <= 0)
        {
            return;
        }
        Chapters[CurrentChapterNumber--].SetActive(false);
        Chapters[CurrentChapterNumber].SetActive(true);
        SetIndicator(CurrentChapterNumber);
        UpdateChapterInfo();
        SetLevelInfo();
    }

    void UpdateChapterInfo()
    {
        Chapters[CurrentChapterNumber].SetChapterName();
    }

    void SetLevelInfo()
    {
        Chapters[CurrentChapterNumber].UpdateLevelInfo();
    }
    
}
