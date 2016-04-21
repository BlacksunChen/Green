using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class new_profile_pad : MonoBehaviour {

	string my_name;
	public Text target_text;
	public int character_limit;
	public GameObject select_this_button_when_close_me;

	public manage_menu_uGUI my_manage_menu_uGUI;
	game_master my_game_master;
	public profile_manager my_profile_manager;

	public GameObject only_ok_button;
	public GameObject ok_and_cancel_button;
	int profile_slot;

	// Use this for initialization
	public void My_start (int profile_target_slot, bool show_cancel_button, GameObject my_target_button) {
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

		if (show_cancel_button)
		{
			only_ok_button.SetActive(false);
			ok_and_cancel_button.SetActive(true);
		}
		else
		{
			only_ok_button.SetActive(true);
			ok_and_cancel_button.SetActive(false);
		}

		profile_slot = profile_target_slot;
		this.gameObject.SetActive(true);
		select_this_button_when_close_me = my_target_button;

	}
	

	public void Add_to_string(string add_this)
	{
		if (target_text.text.Length <= character_limit)
			{
			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			my_name += add_this;
			target_text.text = my_name;
			}
		else
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
	}

	public void Delete_last_character()
	{
		if (target_text.text.Length > 0)
			{
			my_name = my_name.Remove(my_name.Length-1);
			target_text.text = my_name;
			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			}
		else
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
	}

	public void OK_button()
	{
		if (target_text.text != "")
			{
			int old_profile = my_game_master.current_profile_selected;
			my_game_master.current_profile_selected = profile_slot;
			if (my_manage_menu_uGUI.current_screen == my_manage_menu_uGUI.profile_screen)//update profile screen
			{
				my_profile_manager.Select_this_profile(old_profile);
				my_profile_manager.Select_this_profile(profile_slot);
			}


			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			my_game_master.Create_new_profile(my_name);
			my_name = "";
			target_text.text = my_name;
			my_manage_menu_uGUI.Update_profile_name(true);



			Return_to_previous_screen();

			my_profile_manager.Update_this_slot(my_game_master.current_profile_selected);
			}
		else
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);

	}

	void Return_to_previous_screen()
	{
		this.gameObject.SetActive(false);
		my_manage_menu_uGUI.current_screen.gameObject.SetActive(true);

		if (my_manage_menu_uGUI.current_screen == my_manage_menu_uGUI.home_screen)
			my_manage_menu_uGUI.Mark_this_button(select_this_button_when_close_me);
		else if (my_manage_menu_uGUI.current_screen == my_manage_menu_uGUI.profile_screen)
			my_manage_menu_uGUI.Mark_this_button(select_this_button_when_close_me);
	}

	public void Cancel()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);

		Return_to_previous_screen();

		this.gameObject.SetActive(false);
	}
}
