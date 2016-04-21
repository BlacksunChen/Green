using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class manage_linear_map : MonoBehaviour {

	public bool center_to_next_stage_to_play;
	public ScrollRect scroll_map;
	public RectTransform map_container;
	public stage_ico_uGUI[] stage_icons;
	int current_ico_selected;
	manage_menu_uGUI my_manage_menu_uGUI;
	game_master my_game_master;
	
	// Use this for initialization
	void Start () {
		//My_start();

	}

	public void My_start()
	{

		if (stage_icons.Length > 0)
			{
			if (game_master.game_master_obj)
				my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
		
			my_manage_menu_uGUI = GameObject.Find("Home_Canvas").GetComponent<manage_menu_uGUI>();
		
			if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)//if the map is in use
				{
				if (my_game_master.total_number_of_stages_in_the_game != stage_icons.Length)
					{
						Debug.LogError("the number of icons don't match the total number of stages in the game");
					}
				else
					{
					int temp_current_world = 0;
					int temp_stage_conut = 1;
					for (int i = 0; i < stage_icons.Length; i++)
						{
						//check if this stage icon belong to this world of if you must pass to next world
						if (temp_stage_conut  <= my_game_master.total_stages_in_world_n[temp_current_world])
							{
							//Debug.Log("same world " + temp_current_world +","+temp_stage_conut);
							Update_this_icon(i,temp_current_world,temp_stage_conut);
							temp_stage_conut++;
							}
						else//next world
							{
							temp_current_world++;
							temp_stage_conut = 1;
							//Debug.Log("new world " + temp_current_world +","+temp_stage_conut);
							Update_this_icon(i,temp_current_world,temp_stage_conut);
							temp_stage_conut++;
							}
						}
					
					}
				}
			my_manage_menu_uGUI.stage_screen_target_button = stage_icons[0].gameObject;
			if (this.gameObject.activeSelf)
				my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.stage_screen_target_button);
			}
	}

	void Update()
	{
		if (my_game_master.use_pad)
		{
			if (Input.GetKeyDown(my_game_master.pad_next_button))
				Next();
			else if (Input.GetKeyDown(my_game_master.pad_previous_button))
				Previous();
		}
	}

	void Next()
	{
		if (current_ico_selected+1 < stage_icons.Length)
			{
			current_ico_selected++;
			StartCoroutine(Center_the_map_to_this_stage(current_ico_selected));
			}

	}

	void Previous()
	{
		if (current_ico_selected-1 >= 0)
			{
			current_ico_selected--;
			StartCoroutine(Center_the_map_to_this_stage(current_ico_selected));
			}
	}

	void Update_this_icon(int icon, int world, int stage)
		{
			//Debug.Log("map Update_this_icon: " + icon + "," + world + "," + stage);

			stage_icons[icon].number_to_show = icon+1;
			stage_icons[icon].world_number = world;
			stage_icons[icon].stage_number = stage;
			stage_icons[icon].my_manage_menu_uGUI = my_manage_menu_uGUI;
			stage_icons[icon].my_game_master = my_game_master;

			if ((icon+1) <stage_icons.Length)
				stage_icons[icon].next_stage_ico = stage_icons[icon+1];

			stage_icons[icon].My_start();

			if (center_to_next_stage_to_play && this.gameObject.activeInHierarchy)
				{
				if (my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected] == world
				    && my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected] == stage-1)
					StartCoroutine(Center_the_map_to_this_stage(icon));
				}
		}

	public void Center_to_this_icon(int this_world, int this_stage)
	{

		if (my_game_master.show_debug_messages)
			Debug.Log("this_world: " + this_world + "*** this_stage: " + this_stage);
		current_ico_selected = this_stage;
		for (int i = 0; i < stage_icons.Length; i++)
		{
			if ((stage_icons[i].world_number  == this_world+1) && (stage_icons[i].stage_number == this_stage+1))
				{
				StartCoroutine(Center_the_map_to_this_stage(i));
				return;
				}
		}
	}

	IEnumerator Center_the_map_to_this_stage(int icon)
		{
		yield return new WaitForSeconds(0.01f);

		float normalized_x = Mathf.Clamp01((float)stage_icons[icon].GetComponent<RectTransform>().anchoredPosition.x / (float)map_container.rect.width) ;
		float normalized_y = Mathf.Clamp01((float)stage_icons[icon].GetComponent<RectTransform>().anchoredPosition.y / (float)map_container.rect.height) ;

		scroll_map.normalizedPosition = new Vector2(normalized_x,normalized_y);
		}




	

}
