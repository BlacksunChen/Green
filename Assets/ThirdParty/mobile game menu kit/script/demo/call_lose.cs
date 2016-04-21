using UnityEngine;
using System.Collections;

public class call_lose : MonoBehaviour {

	game_uGUI my_game_uGUI;

	void Start()
	{
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}

	void OnMouseDown ()
	{
		if (!game_uGUI.in_pause)
			{
			my_game_uGUI.Defeat();
			}
	}
}
