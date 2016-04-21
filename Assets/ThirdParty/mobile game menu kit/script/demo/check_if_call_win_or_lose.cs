using UnityEngine;
using System.Collections;

public class check_if_call_win_or_lose : MonoBehaviour {

	game_uGUI my_game_uGUI;

	void Start()
	{
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}

	void OnMouseDown ()
	{
		if (!game_uGUI.in_pause)
			{
			if (my_game_uGUI.star_number > 0)
				my_game_uGUI.Victory();
			else
				my_game_uGUI.Defeat();
			}
	}
}
