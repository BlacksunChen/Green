using UnityEngine;
using System.Collections;

public class get_score_points : MonoBehaviour {

	public int points;
	game_uGUI my_game_uGUI;

	void Start()
	{
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}
	
	void OnMouseDown ()
	{
		if (!game_uGUI.in_pause)
		{
			my_game_uGUI.Update_int_score(points);
		}
	}
}
