using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectChapter : MonoBehaviour
{
    public string ChapterName;
    public Text ChapterNameText;
    // Use this for initialization
    void Start()
    {
        ChapterNameText = GameObject.Find("ChapterName").GetComponent<Text>();
        if (!ChapterNameText)
        {
            Debug.LogError("Missing UI: ChapterName");
        }
        ChapterNameText.text = ChapterName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
