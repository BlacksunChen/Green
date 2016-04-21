using UnityEngine;
using System.Collections;

public class call_win : MonoBehaviour {

	public int star_score;
	game_uGUI my_game_uGUI;

	void Start()
	{
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}

	void OnMouseDown ()
	{
		if (!game_uGUI.in_pause)
			{
			my_game_uGUI.star_number = star_score;
			my_game_uGUI.Victory();
			}
	}
}
