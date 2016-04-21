/*
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ads_master)), CanEditMultipleObjects]
public class ads_master_editor : Editor {

	public override void OnInspectorGUI () {

		ads_master my_target = (ads_master)target;
		EditorGUI.BeginChangeCheck ();


		//unity ads

			my_target.enable_ads= EditorGUILayout.Toggle("enable ads",my_target.enable_ads);
			if (my_target.enable_ads)
			{
				EditorGUI.indentLevel++;
				
				my_target.ads_test_mode= EditorGUILayout.Toggle("test mode",my_target.ads_test_mode);
				if (my_target.ads_test_mode)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Remember to set -test mode- FALSE for your final app build for store");
					EditorGUI.indentLevel--;
				}
				else
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField(" - WARNING - ");
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Set -test mode- FALSE only for your final app build for store");
					EditorGUILayout.LabelField("instead if you are just test your app, use TRUE");
					EditorGUILayout.LabelField("if you forgot it TRUE in your final app, you can set it FALSE from http://unityads.unity32.com/admin/");
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
				}
				
				my_target.rewardedVideoZone =  EditorGUILayout.TextField("rewardedVideoZone",my_target.rewardedVideoZone);
				my_target.iOS_ads_app_id = EditorGUILayout.TextField("iOS app id",my_target.iOS_ads_app_id);
				my_target.android_ads_app_id = EditorGUILayout.TextField("android app id",my_target.android_ads_app_id);
				
				EditorGUILayout.Space();
				my_target.minimum_time_interval_between_ads = EditorGUILayout.FloatField("minimum time interval between ads in seconds",my_target.minimum_time_interval_between_ads);
				my_target.reward_feedback_after_ad = EditorGUILayout.Toggle("show reward feedback after ad",my_target.reward_feedback_after_ad);
				EditorGUILayout.Space();
				
				EditorGUILayout.LabelField("Call ad in:");
				EditorGUI.indentLevel++;
				
				my_target.editor_show_home_ads = EditorGUILayout.Foldout(my_target.editor_show_home_ads, "Home scene");
				if (my_target.editor_show_home_ads)
				{
					EditorGUI.indentLevel++;
					my_target.ads_just_after_logo_when_game_start_as_daily_reward.when_app_start = true;
					Unity_ads_options_setup(my_target.ads_just_after_logo_when_game_start_as_daily_reward,"when app start, just after logo");
					Unity_ads_options_setup(my_target.ads_before_start_to_play_a_stage,"before start to play a stage");
					Unity_ads_options_setup(my_target.ads_when_return_to_home_scene_from_a_stage,"when return to home scene from a stage");
					EditorGUI.indentLevel--;
				}

				my_target.editor_show_in_game_ads = EditorGUILayout.Foldout(my_target.editor_show_in_game_ads, "Game");
				if (my_target.editor_show_in_game_ads)
				{
					EditorGUI.indentLevel++;
					Unity_ads_options_setup(my_target.ads_when_stage_start,"when stage start");
					//Unity_ads_options_setup(my_target.ads_when_stage_restart,"when stage restart");
					//Unity_ads_options_setup(my_target.ads_when_reach_a_check_point,"when reach checkpoint");
					my_target.ads_when_reach_a_check_point.this_ad_is_enabled = EditorGUILayout.Toggle("when reach checkpoint",my_target.ads_when_reach_a_check_point.this_ad_is_enabled);
					if (my_target.ads_when_reach_a_check_point.this_ad_is_enabled)
					{
						EditorGUI.indentLevel++;
						my_target.ads_when_reach_a_check_point.asking_text = EditorGUILayout.TextField("what say:",my_target.ads_when_reach_a_check_point.asking_text);
						my_target.ads_when_reach_a_check_point.chance_to_open_an_ad_here = 100;
						my_target.ads_when_reach_a_check_point.ignore_minimum_time_interval_between_ads = true;
						my_target.ads_when_reach_a_check_point.ask_to_player_if_he_want_to_watch_an_ad_before_start_it = true;
						my_target.ads_when_reach_a_check_point.my_special_ad_reward = ads_master.special_ad_reward.in_game_unlock_checkpoint;
						EditorGUI.indentLevel--;
					}
					
					Unity_ads_options_setup(my_target.ads_when_player_open_a_gift_packet,"when player open a gift packet");
					
					
					//if (my_game_master_target.show_int_score)
					//{
						my_target.ask_if_double_int_score_when_selected = (ads_master.ask_if_double_int_score_when)EditorGUILayout.EnumPopup("Ask if double the int score when stage end",my_target.ask_if_double_int_score_when_selected);
						EditorGUI.indentLevel++;
						if (my_target.ask_if_double_int_score_when_selected == ads_master.ask_if_double_int_score_when.no)
							my_target.ask_if_double_int_score.this_ad_is_enabled = false;
						else
						{
							my_target.ask_if_double_int_score.this_ad_is_enabled = true;
							my_target.ask_if_double_int_score.ask_to_player_if_he_want_to_watch_an_ad_before_start_it = true;
							my_target.ask_if_double_int_score.my_special_ad_reward = ads_master.special_ad_reward.double_int_score;
							if (my_target.ask_if_double_int_score_when_selected == ads_master.ask_if_double_int_score_when.random)
								my_target.ask_if_double_int_score.chance_to_open_an_ad_here = EditorGUILayout.IntSlider("% chance to ask",my_target.ask_if_double_int_score.chance_to_open_an_ad_here , 1 , 100);
							else
							{
								my_target.ask_if_double_int_score.chance_to_open_an_ad_here  = 100;
							}
						}
						
						EditorGUI.indentLevel--;

							my_target.ads_when_continue_screen_appear.this_ad_is_enabled = EditorGUILayout.Toggle("when continue screen appear",my_target.ads_when_continue_screen_appear.this_ad_is_enabled);
							if (my_target.ads_when_continue_screen_appear.this_ad_is_enabled)
							{
								EditorGUI.indentLevel++;
								my_target.ads_when_continue_screen_appear.chance_to_open_an_ad_here = EditorGUILayout.IntSlider("% chance to show this ad",my_target.ads_when_continue_screen_appear.chance_to_open_an_ad_here, 1 , 100);
								my_target.ads_when_continue_screen_appear.ignore_minimum_time_interval_between_ads = EditorGUILayout.Toggle("ignore minimum time interval between ads",my_target.ads_when_continue_screen_appear.ignore_minimum_time_interval_between_ads);
								my_target.ads_when_reach_a_check_point.my_special_ad_reward = ads_master.special_ad_reward.lose_continue_to_play;
								EditorGUI.indentLevel--;
							}

					EditorGUI.indentLevel--;
				}
				
				
				EditorGUI.indentLevel--;
			}




		if (EditorGUI.EndChangeCheck ())
		{
			EditorUtility.SetDirty(my_target);
		}
	}



	void Unity_ads_options_setup(ads_master.ad_options target_ad, string my_name)
	{
		target_ad.this_ad_is_enabled = EditorGUILayout.Toggle(my_name,target_ad.this_ad_is_enabled);
		if (target_ad.this_ad_is_enabled)
		{
			EditorGUI.indentLevel++;

			target_ad.show_custom_settings_in_inspector = EditorGUILayout.Foldout(target_ad.show_custom_settings_in_inspector, "Settings:");
			if (target_ad.show_custom_settings_in_inspector)
			{
				EditorGUI.indentLevel++;
				target_ad.chance_to_open_an_ad_here = EditorGUILayout.IntSlider("% chance to show this ad",target_ad.chance_to_open_an_ad_here, 1 , 100);
				
				if (target_ad.when_app_start)
				{
					EditorGUILayout.LabelField("wait for:");
					EditorGUI.indentLevel++;
					target_ad.start_app_ad_wait_for_days = EditorGUILayout.IntSlider("days",target_ad.start_app_ad_wait_for_days,0,365);
					target_ad.start_app_ad_wait_for_hours = EditorGUILayout.IntSlider("hours",target_ad.start_app_ad_wait_for_hours,0,23);
					target_ad.start_app_ad_wait_for_minutes = EditorGUILayout.IntSlider("minutes",target_ad.start_app_ad_wait_for_minutes ,0,59);
					EditorGUI.indentLevel--;
				}
				else
					target_ad.ignore_minimum_time_interval_between_ads = EditorGUILayout.Toggle("ignore minimum time interval between ads",target_ad.ignore_minimum_time_interval_between_ads);
				
				target_ad.ask_to_player_if_he_want_to_watch_an_ad_before_start_it = EditorGUILayout.Toggle("ASK to player if he want to watch an ad before start it",target_ad.ask_to_player_if_he_want_to_watch_an_ad_before_start_it);
				EditorGUI.indentLevel++;
				if (target_ad.ask_to_player_if_he_want_to_watch_an_ad_before_start_it)
				{
					target_ad.asking_text = EditorGUILayout.TextField("what say:",target_ad.asking_text);
					target_ad.my_rule = ads_master.ad_main_rule.rewarded_but_not_skip;
				}
				else
					target_ad.my_rule = ads_master.ad_main_rule.skipable_but_not_reward;
				EditorGUI.indentLevel--;
				
				
				if (target_ad.my_rule == ads_master.ad_main_rule.rewarded_but_not_skip)
				{
					EditorGUI.indentLevel++;
					
					target_ad.randrom_reward = EditorGUILayout.Toggle("random reward",target_ad.randrom_reward);
					
					
					if (target_ad.randrom_reward)
					{
						EditorGUI.indentLevel++;
						float chance_total = 0;
						bool chance_total_error = false;
						
						target_ad.random_slots = EditorGUILayout.IntSlider("slots",target_ad.random_slots, 2 , 10);
						if (target_ad.chance_to_give_this_reward.Length != target_ad.random_slots
						    || target_ad.kind_of_randrom_reward.Length != target_ad.random_slots
						    || target_ad.min_item_id_randrom_reward.Length != target_ad.random_slots
						    || target_ad.max_item_id_randrom_reward.Length != target_ad.random_slots
						    || target_ad.randrom_reward_quantity_for_random_reward.Length != target_ad.random_slots
						    || target_ad.min_reward_quantity_for_random_reward.Length != target_ad.random_slots
						    || target_ad.max_reward_quantity_for_random_reward.Length != target_ad.random_slots
						    || target_ad.reward_quantity_for_random_reward.Length != target_ad.random_slots)
						{
							target_ad.chance_to_give_this_reward = new float[target_ad.random_slots];
							target_ad.kind_of_randrom_reward = new ads_master.ad_reward[target_ad.random_slots];
							target_ad.min_item_id_randrom_reward = new int[target_ad.random_slots];
							target_ad.max_item_id_randrom_reward = new int[target_ad.random_slots];
							target_ad.randrom_reward_quantity_for_random_reward = new bool[target_ad.random_slots];
							target_ad.min_reward_quantity_for_random_reward = new int[target_ad.random_slots];
							target_ad.max_reward_quantity_for_random_reward = new int[target_ad.random_slots];
							target_ad.reward_quantity_for_random_reward = new int[target_ad.random_slots];
						}
						
						//check % sum
						for (int i = 0; i < target_ad.random_slots; i++)
							chance_total += target_ad.chance_to_give_this_reward[i];
						if (chance_total != 100)
						{
							chance_total_error = true;
							EditorGUILayout.LabelField(" - ERROR - the chance total is "+chance_total+" but it MUST be 100");
						}
						
						for (int i = 0; i < target_ad.random_slots; i++)
						{
							//kind of reward
							if (target_ad.kind_of_randrom_reward[i] == ads_master.ad_reward.select_by_the_player)
								GUI.color = Color.red;
							else
								GUI.color = Color.white;
							target_ad.kind_of_randrom_reward[i] = (ads_master.ad_reward)EditorGUILayout.EnumPopup("reward",target_ad.kind_of_randrom_reward[i]);
							GUI.color = Color.white;
							
							EditorGUI.indentLevel++;
							if (chance_total_error)
								GUI.color = Color.red;
							else
								GUI.color = Color.white;
							target_ad.chance_to_give_this_reward[i] = EditorGUILayout.Slider("% chance",target_ad.chance_to_give_this_reward[i], 0 , 100);
							GUI.color = Color.white;
							
							if (target_ad.kind_of_randrom_reward[i] == ads_master.ad_reward.consumable_item)
							{
								target_ad.min_item_id_randrom_reward[i]   = EditorGUILayout.IntField("min item id",target_ad.min_item_id_randrom_reward[i]);
								target_ad.max_item_id_randrom_reward[i]   = EditorGUILayout.IntField("max item id",target_ad.max_item_id_randrom_reward[i]);
							}
							//quantity
							target_ad.randrom_reward_quantity_for_random_reward[i] = EditorGUILayout.Toggle("random quantity",target_ad.randrom_reward_quantity_for_random_reward[i]);
							EditorGUI.indentLevel++;
							if (target_ad.randrom_reward_quantity_for_random_reward[i])
							{
								target_ad.min_reward_quantity_for_random_reward[i] = EditorGUILayout.IntField("min quantity",target_ad.min_reward_quantity_for_random_reward[i]);
								target_ad.max_reward_quantity_for_random_reward[i] = EditorGUILayout.IntField("max quantity",target_ad.max_reward_quantity_for_random_reward[i]);
							}
							else
								target_ad.reward_quantity_for_random_reward[i] = EditorGUILayout.IntField("quantity",target_ad.reward_quantity_for_random_reward[i]);
							EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;
						}
						
						EditorGUI.indentLevel--;
					}
					else //always same reward
					{
						target_ad.my_ad_reward = (ads_master.ad_reward)EditorGUILayout.EnumPopup("reward",target_ad.my_ad_reward);
						
						if (target_ad.my_ad_reward == ads_master.ad_reward.consumable_item)
						{
							EditorGUI.indentLevel++;
							target_ad.choose_a_random_consumable = EditorGUILayout.Toggle("choose a random consumable",target_ad.choose_a_random_consumable);
							EditorGUI.indentLevel++;
							if (target_ad.choose_a_random_consumable)
							{
								if (target_ad.min_random_consumable_item_id < 0)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								target_ad.min_random_consumable_item_id  = EditorGUILayout.IntField("min item id",target_ad.min_random_consumable_item_id );
								GUI.color = Color.white;
								
								target_ad.max_random_consumable_item_id  = EditorGUILayout.IntField("max item id",target_ad.max_random_consumable_item_id );
							}
							else
							{
								if (target_ad.consumable_item_id < 0)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								target_ad.consumable_item_id = EditorGUILayout.IntField("item id",target_ad.consumable_item_id);
								GUI.color = Color.white;
							}
							EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;
						}
						
						if (target_ad.my_ad_reward != ads_master.ad_reward.select_by_the_player)
						{
							target_ad.randrom_reward_quantity = EditorGUILayout.Toggle("random quantity",target_ad.randrom_reward_quantity);
							if (target_ad.randrom_reward_quantity)
							{
								EditorGUI.indentLevel++;
								if (target_ad.min_reward_quantity < 1)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								target_ad.min_reward_quantity = EditorGUILayout.IntField("min",target_ad.min_reward_quantity);
								GUI.color = Color.white;
								
								if (target_ad.max_reward_quantity < 2)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								target_ad.max_reward_quantity = EditorGUILayout.IntField("max",target_ad.max_reward_quantity);
								GUI.color = Color.white;
								EditorGUI.indentLevel--;
							}
							else //always same quantity
							{
								if (target_ad.reward_quantity < 1)
									GUI.color = Color.red;
								else
									GUI.color = Color.white;
								target_ad.reward_quantity = EditorGUILayout.IntField("quantity",target_ad.reward_quantity);
								GUI.color = Color.white;
							}
						}
						else //reward selected by the player
						{
							EditorGUI.indentLevel++;
							target_ad.player_choices = EditorGUILayout.IntSlider("choices",target_ad.player_choices,2,4);
							EditorGUI.indentLevel++;
							for (int i = 0; i < target_ad.player_choices; i++)
							{
								EditorGUILayout.LabelField("button " + (i+1).ToString());
								EditorGUI.indentLevel++;
								target_ad.ad_reward_by_player_choice_selected[i] = (ads_master.ad_reward_by_player_choice)EditorGUILayout.EnumPopup("reward",target_ad.ad_reward_by_player_choice_selected[i]);
								//consumable item id
								if (target_ad.ad_reward_by_player_choice_selected[i] == ads_master.ad_reward_by_player_choice.consumable_item)
								{
									EditorGUILayout.BeginHorizontal();
									string t_id = "";
									if (!target_ad.player_choice_random_item_id[i])
									{
										if (target_ad.player_choice_consumable_item_id[i] < 0)
											GUI.color = Color.red;
										else
											GUI.color = Color.white;
										target_ad.player_choice_consumable_item_id[i] = EditorGUILayout.IntField("item id",target_ad.player_choice_consumable_item_id[i]);
										GUI.color = Color.white;
									}
									else
										t_id = " id";
									
									target_ad.player_choice_random_item_id[i] = EditorGUILayout.Toggle("random" + t_id,target_ad.player_choice_random_item_id[i]);
									EditorGUILayout.EndHorizontal();
									
									if (target_ad.player_choice_random_item_id[i])
									{
										EditorGUI.indentLevel++;
										if (target_ad.player_choice_random_item_id_min[i] < 0 || target_ad.player_choice_random_item_id_min[i] >= target_ad.player_choice_random_item_id_max[i])
											GUI.color = Color.red;
										else
											GUI.color = Color.white;
										target_ad.player_choice_random_item_id_min[i] = EditorGUILayout.IntField("min",target_ad.player_choice_random_item_id_min[i]);
										GUI.color = Color.white;
										
										if (target_ad.player_choice_random_item_id_max[i]  <= target_ad.player_choice_random_item_id_min[i] )
											GUI.color = Color.red;
										else
											GUI.color = Color.white;
										target_ad.player_choice_random_item_id_max[i] = EditorGUILayout.IntField("max",target_ad.player_choice_random_item_id_max[i]);
										GUI.color = Color.white;
										EditorGUI.indentLevel--;
									}
								}
								
								//quantity
								EditorGUILayout.BeginHorizontal();
								string tq = "";
								if (!target_ad.player_choice_random_quantity[i])
								{
									if (target_ad.player_choice_quantity[i] < 1)
										GUI.color = Color.red;
									else
										GUI.color = Color.white;
									target_ad.player_choice_quantity[i] = EditorGUILayout.IntField("quantity",target_ad.player_choice_quantity[i]);
									GUI.color = Color.white;
								}
								else
									tq = " quantity";
								
								target_ad.player_choice_random_quantity[i] = EditorGUILayout.Toggle("random" + tq,target_ad.player_choice_random_quantity[i]);
								EditorGUILayout.EndHorizontal();
								
								if (target_ad.player_choice_random_quantity[i])
								{
									EditorGUI.indentLevel++;
									if (target_ad.player_choice_random_quantity_min[i] < 0 || target_ad.player_choice_random_quantity_min[i] >= target_ad.player_choice_random_quantity_max[i])
										GUI.color = Color.red;
									else
										GUI.color = Color.white;
									target_ad.player_choice_random_quantity_min[i] = EditorGUILayout.IntField("min",target_ad.player_choice_random_quantity_min[i]);
									GUI.color = Color.white;
									
									if (target_ad.player_choice_random_quantity_max[i]  <= target_ad.player_choice_random_quantity_min[i] )
										GUI.color = Color.red;
									else
										GUI.color = Color.white;
									target_ad.player_choice_random_quantity_max[i] = EditorGUILayout.IntField("max",target_ad.player_choice_random_quantity_max[i]);
									GUI.color = Color.white;
									EditorGUI.indentLevel--;
								}
								
								
								
								EditorGUI.indentLevel--;
							}
							EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;
						}
					}
					
					
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

	}
}
*/