using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HorizontalDemoClicker : MonoBehaviour
{

    manage_menu_uGUI gui = null;
	void Awake() {
		LevelMenu2D.Instance.OnItemClicked += HandleOnItemClicked;
		SwipeDetector.OnSwipeLeft += HandleOnSwipeDown;
		SwipeDetector.OnSwipeRight += HandleOnSwipeUp;
	    gui = GameObject.FindObjectOfType<manage_menu_uGUI>();
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
	    if (itemObject.name == "level_1")
	    {
	        gui.loading_screen.gameObject.SetActive(true);
	        SceneManager.LoadScene("Level_1");
	    }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
