using UnityEngine;
using System.Collections;

public class manual_world_screen : MonoBehaviour {

	public world_ico_uGUI[] world_icons;
	manage_menu_uGUI my_manage_menu_uGUI;
	game_master my_game_master;

	// Use this for initialization
	void Start () {
	
	}

	public void My_start()
		{
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

		my_manage_menu_uGUI = GameObject.Find("Home_Canvas").GetComponent<manage_menu_uGUI>();

		if (my_game_master.total_stages_in_world_n.Length != world_icons.Length)
			Debug.LogError("the length of game_master._total_stages_in_world_n and manual_world_screen.world_icons MUST be the same");
		
		for (int i = 0; i < world_icons.Length; i++)
			{
			world_icons[i].my_number = i;
			world_icons[i].my_manage_menu_uGUI = my_manage_menu_uGUI;
			world_icons[i].my_game_master = my_game_master;
			world_icons[i].My_start();
			}
		}



	public void Focus_on_first_icon()
	{
			my_manage_menu_uGUI.world_screen_target_button = world_icons[0].gameObject;
			if (this.gameObject.activeSelf)
				my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.world_screen_target_button);

	}

	public void Focus_on_current_world_icon()
	{
		my_manage_menu_uGUI.Mark_this_button(world_icons[my_game_master.current_world[my_game_master.current_profile_selected]].gameObject);
	}
	

}
