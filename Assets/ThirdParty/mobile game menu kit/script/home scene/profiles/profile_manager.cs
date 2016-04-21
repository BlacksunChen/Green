using UnityEngine;
using System.Collections;

public class profile_manager : MonoBehaviour {

	public manage_menu_uGUI my_manage_menu_uGUI;
	profile_button[] profiles_array;
	int total_buttons;
	game_master my_game_master;
	public GameObject ask_confirmation_window_prefab;

	//these are for pad controls
	public GameObject ask_confirmation_target_button;
	public GameObject target_delete_button;
	public GameObject target_back_button;

	// Use this for initialization
	void Start () {
	
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

		//search all buttons that you must manage
		total_buttons = this.transform.childCount;
		profiles_array = new profile_button[total_buttons];
		for (int i = 0; i < total_buttons; i++)
			{
			Transform temp_childeren = this.transform.GetChild(i);
			profile_manager this_script = this.GetComponent<profile_manager>();

			if (i < my_game_master.number_of_save_profile_slot_avaibles)
				{
				temp_childeren.gameObject.SetActive(true);
				profiles_array[i] = temp_childeren.GetComponent<profile_button>();
				profiles_array[i].my_profile_manager = this_script;
				profiles_array[i].my_game_master = my_game_master;
				profiles_array[i].Start_me(i);


				if (my_game_master.this_profile_have_a_save_state_in_it[i])
					{
					my_game_master.Load(i);

					Update_this_slot(i);
					}
				else
					profiles_array[i].Set_off();
				}
			else
				temp_childeren.gameObject.SetActive(false);
			}
	}


	public void Create_new_profile(int profile_slot,bool show_cancel_button)
	{
		//copy options settings
		game_master.music_on[profile_slot] = game_master.music_on[my_game_master.current_profile_selected];
			game_master.music_volume[profile_slot] = game_master.music_volume[my_game_master.current_profile_selected];
		game_master.sfx_on[profile_slot] = game_master.sfx_on[my_game_master.current_profile_selected];
			game_master.sfx_volume[profile_slot] = game_master.sfx_volume[my_game_master.current_profile_selected];
		game_master.voice_on[profile_slot] = game_master.voice_on[my_game_master.current_profile_selected];
			game_master.voice_volume[profile_slot] = game_master.voice_volume[my_game_master.current_profile_selected];
		my_game_master.Save(profile_slot);

		//my_game_master.current_profile_selected = profile_slot;
		Show_current_slot_selected();

		if (my_game_master.require_a_name_for_profiles)
			{
			if (my_game_master.use_pad)
				{
				my_manage_menu_uGUI.current_screen.gameObject.SetActive(false);
				my_manage_menu_uGUI.my_new_profile_pad.My_start(profile_slot,show_cancel_button,target_back_button);

				my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.new_profile_window_pad_target_button);
				}
			else
				{
				my_manage_menu_uGUI.my_new_profile_window.GetComponent<new_profile_window>().My_start(profile_slot,show_cancel_button);
				//my_manage_menu_uGUI.my_new_profile_window.SetActive(true);
				//my_manage_menu_uGUI.my_new_profile_window.GetComponent<new_profile_window>().Focus();
				}
			}
		else
			{
			my_game_master.Create_new_profile("");
			Update_this_slot(profile_slot);
			}

	}

	public void Update_this_slot(int slot_n)
	{
		if (my_game_master != null)
			{
			/*
			Debug.Log("my_game_master.current_profile_selected: " + my_game_master.current_profile_selected);
			Debug.Log("slot_n: " + slot_n);
			Debug.Log("profiles_array " + profiles_array.Length);
			Debug.Log("profiles_array[slot_n] = " + profiles_array[slot_n]);*/

			int temp_lives;
			if (my_game_master.infinite_lives)
				temp_lives = 1;
			else
				temp_lives = my_game_master.current_lives[slot_n];

			if (my_game_master.current_profile_selected == slot_n)
				profiles_array[slot_n].Set_on(true,
				                              my_game_master.profile_name[slot_n],
				                              "World  " + my_game_master.play_this_stage_to_progress_in_the_game_world[slot_n].ToString() + "  Stage  " + my_game_master.play_this_stage_to_progress_in_the_game_stage[slot_n].ToString(),
				                              temp_lives,
				                              my_game_master.stars_total_score[slot_n]);
			else
				profiles_array[slot_n].Set_on(false,
				                              my_game_master.profile_name[slot_n],
				                              "World  " + my_game_master.play_this_stage_to_progress_in_the_game_world[slot_n].ToString() + "  Stage  " + my_game_master.play_this_stage_to_progress_in_the_game_stage[slot_n].ToString(),
				                              temp_lives,
				                              my_game_master.stars_total_score[slot_n]);
			}
	}

	public void Select_this_profile(int profile_slot)
		{
		if (profile_slot != my_game_master.current_profile_selected)
			{
			if (!my_game_master.infinite_lives)
				my_game_master.Check_if_interrupt_countdown();

			my_game_master.current_profile_selected = profile_slot;
			PlayerPrefs.SetInt("last_profile_used",my_game_master.current_profile_selected);
			my_manage_menu_uGUI.update_world_and_stage_screen = true;
			my_game_master.Load(my_game_master.current_profile_selected);
			Show_current_slot_selected();
			}
		}

	public void Show_current_slot_selected()
	{
		for (int i = 0; i < my_game_master.number_of_save_profile_slot_avaibles; i++)
		{
			if (i == my_game_master.current_profile_selected)
				profiles_array[i].Show_selection(true);
			else
				profiles_array[i].Show_selection(false);
		}
	}



	public void Delete_current_profile_selected()
	{
		Close_ask_confirmation();
		my_game_master.Delete_this_profile(my_game_master.current_profile_selected);
		profiles_array[my_game_master.current_profile_selected].Set_off();

		//search if it is left a profile
			int temp_profile_slot = -1;
			for (int i = 0; i < my_game_master.number_of_save_profile_slot_avaibles; i++)
				{
				if (my_game_master.this_profile_have_a_save_state_in_it[i] && temp_profile_slot == -1)
					{
					temp_profile_slot = i;
					break;
					}
				}
			if (temp_profile_slot >= 0) //if there is a profile
				{
				Select_this_profile(temp_profile_slot);
				}
			else //no save data
				{
				//so you MUST create a profile
				Create_new_profile(0,false);
				}
	}

	#region aks confirmation window
	public void Ask_confirmation_to_delete_this_profile()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		ask_confirmation_window_prefab.SetActive(true);
		my_manage_menu_uGUI.Mark_this_button(ask_confirmation_target_button);
	}

	public void Close_ask_confirmation()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		ask_confirmation_window_prefab.SetActive(false);
		my_manage_menu_uGUI.Mark_this_button(target_delete_button);
	}
	#endregion

}
