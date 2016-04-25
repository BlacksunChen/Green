using UnityEngine;
using System.Collections;

public class VerticalDemoClicker : MonoBehaviour {

	void Awake() {
		LevelMenu2D.Instance.OnItemClicked += HandleOnItemClicked;
		SwipeDetector.OnSwipeDown += HandleOnSwipeDown;
		SwipeDetector.OnSwipeUp += HandleOnSwipeUp;
	}

	void HandleOnSwipeUp ()
	{
		LevelMenu2D.Instance.gotoBackItem();
	}

	void HandleOnSwipeDown ()
	{
		LevelMenu2D.Instance.gotoNextItem();
	}



	void HandleOnItemClicked (int itemIndex, GameObject itemObject)
	{
		//if (SwipeDetector.isSwiping) return;
		//Debug.Log ("Clicked");
		iTween.FadeTo(itemObject, 0f, 0.5f);
		iTween.FadeTo(itemObject, iTween.Hash("alpha", 1f, "delay", 0.5f, "time", 0.5f));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
