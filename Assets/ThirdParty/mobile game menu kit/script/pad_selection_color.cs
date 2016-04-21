using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pad_selection_color : MonoBehaviour {

	Button my_button;
	game_master my_game_master;

	// Use this for initialization
	void Start () {

		if (game_master.game_master_obj)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			if (my_game_master.use_pad)
				{
				my_button = this.gameObject.GetComponent<Button>();

				var target = my_button.colors;
				target.normalColor = my_game_master.normal_button_color;
				target.highlightedColor = my_game_master.highlighted_button_color;
				my_button.colors = target;
				}
			}
	}
	

}
