using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class single_screen : MonoBehaviour {

	public manage_menu_uGUI my_manage_menu_uGUI;

	public Text current_page_name;
	public Image my_bk;
	public Sprite[] world_bk;

	public GameObject next_button;
	public GameObject previous_button;

	public stage_ico_uGUI[] stage_icons;

	game_master my_game_master;

	// Use this for initialization
	void Start () {

		//My_start();
	}

	void Update()
	{
		if (my_game_master.use_pad)
			{
			if (Input.GetKeyDown(my_game_master.pad_next_button) && next_button.activeSelf)
				Next();
			else if (Input.GetKeyDown(my_game_master.pad_previous_button) && previous_button.activeSelf)
				Previous();
			}
	}

	public void My_start() {

		if (stage_icons.Length > 0)
		{
			if (game_master.game_master_obj)
			{
				my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
				my_game_master.current_world[my_game_master.current_profile_selected] = my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected];
				Update_page();
			}

		}
	}
	

	void Update_page() {

		int current_page = my_game_master.current_world[my_game_master.current_profile_selected];

		//set page name and background
		current_page_name.text = my_game_master.world_name[current_page];
		//Debug.Log("current_page " + current_page);
		if (current_page < world_bk.Length)
			my_bk.sprite = world_bk[current_page];

		//show_arrows
			if (current_page  == 0)
				previous_button.SetActive(false);
			else
				previous_button.SetActive(true);

			if (current_page  == my_game_master.total_stages_in_world_n.Length-1)
				next_button.SetActive(false);
			else
				next_button.SetActive(true);

		//update icons
		for (int i = 0; i < stage_icons.Length; i++)
			{
			if (i < my_game_master.total_stages_in_world_n[my_game_master.current_world[my_game_master.current_profile_selected]])
				{
				stage_icons[i].gameObject.SetActive(true);
				stage_icons[i].world_number = current_page;
				stage_icons[i].stage_number =i+1;
				stage_icons[i].my_manage_menu_uGUI = my_manage_menu_uGUI;
				stage_icons[i].my_game_master = my_game_master;
				stage_icons[i].My_start();
				}
			else 
				stage_icons[i].gameObject.SetActive(false);
			}

		my_manage_menu_uGUI.stage_screen_target_button = stage_icons[0].gameObject;
		if (this.gameObject.activeSelf)
			my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.stage_screen_target_button);
	}

	public void Next()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_game_master.current_world[my_game_master.current_profile_selected]++;
		Update_page();
	}

	public void Previous()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_game_master.current_world[my_game_master.current_profile_selected]--;
		Update_page();
	}
}
