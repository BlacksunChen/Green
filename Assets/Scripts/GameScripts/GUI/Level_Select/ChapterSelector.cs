using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ChapterSelector : MonoBehaviour
{
    public int CurrentChapterNumber;
    public Dictionary<int, LevelSelectChapter> Chapters = new Dictionary<int, LevelSelectChapter>();
    public Sprite IndicatorLargeImage;
    public Sprite IndicatorSmallImage;
    public Dictionary<int, Image> Indicators;
    void Awake()
    {
        
    }
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

        SetCurrentChapterWithoutAnim(0);
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
                c.Value.GetComponent<RectTransform>().Rotate(0f, 0f, -180f);
            }
        }
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


                    if (_curTouchPos.Value.x < _lastTouchPos.Value.x && _curTouchPos.Value.y < _lastTouchPos.Value.y)
                    {
                        _touchType = TouchType.Left;
                    }
                    else if (_curTouchPos.Value.x > _lastTouchPos.Value.x && _curTouchPos.Value.y > _lastTouchPos.Value.y)
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
            _dragged = false;
            OnDragEnd();
        }
    }

    void OnDragStart()
    {
        
    }

    RectTransform GetNextChapter()
    {
        if (CurrentChapterNumber == Chapters.Count-1) return null;
        return Chapters[CurrentChapterNumber + 1].GetComponent<RectTransform>();
    }

    RectTransform GetLastChapter()
    {
        if (CurrentChapterNumber == 0) return null;
        return Chapters[CurrentChapterNumber - 1].GetComponent<RectTransform>();
    }
    void OnDrag()
    {
        if (_touchType == TouchType.Left)
        {
            var speed = TouchSlideSpeed * Vector3.Distance(_curTouchPos.Value, _lastTouchPos.Value);
            var angleCur = Mathf.Clamp(speed, 0, -180f);
            var next = GetNextChapter();
            if (next != null)
            {
                var cur = Chapters[CurrentChapterNumber].transform;
                cur.Rotate(0f, 0, cur.localEulerAngles.z + angleCur);
                var nex = next.transform;
                nex.Rotate(0f, 0, cur.localEulerAngles.z + angleCur);
            }
        }
        else if(_touchType == TouchType.Right)
        {
            var speed = TouchSlideSpeed * Vector3.Distance(_curTouchPos.Value, _lastTouchPos.Value);
            var angleCur = Mathf.Clamp(speed, 0, 180f);
            var last = GetLastChapter();
            if (last != null)
            {
                var cur = Chapters[CurrentChapterNumber].transform;
                cur.Rotate(0f, 0, cur.localEulerAngles.z + angleCur);
                var nex = last.transform;
                nex.Rotate(0f, 0, cur.localEulerAngles.z + angleCur);
            }
        }
    }

    void OnDragEnd()
    {
       // if(Chapters[CurrentChapterNumber].transform.localEulerAngles.z > )
    }
}
