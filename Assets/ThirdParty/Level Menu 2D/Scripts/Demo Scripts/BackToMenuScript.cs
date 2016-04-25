using UnityEngine;
using System.Collections;

public class BackToMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    gui = GameObject.FindObjectOfType<manage_menu_uGUI>();
	}

    manage_menu_uGUI gui = null;
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp()
	{
	    gui.Back();
	}

}
