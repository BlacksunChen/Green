using UnityEngine;
using System.Collections;
using Utilities;

public class AutoInitScript : Singleton<AutoInitScript> {



	public int indexToLoad = 0;
	public string demoType = "hori_icon";

	protected override void Awake()
	{
	    base.Awake();
	    //Debug.LogWarning("Current: " + indexToLoad);
	    //LevelMenu2D.Instance.gotoItem(indexToLoad);
	}

	// Use this for initialization
	void Start () {
		//Debug.LogWarning("Current: " + indexToLoad);
		//LevelMenu2D.Instance.gotoItem(indexToLoad);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
