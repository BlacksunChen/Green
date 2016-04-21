using UnityEngine;
using System.Collections;

public class take_money : MonoBehaviour {

	game_uGUI my_game_uGUI;
	int my_value = 1;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(true);
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player"))//if the player collide with this star
			{
			my_game_uGUI.Update_virtual_money(my_value);
			
			this.gameObject.SetActive(false);
			}
	}
}
