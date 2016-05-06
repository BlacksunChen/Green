using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectChapter : MonoBehaviour
{
    public string ChapterName;
    public int ChapterNumber;
    public Text ChapterNameText;
    // Use this for initialization
    void Start()
    {
        ChapterNameText = GameObject.Find("ChapterName").GetComponent<Text>();
        if (!ChapterNameText)
        {
            Debug.LogError("Missing UI: ChapterName");
        }
        //ChapterNameText.text = ChapterName;
    }

    string GetChapterName()
    {
        return "第" + ChapterNumber.ToString() + "章  " + ChapterName;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
