using UnityEngine;
using System.Collections;

public class manual_stage_screen : MonoBehaviour {

	public int my_world_number;
	public stage_ico_uGUI[] stage_icons;
	manage_menu_uGUI my_manage_menu_uGUI;
	game_master my_game_master;

	// Use this for initialization
	void Start () {
	
		//My_start();
	}

	public void My_start()
		{
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

		if (my_world_number < my_game_master.total_stages_in_world_n.Length)
		{
			if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.manual)
				{
				my_manage_menu_uGUI = GameObject.Find("Home_Canvas").GetComponent<manage_menu_uGUI>();
				
				if (my_game_master.total_stages_in_world_n[my_world_number] != stage_icons.Length)
					Debug.LogError("the number of icons don't match the number of stages in this world ");
				
				for (int i = 0; i < stage_icons.Length; i++)
					{
					stage_icons[i].world_number = my_world_number;
					stage_icons[i].stage_number = i+1;
					stage_icons[i].my_manage_menu_uGUI = my_manage_menu_uGUI;
					stage_icons[i].my_game_master = my_game_master;
					if ((i+1) <stage_icons.Length)
						stage_icons[i].next_stage_ico = stage_icons[i+1];
					stage_icons[i].My_start();
					}

				Focus_on_first_icon();

					
				}
			}
		}

	void OnEnable()
	{
		if (stage_icons[0].gameObject != null)
		{
			if (my_game_master)
				Focus_on_first_icon();
		}
	}

	void Focus_on_first_icon()
	{

	if (my_world_number == my_game_master.current_world[my_game_master.current_profile_selected])
		{
		my_manage_menu_uGUI.stage_screen_target_button = stage_icons[0].gameObject;
		if (this.gameObject.activeSelf)
			my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.stage_screen_target_button);
		}
	}

}
