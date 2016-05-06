using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterSelector : MonoBehaviour
{
    public int CurrentChapterNumber;
    public List<LevelSelectChapter> Chapters = new List<LevelSelectChapter>();

    void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        Chapters = GetComponentsInChildren<LevelSelectChapter>().ToList();
        CurrentChapterNumber = 0;

    }

    void SetCurrentChapterWithoutAnim()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
