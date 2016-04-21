using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class world_ico_uGUI : MonoBehaviour {

	public Image my_padlock;
	public int my_number;
	public Text my_text_number;
	public Text my_text_name;
	public Text my_star_need;
	public manage_menu_uGUI my_manage_menu_uGUI;

	public Transform my_stages_screen;
	public game_master my_game_master;

	void Start()
	{

	}

	// Use this for initialization
	public void My_start () {

		//show world name
		if (my_game_master.show_world_name_on_world_ico)
			{
			my_text_name.gameObject.SetActive(true);
			my_text_name.text = my_game_master.world_name[my_number];
			}
		else
			my_text_name.gameObject.SetActive(false);

		//show world number
		if (my_game_master.show_world_number_on_world_ico)
			{
			my_text_number.gameObject.SetActive(true);
			my_text_number.text = (my_number+1).ToString();
			}
		else
			my_text_number.gameObject.SetActive(false);

		My_padlock();

		if (my_stages_screen && (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.manual))
		{
			my_stages_screen.GetComponent<manual_stage_screen>().my_world_number = my_number;
		}
	}

	public void My_padlock()
	{
		if (my_game_master.world_playable[my_game_master.current_profile_selected][my_number])
			{
			my_padlock.enabled = false;
			my_star_need.gameObject.SetActive(false);
			}
		else
			{
			my_padlock.enabled = true;
			if (my_game_master.this_world_is_unlocked_after_selected[my_number] == game_master.this_world_is_unlocked_after.reach_this_star_score)
				{
				if (my_game_master.stars_total_score[my_game_master.current_profile_selected] >= my_game_master.star_score_required_to_unlock_this_world[my_number])
					{
					my_game_master.world_playable[my_game_master.current_profile_selected][my_number] = true;
					my_game_master.stage_playable[my_game_master.current_profile_selected][my_number,0] = true;
					my_game_master.Save(my_game_master.current_profile_selected);
					My_padlock();
					}
				else
					{
					my_star_need.gameObject.SetActive(true);
					my_star_need.text = my_game_master.star_score_required_to_unlock_this_world[my_number].ToString();
					}
				}
			else
				my_star_need.gameObject.SetActive(false);
			}
	}

	public void Click_me()
	{
		if (my_padlock.enabled) //you can't go in this world yet
			{
			if (!GetComponent<Animation>().isPlaying)
				{
				my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
				
				GetComponent<Animation>().Play("tap_icon_error");
				}
			}
		else //open stage screen of this world
			{
			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			my_game_master.current_world[my_game_master.current_profile_selected] = my_number;
			if(my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
				{
				my_manage_menu_uGUI.Go_to_this_world_stage_menu(my_number);
				}
			else if(my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.manual)
				{
				if (my_game_master.show_debug_messages)
					Debug.Log("manual stage creation");
				my_manage_menu_uGUI.worlds_screen_automatic.gameObject.SetActive(false);
				my_manage_menu_uGUI.worlds_screen_manual.gameObject.SetActive(false);
				my_manage_menu_uGUI.Mark_current_screen(my_manage_menu_uGUI.stages_screen_manual[my_number]);
				my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.stage_screen_target_button);
				}
			}
	}
}
