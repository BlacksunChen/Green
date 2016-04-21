using UnityEngine;
using System.Collections;

public class exit : MonoBehaviour {

	game_uGUI my_game_uGUI;

	// Use this for initialization
	void Start () {
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player"))
		{
			if (!game_uGUI.stage_end)
				{
				if (my_game_uGUI.star_number>0)
					my_game_uGUI.Victory();
				else
					my_game_uGUI.Defeat();
				}
		}
	}
}
