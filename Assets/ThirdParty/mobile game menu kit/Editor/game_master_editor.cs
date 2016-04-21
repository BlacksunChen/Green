
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(game_master)), CanEditMultipleObjects]
public class game_master_editor : Editor {

	int array_length;
	string[] temp_world_name;
	game_master.this_world_is_unlocked_after[] temp_this_world_is_unlocked_after_selected;

	// Use this for initialization
	void Start () {
	
	}

	public override void OnInspectorGUI () {
		game_master my_target = (game_master)target;
		EditorGUI.BeginChangeCheck ();

		my_target.allow_ESC = EditorGUILayout.Toggle("go BACK when press device button/ESC",my_target.allow_ESC);
		my_target.show_loading_screen = EditorGUILayout.Toggle("show loading screen",my_target.show_loading_screen);
		EditorGUILayout.Space();

		my_target.number_of_save_profile_slot_avaibles = EditorGUILayout.IntSlider("profiles save slots",my_target.number_of_save_profile_slot_avaibles,1,10);
		EditorGUI.indentLevel++;
		if (my_target.number_of_save_profile_slot_avaibles > 1)
			my_target.require_a_name_for_profiles = EditorGUILayout.Toggle("require to insert a name",my_target.require_a_name_for_profiles);
		EditorGUI.indentLevel--;
		
		EditorGUILayout.Space();

		my_target.press_start_and_go_to_selected = (game_master.press_start_and_go_to)EditorGUILayout.EnumPopup("press start and go to",my_target.press_start_and_go_to_selected);
		if (my_target.press_start_and_go_to_selected == game_master.press_start_and_go_to.nested_world_stage_select_screen)
			{
			EditorGUI.indentLevel++;
			my_target.world_screen_generation_selected = (game_master.world_screen_generation)EditorGUILayout.EnumPopup("world screen generation",my_target.world_screen_generation_selected);
				EditorGUI.indentLevel++;
				my_target.show_world_name_on_world_ico = EditorGUILayout.Toggle("show world name on world_ico",my_target.show_world_name_on_world_ico);
				my_target.show_world_number_on_world_ico = EditorGUILayout.Toggle("show world number on world_ico",my_target.show_world_number_on_world_ico);
				EditorGUI.indentLevel--;

			my_target.stage_screen_generation_selected = (game_master.stage_screen_generation)EditorGUILayout.EnumPopup("stage screen generation",my_target.stage_screen_generation_selected);
			EditorGUI.indentLevel--;
			}
		EditorGUILayout.Space();




		//worlds
		my_target.editor_show_worlds = EditorGUILayout.Foldout(my_target.editor_show_worlds, "Worlds");
		if (my_target.editor_show_worlds)
		{
		serializedObject.Update();

		if (serializedObject.FindProperty("total_stages_in_world_n").arraySize < 1)
			GUI.color = Color.red;
		else
			GUI.color = Color.white;
			serializedObject.FindProperty("total_stages_in_world_n").arraySize = EditorGUILayout.IntField("Total worlds",serializedObject.FindProperty("total_stages_in_world_n").arraySize);
			serializedObject.FindProperty("star_score_required_to_unlock_this_world").arraySize = my_target.total_stages_in_world_n.Length;
		GUI.color = Color.white;
		serializedObject.ApplyModifiedProperties();

		
		array_length = my_target.total_stages_in_world_n.Length;
			if (array_length > 0)//if there is something to copy
				{
				if (my_target.world_name.Length != array_length) //uppdate arrays Lengths
					{
					temp_world_name = my_target.world_name;
					my_target.world_name = new string[array_length];

					temp_this_world_is_unlocked_after_selected = my_target.this_world_is_unlocked_after_selected;
					my_target.this_world_is_unlocked_after_selected = new game_master.this_world_is_unlocked_after[array_length];

					for(int i = 0; i < my_target.world_name.Length; i++)
						{
						if (i < temp_world_name.Length)
							{
							my_target.world_name[i] = temp_world_name[i];
							if (i>0)
								my_target.this_world_is_unlocked_after_selected[i] = temp_this_world_is_unlocked_after_selected[i];
							else
								my_target.this_world_is_unlocked_after_selected[i] = game_master.this_world_is_unlocked_after.start;

							}
						else
							{
							my_target.world_name[i] = temp_world_name[temp_world_name.Length-1];
							if (i>0)
								my_target.this_world_is_unlocked_after_selected[i] = temp_this_world_is_unlocked_after_selected[temp_world_name.Length-1];
							else
								my_target.this_world_is_unlocked_after_selected[i] = game_master.this_world_is_unlocked_after.start;

							}
						}
				
				}

			//show arrays in custom editor
				EditorGUI.indentLevel++;
				for(int i = 0; i < array_length; i++)
					{
					EditorGUILayout.LabelField("World " + (i+1).ToString());

					EditorGUI.indentLevel++;

						my_target.world_name[i] = EditorGUILayout.TextField("name",my_target.world_name[i]);

						if (my_target.total_stages_in_world_n[i] < 1)
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						my_target.total_stages_in_world_n[i] = EditorGUILayout.IntField("stages",my_target.total_stages_in_world_n[i] );
						GUI.color = Color.white;

						if(i == 0)
							my_target.this_world_is_unlocked_after_selected[i] = game_master.this_world_is_unlocked_after.start;
						else
							{
							my_target.this_world_is_unlocked_after_selected[i] = (game_master.this_world_is_unlocked_after)EditorGUILayout.EnumPopup("unlock after",my_target.this_world_is_unlocked_after_selected[i]);
							if (my_target.this_world_is_unlocked_after_selected[i] == game_master.this_world_is_unlocked_after.reach_this_star_score)
								{

								EditorGUI.indentLevel++;
								if (my_target.star_score_required_to_unlock_this_world[i] < 1)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								my_target.star_score_required_to_unlock_this_world[i] = EditorGUILayout.IntField("target star score",my_target.star_score_required_to_unlock_this_world[i] );
								GUI.color = Color.white;
								EditorGUI.indentLevel--;
								}
								else if (my_target.this_world_is_unlocked_after_selected[i] == game_master.this_world_is_unlocked_after.buy_it)
						        {
								my_target.store_enabled = true;
								}
							}
						
					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
					}

				EditorGUI.indentLevel--;

			

			}


		}

		//lives
		my_target.editor_show_lives = EditorGUILayout.Foldout(my_target.editor_show_lives, "Lives");
		if (my_target.editor_show_lives)
		{
			EditorGUI.indentLevel++;
			
			my_target.infinite_lives = EditorGUILayout.Toggle("infinite lives",my_target.infinite_lives);
			if (!my_target.infinite_lives)
			{
				EditorGUILayout.BeginHorizontal();
				if (my_target.start_lives < 1)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				my_target.start_lives = EditorGUILayout.IntField("start lives",my_target.start_lives);
				GUI.color = Color.white;
				
				if (my_target.live_cap < 1)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				my_target.live_cap = EditorGUILayout.IntField("live cap",my_target.live_cap);
				GUI.color = Color.white;
				EditorGUILayout.EndHorizontal();

				my_target.lives_name = EditorGUILayout.TextField("lives name",my_target.lives_name);
				
				my_target.lose_lives_selected = (game_master.lose_lives)EditorGUILayout.EnumPopup("lose lives",my_target.lose_lives_selected);
				
				EditorGUILayout.LabelField("if zero lives:");
				EditorGUI.indentLevel++;

				my_target.continue_rule_selected = (game_master.continue_rule)EditorGUILayout.EnumPopup("continue rule",my_target.continue_rule_selected);
				
				if(my_target.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token)
				{
					EditorGUI.indentLevel++;
					EditorGUI.indentLevel++;
					if (my_target.start_continue_tokens < 0)
						GUI.color = Color.red;
					else
						GUI.color = Color.white;
					my_target.start_continue_tokens = EditorGUILayout.IntField("start continue tokens",my_target.start_continue_tokens);
					GUI.color = Color.white;

					if (my_target.continue_tokens_cap < 1)
						GUI.color = Color.red;
					else
						GUI.color = Color.white;
					my_target.continue_tokens_cap = EditorGUILayout.IntField("continue tokens cap",my_target.continue_tokens_cap);
					GUI.color = Color.white;
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
				}


				
				
				if(my_target.continue_rule_selected != game_master.continue_rule.never_continue)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.BeginHorizontal();
					my_target.continue_menu_have_countdown = EditorGUILayout.Toggle("countdown",my_target.continue_menu_have_countdown);
					if(my_target.continue_menu_have_countdown)
					{
						if (my_target.continue_menu_countdown_seconds <= 0)
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						my_target.continue_menu_countdown_seconds = EditorGUILayout.IntField("seconds",my_target.continue_menu_countdown_seconds);
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.LabelField("if player:");
					EditorGUI.indentLevel++;
					my_target.if_player_continue_selected = (game_master.if_player_continue)EditorGUILayout.EnumPopup("continue",my_target.if_player_continue_selected);
					EditorGUI.indentLevel++;
					if (my_target.continue_give_new_lives < 1)
						GUI.color = Color.red;
					else
						GUI.color = Color.white;
					my_target.continue_give_new_lives = EditorGUILayout.IntField("give new lives",my_target.continue_give_new_lives);
					EditorGUI.indentLevel--;
					
					EditorGUILayout.Space();
					
					my_target.if_player_not_continue_selected = (game_master.if_player_not_continue)EditorGUILayout.EnumPopup("not continue",my_target.if_player_not_continue_selected);
					EditorGUI.indentLevel++;
					
					my_target.when_restart_selected = (game_master.when_restart)EditorGUILayout.EnumPopup("when restart",my_target.when_restart_selected);
					
					
					if(my_target.when_restart_selected == game_master.when_restart.give_lives_after_countdown)
					{
						EditorGUI.indentLevel++;
						EditorGUILayout.LabelField("wait for:");
						EditorGUI.indentLevel++;
						my_target.wait_for_days = EditorGUILayout.IntSlider("days",my_target.wait_for_days,0,365);
						my_target.wait_for_hours = EditorGUILayout.IntSlider("hours",my_target.wait_for_hours,0,23);
						my_target.wait_for_minutes = EditorGUILayout.IntSlider("minutes",my_target.wait_for_minutes,0,59);
						my_target.wait_for_seconds = EditorGUILayout.IntSlider("seconds",my_target.wait_for_seconds,0,59);
						EditorGUI.indentLevel--;
						EditorGUI.indentLevel--;
					}
					
					if (my_target.if_not_continue_restart_with_lives < 1)
						GUI.color = Color.red;
					else
						GUI.color = Color.white;
					my_target.if_not_continue_restart_with_lives = EditorGUILayout.IntField("restart lives",my_target.if_not_continue_restart_with_lives);
					GUI.color = Color.white;
					
					my_target.if_not_continue_lose_gained_stars =  EditorGUILayout.Toggle("lose star score",my_target.if_not_continue_lose_gained_stars);
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
					
					EditorGUI.indentLevel--;
				}
				else //no continue allowed
				{
					EditorGUI.indentLevel++;
					my_target.if_player_not_continue_selected = (game_master.if_player_not_continue)EditorGUILayout.EnumPopup("not continue and restart from",my_target.if_player_not_continue_selected);

					my_target.when_restart_selected = (game_master.when_restart)EditorGUILayout.EnumPopup("when restart",my_target.when_restart_selected);
						if(my_target.when_restart_selected == game_master.when_restart.give_lives_after_countdown)
						{
							EditorGUI.indentLevel++;
							EditorGUILayout.LabelField("wait for:");
							EditorGUI.indentLevel++;
							my_target.wait_for_days = EditorGUILayout.IntSlider("days",my_target.wait_for_days,0,365);
							my_target.wait_for_hours = EditorGUILayout.IntSlider("hours",my_target.wait_for_hours,0,23);
							my_target.wait_for_minutes = EditorGUILayout.IntSlider("minutes",my_target.wait_for_minutes,0,59);
							my_target.wait_for_seconds = EditorGUILayout.IntSlider("seconds",my_target.wait_for_seconds,0,59);
							EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;
						}
						
						if (my_target.if_not_continue_restart_with_lives < 1)
							GUI.color = Color.red;
						else
							GUI.color = Color.white;
						my_target.if_not_continue_restart_with_lives = EditorGUILayout.IntField("restart lives",my_target.if_not_continue_restart_with_lives);
						GUI.color = Color.white;
						
						my_target.if_not_continue_lose_gained_stars =  EditorGUILayout.Toggle("lose star score",my_target.if_not_continue_lose_gained_stars);
					EditorGUI.indentLevel--;
				}
				
				
				
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
			
		}

		my_target.editor_show_score = EditorGUILayout.Foldout(my_target.editor_show_score, "Score");
		if (my_target.editor_show_score)
		{
			EditorGUI.indentLevel++;
			my_target.show_star_score = EditorGUILayout.Toggle("use star score",my_target.show_star_score);
			if (my_target.show_star_score)
				{
				EditorGUI.indentLevel++;
				my_target.show_progres_bar = EditorGUILayout.Toggle("show progres bar",my_target.show_progres_bar);
				EditorGUI.indentLevel--;
				}
			else
				my_target.show_star_score = false;

			my_target.show_int_score = EditorGUILayout.Toggle("use int score",my_target.show_int_score);
			if (my_target.show_int_score)
				{
				EditorGUI.indentLevel++;
				my_target.score_name  = EditorGUILayout.TextField("score name",my_target.score_name);
				my_target.what_say_if_new_stage_record = EditorGUILayout.TextField("what say if new stage record",my_target.what_say_if_new_stage_record);
				my_target.what_say_if_new_personal_record = EditorGUILayout.TextField("what say if new personal record",my_target.what_say_if_new_personal_record);
				if (my_target.number_of_save_profile_slot_avaibles > 1)
					{
					EditorGUI.indentLevel++;
					my_target.what_say_if_new_device_record = EditorGUILayout.TextField("what say if new device record",my_target.what_say_if_new_device_record);
					EditorGUI.indentLevel--;
					}
				else
					my_target.show_int_score_rank = false;

				my_target.show_score_in_lose_screen_too = EditorGUILayout.Toggle("show score in lose_screen too",my_target.show_score_in_lose_screen_too);
				my_target.show_int_score_rank = EditorGUILayout.Toggle("show int score rank",my_target.show_int_score_rank);
				my_target.show_int_score_stage_record_in_game_stage = EditorGUILayout.Toggle("show stage record in game",my_target.show_int_score_stage_record_in_game_stage);
				EditorGUI.indentLevel--;
				}
			else
				my_target.show_score_in_lose_screen_too = false;
			EditorGUI.indentLevel--;
		}

		//audio
		my_target.editor_show_audio = EditorGUILayout.Foldout(my_target.editor_show_audio, "Audio");
		if (my_target.editor_show_audio)
		{
			EditorGUI.indentLevel++;
			my_target.sfx_source = EditorGUILayout.ObjectField("sfx source", my_target.sfx_source, typeof(AudioSource), true) as AudioSource;
			my_target.tap_sfx = EditorGUILayout.ObjectField("tap sfx", my_target.tap_sfx, typeof(AudioClip), false) as AudioClip;
			my_target.tap_error_sfx = EditorGUILayout.ObjectField("tap error sfx", my_target.tap_error_sfx, typeof(AudioClip), false) as AudioClip;

			EditorGUILayout.Space();
			my_target.music_source = EditorGUILayout.ObjectField("music source", my_target.music_source, typeof(AudioSource), true) as AudioSource;
			my_target.fade_music = EditorGUILayout.FloatField("fade music",my_target.fade_music);
			my_target.music_menu = EditorGUILayout.ObjectField("music menu", my_target.music_menu, typeof(AudioClip), false) as AudioClip;

			my_target.when_win_play_selected = (game_master.when_win_play)EditorGUILayout.EnumPopup("When win play",my_target.when_win_play_selected);
			EditorGUI.indentLevel++;
				if (my_target.when_win_play_selected == game_master.when_win_play.no)
					my_target.music_stage_win = null;
				else if (my_target.when_win_play_selected == game_master.when_win_play.music)
					{
					EditorGUILayout.BeginHorizontal();
					my_target.music_stage_win = EditorGUILayout.ObjectField("music stage win", my_target.music_stage_win, typeof(AudioClip), false) as AudioClip;
					my_target.play_win_music_in_loop = EditorGUILayout.Toggle("loop",my_target.play_win_music_in_loop);
					EditorGUILayout.EndHorizontal();
					}
				else if (my_target.when_win_play_selected == game_master.when_win_play.sfx)
					{
					my_target.music_stage_win = EditorGUILayout.ObjectField("sfx stage win", my_target.music_stage_win, typeof(AudioClip), false) as AudioClip;
					my_target.play_win_music_in_loop = false;
					}
				
			for (int i = 0; i < my_target.show_big_star_sfx.Length; i++)
					my_target.show_big_star_sfx[i] = EditorGUILayout.ObjectField("big star " + (i+1).ToString() + " sfx", my_target.show_big_star_sfx[i], typeof(AudioClip), false) as AudioClip;

			EditorGUI.indentLevel--;

			my_target.when_lose_play_selected = (game_master.when_lose_play)EditorGUILayout.EnumPopup("When lose play",my_target.when_lose_play_selected);
			EditorGUI.indentLevel++;
			if (my_target.when_lose_play_selected == game_master.when_lose_play.no)
				my_target.music_stage_lose = null;
			else if (my_target.when_lose_play_selected == game_master.when_lose_play.music)
			{
				EditorGUILayout.BeginHorizontal();
				my_target.music_stage_lose = EditorGUILayout.ObjectField("music stage lose", my_target.music_stage_lose, typeof(AudioClip), false) as AudioClip;
				my_target.play_lose_music_in_loop = EditorGUILayout.Toggle("loop",my_target.play_lose_music_in_loop);
				EditorGUILayout.EndHorizontal();
			}
			else if (my_target.when_lose_play_selected == game_master.when_lose_play.sfx)
			{
				my_target.music_stage_lose = EditorGUILayout.ObjectField("sfx stage lose", my_target.music_stage_lose, typeof(AudioClip), false) as AudioClip;
				my_target.play_lose_music_in_loop = false;
			}
			EditorGUI.indentLevel--;


			EditorGUI.indentLevel--;
		}

		//pad
		my_target.editor_show_pad = EditorGUILayout.Foldout(my_target.editor_show_pad, "Pad");
		if (my_target.editor_show_pad)
		{
			EditorGUI.indentLevel++;
			my_target.use_pad= EditorGUILayout.Toggle("use pad",my_target.use_pad);
			if (my_target.use_pad)
				{
				my_target.normal_button_color = EditorGUILayout.ColorField("normal",my_target.normal_button_color);
				my_target.highlighted_button_color = EditorGUILayout.ColorField("highlighted",my_target.highlighted_button_color);
				EditorGUILayout.LabelField("Buttons:");
					EditorGUI.indentLevel++;
					my_target.pad_start_button = (KeyCode)EditorGUILayout.EnumPopup("Start",my_target.pad_start_button);
					//my_target.pad_ok_button = (KeyCode)EditorGUILayout.EnumPopup("OK",my_target.pad_ok_button);
					my_target.pad_back_button = (KeyCode)EditorGUILayout.EnumPopup("Back",my_target.pad_back_button);
					my_target.pad_next_button = (KeyCode)EditorGUILayout.EnumPopup("Next",my_target.pad_next_button);
					my_target.pad_previous_button = (KeyCode)EditorGUILayout.EnumPopup("Previous",my_target.pad_previous_button);
					my_target.pad_pause_button = (KeyCode)EditorGUILayout.EnumPopup("Pause",my_target.pad_pause_button);
					EditorGUI.indentLevel--;
				}
			EditorGUI.indentLevel--;
		}

		//store
		my_target.editor_show_store = EditorGUILayout.Foldout(my_target.editor_show_store, "Store");
		if (my_target.editor_show_store)
		{
			EditorGUI.indentLevel++;
			my_target.store_enabled = EditorGUILayout.Toggle("store enabled",my_target.store_enabled);
			if (my_target.store_enabled)
			{
				EditorGUI.indentLevel++;
				my_target.start_virtual_money = EditorGUILayout.IntField("start virtual money",my_target.start_virtual_money);
				my_target.virtual_money_cap = EditorGUILayout.IntField("virtual money cap",my_target.virtual_money_cap);
				my_target.virtual_money_name = EditorGUILayout.TextField("virtual money name",my_target.virtual_money_name);
				my_target.can_buy_virtual_money_with_real_money = EditorGUILayout.Toggle("can buy virtual money with real money",my_target.can_buy_virtual_money_with_real_money);
				if (my_target.can_buy_virtual_money_with_real_money)
					{
					EditorGUI.indentLevel++;
					my_target.buy_virtual_money_with_real_money_with_soomla = EditorGUILayout.Toggle("...with Soomla",my_target.buy_virtual_money_with_real_money_with_soomla);
					if (my_target.buy_virtual_money_with_real_money_with_soomla)
						my_target.my_Soomla_billing_script = EditorGUILayout.ObjectField("my_Soomla_billing", my_target.my_Soomla_billing_script , typeof(Soomla.Store.my_Soomla_billing), true) as Soomla.Store.my_Soomla_billing;
					EditorGUI.indentLevel--;
					}
				else
					my_target.buy_virtual_money_with_real_money_with_soomla = false;

				my_target.show_purchase_feedback = EditorGUILayout.Toggle("show purchase feedback",my_target.show_purchase_feedback);

				EditorGUILayout.LabelField("show buttons even if its cap is reached? Yes for:");
					EditorGUI.indentLevel++;
					my_target.show_lives_even_if_cap_reached = EditorGUILayout.Toggle("lives",my_target.show_lives_even_if_cap_reached);
					my_target.show_continue_tokens_even_if_cap_reached = EditorGUILayout.Toggle("continue tokens",my_target.show_continue_tokens_even_if_cap_reached);
					my_target.show_consumable_item_even_if_cap_reached = EditorGUILayout.Toggle("consumable items",my_target.show_consumable_item_even_if_cap_reached);
					my_target.show_incremental_item_even_if_cap_reached = EditorGUILayout.Toggle("incremental items",my_target.show_incremental_item_even_if_cap_reached);
					my_target.show_virtual_money_even_if_cap_reached = EditorGUILayout.Toggle("virtual money",my_target.show_virtual_money_even_if_cap_reached);
					EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}


		//debug
		my_target.editor_show_debug = EditorGUILayout.Foldout(my_target.editor_show_debug, "Debug");
		if (my_target.editor_show_debug)
			{
			EditorGUILayout.LabelField("show debug:");
			EditorGUI.indentLevel++;
				my_target.show_debug_messages = EditorGUILayout.Toggle("messages",my_target.show_debug_messages);
				my_target.show_debug_warnings = EditorGUILayout.Toggle("warnings",my_target.show_debug_warnings);
			EditorGUI.indentLevel--;


			if (GUILayout.Button("erase all save data"))
				my_target.Erase_saves();

			for(int i = 0; i < 10; i++)
				{
				EditorGUILayout.LabelField("Profile " + (i).ToString() + " = " + PlayerPrefs.GetString("profile_"+i.ToString()+"_name") + " (" + PlayerPrefs.GetInt("profile_"+i.ToString()+"_have_a_save_state_in_it") +")"+ " :");
					EditorGUI.indentLevel++;
						
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("lives " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_current_lives").ToString());
						EditorGUILayout.LabelField("continue tokes " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_current_continue_tokens").ToString());
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.LabelField("virtual money " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_virtual_money"));
					EditorGUILayout.LabelField("total star core " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_total_stars"));
					EditorGUILayout.LabelField("total number of stages solved " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_total_number_of_stages_in_the_game_solved"));

					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("world: " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_play_this_world_to_progress"));
						EditorGUILayout.LabelField("stage: " + PlayerPrefs.GetInt("profile_"+i.ToString()+"_play_this_stage_to_progress"));
					EditorGUILayout.EndHorizontal();

					EditorGUI.indentLevel--;
				}
			}

		if (EditorGUI.EndChangeCheck ())
			{
			EditorUtility.SetDirty(my_target);
			}
	}


}
