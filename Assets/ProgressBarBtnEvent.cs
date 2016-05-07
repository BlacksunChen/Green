using UnityEngine;
using System.Collections;
using Green;
using UnityEngine.UI;

public class ProgressBarBtnEvent : MonoBehaviour
{

    public Button Button_;
	// Use this for initialization
	void Start ()
	{
	    Button_ = GetComponent<Button>();
	    var touch = GameObject.Find("Gameplay").GetComponent<GameTouch>();
        Button_.onClick.AddListener(touch.OnCloseProgressBar);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
