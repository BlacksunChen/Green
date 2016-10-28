using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Green;
using UnityEngine.UI;

public class LevelSelectChapter : MonoBehaviour
{
    public string ChapterName;
    public int ChapterNumber;
    public Text ChapterNameText;

    public GameObject BgToRotate;
    public GameObject ChapterToRotate;

    public enum RotateDirection
    {
        ClockWise,
        CounterClockWise
    }
    // Use this for initialization
    void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        foreach (var t in texts)
        {
            if (t.name == "ChapterName")
            {
                ChapterNameText = t;
            }
        }
        //ChapterNameText = GameObject.Find("ChapterName").GetComponent<Text>();
        if (!ChapterNameText)
        {
            Debug.LogError("Missing UI: ChapterName");
        }
        BgToRotate = GameObject.Find("Level_Select_Canvas/ChaptersBG/" + ChapterNumber.ToString());
        ChapterToRotate = this.gameObject;
        //ChapterNameText.text = ChapterName;
    }

    public void SetChapterName()
    {
        ChapterNameText.text = "第" + ChapterNumber.ToString() + "章  " + ChapterName;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator RotateAnim(Transform obj, Quaternion to, float duration, Action onComplete, RotateDirection dir)
    {
        float dt = 0f;
        var from = obj.rotation;
      //  var cosOmega = from.
        while (dt <= duration)
        {
            var q = Quaternion.Slerp(from, to, dt / duration);
            if (dir == RotateDirection.CounterClockWise) Quaternion.Inverse(q);
            obj.rotation = q;
            dt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        onComplete();
    }

    public void Rotate(Quaternion to, float duration, Action onComplete, RotateDirection dir = RotateDirection.ClockWise)
    {
        StartCoroutine(RotateAnim(BgToRotate.transform, to, duration, onComplete, dir));
        StartCoroutine(RotateAnim(ChapterToRotate.transform, to, duration, onComplete, dir));
    }

    
    public void DoRotateClockWise(float duration, Action onComplete)
    {
        float bgRotate = 0f;

        if (BgToRotate.transform.localEulerAngles.z > 180f)
        {
            //上面
            bgRotate = 180f;
        }
        else
        {
            //原地
            bgRotate = 0f;
        }

        Debug.LogFormat("{0} DoRotateClockWise BG: {1}", name, bgRotate);
        BgToRotate.transform.DORotate(new Vector3(0f, 0f, bgRotate), duration, RotateMode.FastBeyond360)
            .OnComplete(() => onComplete());
        float chRotate = 0f;
        if (ChapterToRotate.transform.localEulerAngles.z > 180f)
        {
            chRotate = 180f;
        }
        else
        {
            chRotate = 0f;
        }
        Debug.LogFormat("{0} DoRotateClockWise Chapter: {1}", name, chRotate);
        ChapterToRotate.transform.DORotate(new Vector3(0f, 0f, chRotate), duration, RotateMode.FastBeyond360)
            .OnComplete(() => onComplete());
    }


    public void DoRotateCounterClockWise(float duration, Action onComplete)
    {
        var bg = BgToRotate.transform.localEulerAngles.z - BgToRotate.transform.localEulerAngles.z % 180; // + 180f;
        Debug.LogFormat("{0} DoRotateClockWise BG: {1}", name, bg);
        BgToRotate.transform.DOLocalRotate(new Vector3(0f, 0f, bg), duration, RotateMode.LocalAxisAdd);

        var ch = ChapterToRotate.transform.localEulerAngles.z - ChapterToRotate.transform.localEulerAngles.z % 180f;
            // + 180f;
        Debug.LogFormat("{0} DoRotateClockWise Chapter: {1}", name, ch);
        ChapterToRotate.transform.DOLocalRotate(new Vector3(0f, 0f, ch), duration, RotateMode.LocalAxisAdd)
            .OnComplete(() => onComplete());
    }

    public void DoRotateTo(float zValue, float duration, Action onComplete)
    {
        BgToRotate.transform.DOLocalRotate(
            new Vector3(0f, 0f,
                BgToRotate.transform.localEulerAngles.z + Mathf.Abs(BgToRotate.transform.localEulerAngles.z % 180)),
            duration, RotateMode.FastBeyond360);
        ChapterToRotate.transform.DOLocalRotate(
            new Vector3(0f, 0f,
                ChapterToRotate.transform.localEulerAngles.z +
                Mathf.Abs(ChapterToRotate.transform.localEulerAngles.z % 180)), duration, RotateMode.FastBeyond360)
            .OnComplete(() => onComplete());
    }

    public void AddRotateZ(float angleZ, RotateDirection dir = RotateDirection.ClockWise)
    {
        /*
        //BgToRotate.transform.localEulerAngles.z + 
        if (angleZ > 0)
        {
            

            //BgToRotate.transform.Rotate(Vector3.forward, angleZ);
            //ChapterToRotate.transform.Rotate(Vector3.forward, angleZ);
        }
        else
        {
            BgToRotate.transform.Rotate(Vector3.back, 180f - angleZ);
            ChapterToRotate.transform.Rotate(Vector3.back, 180f - angleZ);
            //BgToRotate.transform.Rotate(Vector3.forward, angleZ);
            //ChapterToRotate.transform.Rotate(Vector3.forward, angleZ);
        }*/
        Quaternion rotate;
        if (dir == RotateDirection.CounterClockWise)
        {
            rotate = Quaternion.AngleAxis(angleZ, Vector3.back);
        }
        else
        {
            rotate = Quaternion.AngleAxis(angleZ, Vector3.forward);
        }
        
        //if (dir == RotateDirection.CounterClockWise) Quaternion.Inverse(rotate); 
        BgToRotate.transform.rotation = rotate * BgToRotate.transform.rotation;
        ChapterToRotate.transform.rotation = rotate * ChapterToRotate.transform.rotation;
    }

    public void SetRotateToUp()
    {
        BgToRotate.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
        ChapterToRotate.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
    }

    public void SetRotateToDown()
    {
        BgToRotate.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        ChapterToRotate.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void SetActive(bool active)
    {
        BgToRotate.gameObject.SetActive(active);
        ChapterToRotate.gameObject.SetActive(active);
    }

    public void UpdateLevelInfo()
    {
        GetComponent<LevelSelectMenu>().itemsList[0].GenerateLevelNumber();
    }
}