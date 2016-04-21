// DUMMY code: delete this if you whant use unity ads:
using UnityEngine;
using System.Collections;
using System;

public class ads_master : MonoBehaviour {
	
	public bool enable_ads = false; 

	[HideInInspector]public game_master my_game_master;
	[HideInInspector]public game_uGUI my_game_uGUI;
	[HideInInspector]public feedback_window my_feedback_window;
	[HideInInspector]public gift_manager my_gift_manager;
	[HideInInspector]public Info_bar my_info_bar;
	
	[HideInInspector]public int buy_button_cost;
	[HideInInspector]public checkpoint target_checkpoint;
	[HideInInspector]public float minimum_time_interval_between_ads;
	[HideInInspector]public float time_of_latest_ad_showed;
	
	[HideInInspector]public int current_quantity_reward_selected;
	[HideInInspector]public int current_item_id_reward_selected;
	
	[System.Serializable]
	public class ad_options
	{
		public bool this_ad_is_enabled;
		public bool ignore_minimum_time_interval_between_ads;
		public int chance_to_open_an_ad_here;
		public special_ad_reward my_special_ad_reward = special_ad_reward.none;
		public int player_choices;
		public bool[] player_choice_random_quantity = new bool[4];
		public int[] player_choice_random_quantity_min = new int[4];
		public int[] player_choice_random_quantity_max = new int[4];
		public int[] player_choice_quantity = new int[4];
		public ad_reward_by_player_choice[] ad_reward_by_player_choice_selected = new ad_reward_by_player_choice[4];
		public bool[] player_choice_random_item_id = new bool[4];
		public int[] player_choice_random_item_id_min = new int[4];
		public int[] player_choice_random_item_id_max = new int[4];
		public int[] player_choice_consumable_item_id = new int[4];
		
	}
	
	[HideInInspector]public ad_options ads_before_start_to_play_a_stage;
	[HideInInspector]public ad_options ads_when_return_to_home_scene_from_a_stage;
	[HideInInspector]public ad_options ads_just_after_logo_when_game_start_as_daily_reward;
	[HideInInspector]public ad_options ads_when_stage_start;
	[HideInInspector]public ad_options ads_when_player_open_a_gift_packet;
	[HideInInspector]public ad_options ads_when_reach_a_check_point;
	[HideInInspector]public ad_options ads_when_continue_screen_appear;
	[HideInInspector]public ad_options ask_if_double_int_score;
	[HideInInspector]public ad_options current_ad = null;
	
	[HideInInspector]public ad_reward current_reward_selected = ad_reward.virtual_money;
	
	public enum ad_reward_by_player_choice
	{
		virtual_money, 
		new_live,
		consumable_item
	}
	
	public enum ask_if_double_int_score_when
	{
		no,
		random
		
	}
	[HideInInspector]public ask_if_double_int_score_when ask_if_double_int_score_when_selected = ask_if_double_int_score_when.no;
	
	public enum special_ad_reward
	{
		none,
		in_game_unlock_checkpoint,
		
		double_int_score,
		
		win_screen_double_virtual_money_gain_in_this_stage,
		win_screen_get_the_tird_star,
		
		lose_continue_to_play
	}
	
	public enum ad_reward
	{
		virtual_money, 
		new_live,
		consumable_item,
		select_by_the_player
	}
	
	public void Initiate_ads()
	{
	}
	
	public void Call_ad(ad_options target_ad)
	{
	}
	
	public void Show_ad(bool rewarded)
	{
	}
	
	public void Set_app_start_ad_countdown()
	{
	}
	
	public void Give_reward()
	{
	}
	
	public void Reset_reward()
	{
	}
	
	public bool Check_app_start_ad_countdown()
	{
		return false;
	}
	
	public bool Advertisement_isInitialized()
	{
		return false;
	}
}
//DUMMY code END here - Stop delete here and enable the code below:

/*
using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;//this allow you to use unity-ads
using System;


public class ads_master : MonoBehaviour {
	
	//Unity Ads
	public string rewardedVideoZone = "rewardedVideo";
	public bool enable_ads;
	public bool ads_test_mode;
	public string iOS_ads_app_id;
	public string android_ads_app_id;
	public string app_id_selected;
	public float minimum_time_interval_between_ads;
	public float time_of_latest_ad_showed;
	public bool reward_feedback_after_ad;
	public feedback_window my_feedback_window;
	
	public enum ad_reward
	{
		virtual_money, 
		new_live,
		consumable_item,
		select_by_the_player
	}
	
	public enum special_ad_reward
	{
		none,
		in_game_unlock_checkpoint,
		
		double_int_score,
		
		win_screen_double_virtual_money_gain_in_this_stage,
		win_screen_get_the_tird_star,
		
		lose_continue_to_play
	}
	public checkpoint target_checkpoint;
	public game_uGUI my_game_uGUI;
	public Info_bar my_info_bar;
	
	public enum ad_reward_by_player_choice
	{
		virtual_money, 
		new_live,
		consumable_item
	}
	
	
	public enum allow_double_int_score_ads_if
	{
		no,
		new_stage_record,
		new_personal_record,
		new_device_record,
		new_online_record
	}
	
	public enum ad_main_rule
	{
		skipable_but_not_reward,
		rewarded_but_not_skip
	}
	
	public DateTime start_app_ad_target_time;
	
	[System.Serializable]
	public class ad_options
	{
		//only for ad at the start
		public bool when_app_start;
		public int start_app_ad_wait_for_days;
		public int start_app_ad_wait_for_hours;
		public int start_app_ad_wait_for_minutes;
		
		public bool this_ad_is_enabled;
		
		public bool use_typical_settings = true;
		public bool show_custom_settings_in_inspector;
		
		public int chance_to_open_an_ad_here;//automatically or by asking
		
		public bool ask_to_player_if_he_want_to_watch_an_ad_before_start_it;
		public string asking_text;
		
		public bool ignore_minimum_time_interval_between_ads;
		
		public ad_main_rule my_rule;//if this ad is rewarded or skippable
		
		public int chance_to_reward_the_player_when_not_skip_a_skipable_ad;
		
		public bool randrom_reward;
		public int random_slots;
		public float[] chance_to_give_this_reward = new float[2]; 
		public ad_reward[] kind_of_randrom_reward = new ad_reward[2];
		public int[] min_item_id_randrom_reward = new int[2];
		public int[] max_item_id_randrom_reward = new int[2];
		public bool[] randrom_reward_quantity_for_random_reward = new bool[2];
		public int[] min_reward_quantity_for_random_reward = new int[2]; 
		public int[] max_reward_quantity_for_random_reward = new int[2];
		public int[] reward_quantity_for_random_reward = new int[2];
		
		
		public ad_reward my_ad_reward;
		public special_ad_reward my_special_ad_reward = special_ad_reward.none;
		public bool randrom_reward_quantity;
		public int min_reward_quantity;
		public int max_reward_quantity;
		public int reward_quantity;
		
		//for consumable reward
		public int consumable_item_id;
		public bool choose_a_random_consumable;
		public int min_random_consumable_item_id;
		public int max_random_consumable_item_id;
		
		//for reward choiced by the player
		public int player_choices;//from 2 to 4
		public ad_reward_by_player_choice[] ad_reward_by_player_choice_selected = new ad_reward_by_player_choice[4];
		public int[] player_choice_quantity = new int[4];
		public bool[] player_choice_random_quantity = new bool[4];
		public int[] player_choice_random_quantity_min = new int[4];
		public int[] player_choice_random_quantity_max = new int[4];
		public int[] player_choice_consumable_item_id = new int[4];
		public bool[] player_choice_random_item_id = new bool[4];
		public int[] player_choice_random_item_id_min = new int[4];
		public int[] player_choice_random_item_id_max = new int[4];
		
	}
	public ad_options ads_typical_settings;
	
	//special settings
	//home scene:
	public bool editor_show_home_ads;
	public ad_options ads_just_after_logo_when_game_start_as_daily_reward;
	public ad_options ads_before_start_to_play_a_stage;
	public ad_options ads_when_return_to_home_scene_from_a_stage;
	//game scene:
	//win screen:
	public bool editor_show_win_screen_ads;
	public ad_options ads_when_win_screen;
	public ad_options ads_when_you_have_gain_virtual_money_in_this_stage;
	//if get 2 stars allow to gain the last one
	public ad_options ads_when_win_screen_get_the_third_star;
	//lose screen
	public bool editor_show_lose_screen_ads;
	public ad_options ads_when_continue_screen_appear;
	//in game
	public bool editor_show_in_game_ads;
	public ad_options ads_when_stage_start;
	public ad_options ads_when_reach_a_check_point;
	public ad_options ads_when_player_open_a_gift_packet;
	//score
	public ad_options ask_if_double_int_score;
	public enum ask_if_double_int_score_when
	{
		no,
		random,
		//personal_stage_record,
		//device_stage_record,
		//personal_record,
		//device_record,
		//online_record
	}
	public ask_if_double_int_score_when ask_if_double_int_score_when_selected = ask_if_double_int_score_when.no;
	
	//the ad and the reward selected
	public ad_options current_ad = null;
	public ad_reward current_reward_selected = ad_reward.virtual_money;
	public int current_quantity_reward_selected = 0;
	public int current_item_id_reward_selected = -1;
	public int buy_button_cost;//only when you whant allow to pay instead of watch the video ad
	
	//show reward ad gui
	public gift_manager my_gift_manager;

	public game_master my_game_master;

	//debug
	public bool show_debug_warnings;
	public bool show_debug_messages;
	
	public void Initiate_ads()
	{
		if(enable_ads && !Advertisement.isInitialized)
		{
			current_ad = null;
			app_id_selected = "";
			#if UNITY_IPHONE
			app_id_selected = iOS_ads_app_id;
			#elif UNITY_ANDROID
			app_id_selected = android_ads_app_id;
			#endif
			if (app_id_selected == "" && show_debug_warnings)
				Debug.LogWarning("No app_id found");
			
		}
	}
	
	
	public void Show_ad(bool rewarded)
	{
		if (show_debug_messages)
			Debug.Log("Show_ad("+rewarded+") - id = " + app_id_selected);
		
		#if UNITY_EDITOR
		StartCoroutine(WaitForAd());
		#endif
		
		string zone = null;
		if (rewarded)
			zone = rewardedVideoZone;
		
		ShowOptions options = new ShowOptions();
		options.resultCallback = AdCallbackhandler;
		
		if (Advertisement.IsReady(zone))
		{
			Advertisement.Show(zone, options); //this work ONLY if "File" > "build setting" is set on iOS or Android
		}
		
	}
	
	void AdCallbackhandler(ShowResult result)
	{
		switch(result)
		{
		case ShowResult.Finished: //player had watched the whole video
			if (show_debug_messages)
				Debug.Log("Ad Finished: give reward?");
			
			Debug.Log(current_ad.my_rule);
			if (current_ad.my_rule == ad_main_rule.rewarded_but_not_skip)
				Give_reward();
			else
			{
				if (UnityEngine.Random.Range(1,100) <= current_ad.chance_to_reward_the_player_when_not_skip_a_skipable_ad)
					Give_reward();
			}
			
			time_of_latest_ad_showed = Time.realtimeSinceStartup;
			Empty_ad();
			break;
			
		case ShowResult.Skipped: //player had skipped the video (only not rewarded ad can be skipped)
			if (show_debug_messages)
				Debug.Log("Ad skipped. Give penalty?");
			
			
			time_of_latest_ad_showed = Time.realtimeSinceStartup;
			Empty_ad();
			break;
			
		case ShowResult.Failed: //for some reason the video not start
			if (show_debug_messages)
				Debug.LogWarning("Ad video Failed");
			Empty_ad();
			break;
		}
	}
	
	void Empty_ad()
	{
		current_ad = null;
		ad_reward current_reward_selected = ad_reward.virtual_money;
		current_quantity_reward_selected = 0;
		current_item_id_reward_selected = -1;
		buy_button_cost = 0;
	}
	
	public void Give_reward()
	{
		if (current_ad.my_special_ad_reward == special_ad_reward.none)
		{
			switch(current_reward_selected)
			{
			case ad_reward.virtual_money:

				if (my_game_master.buy_virtual_money_with_real_money_with_soomla)
					{
					//Enable those two lines FOR SOOMLA
					//my_game_master.my_Soomla_billing_script.Give_virtual_money_for_free(my_game_master.current_profile_selected,current_quantity_reward_selected);
					//my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
					}
				else
					my_game_master.current_virtual_money[my_game_master.current_profile_selected] += current_quantity_reward_selected;
				
				if (show_debug_messages)	
					Debug.Log("player rewarded with " + current_quantity_reward_selected + " virtual money");
				
				if (reward_feedback_after_ad)
					my_feedback_window.Start_me(my_feedback_window.virtual_money_ico,current_quantity_reward_selected,my_game_master.virtual_money_name);
				break;
				
			case ad_reward.new_live:
				
				my_game_master.current_lives[my_game_master.current_profile_selected] += current_quantity_reward_selected;
				
				if (show_debug_messages)
					Debug.Log("player rewarded with " + current_quantity_reward_selected + " lives");
				
				if (reward_feedback_after_ad)
					my_feedback_window.Start_me(my_feedback_window.live_ico,current_quantity_reward_selected,my_game_master.lives_name);
				
				break;
				
			case ad_reward.consumable_item:
				
				my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][current_item_id_reward_selected] += current_quantity_reward_selected;
				
				if (show_debug_messages)
					Debug.Log("player rewarded with " + current_quantity_reward_selected + my_game_master.my_store_item_manager.consumable_item_list[current_item_id_reward_selected].name);
				
				if (reward_feedback_after_ad)
					my_feedback_window.Start_me(my_game_master.my_store_item_manager.consumable_item_list[current_item_id_reward_selected].icon,current_quantity_reward_selected,my_game_master.my_store_item_manager.consumable_item_list[current_item_id_reward_selected].name);
				
				break;
			}
		}
		else //special reward
		{
			Debug.Log("special reward");
			switch(current_ad.my_special_ad_reward)
			{
			case special_ad_reward.in_game_unlock_checkpoint:
				target_checkpoint.Enable_this_checkpoint();
				break;
				
			case special_ad_reward.lose_continue_to_play:
				my_game_uGUI.Update_lives(my_game_master.continue_give_new_lives);
				break;
				
			case special_ad_reward.double_int_score:
				my_game_uGUI.Score_doubled();
				break;
			}
		}
		
		if (my_info_bar)
			my_info_bar.Update_me();
		
		my_game_master.Save(my_game_master.current_profile_selected);
		Reset_reward();
		
	}
	
	public void Reset_reward()
	{
		current_ad = null;
		current_quantity_reward_selected = 0;
		current_item_id_reward_selected = -1;
		
		target_checkpoint = null;
		buy_button_cost = 0;
	}
	
	
	public float Choose (float[] probs) {
		
		float total = 0;
		
		foreach (float elem in probs) {
			total += elem;
		}
		
		float randomPoint = UnityEngine.Random.value * total;
		
		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}
	
	
	IEnumerator WaitForAd()
	{
		//pause the game when the ad is showing
		float currentTimeScale = Time.timeScale;
		Time.timeScale = 0; 
		yield return null;
		
		while (Advertisement.isShowing)
			yield return null;
		
		Time.timeScale = currentTimeScale; 
	}
	
	public bool Advertisement_isInitialized()
	{
		return Advertisement.isInitialized;
	}
	
	public void Call_ad(ad_options target_ad)
	{
		if (enable_ads && target_ad.this_ad_is_enabled)
		{
			if (target_ad.ignore_minimum_time_interval_between_ads 
			    || (minimum_time_interval_between_ads+time_of_latest_ad_showed) < Time.realtimeSinceStartup)
			{
				if (show_debug_messages)
					Debug.Log("ad pass time check");
				
				if (UnityEngine.Random.Range(1,100) <= target_ad.chance_to_open_an_ad_here)
				{
					if (show_debug_messages)
						Debug.Log("ads_just_after_logo_when_game_start_as_daily_reward" + " - random ok");
					
					if (target_ad.ask_to_player_if_he_want_to_watch_an_ad_before_start_it)
					{
						if (show_debug_messages)
							Debug.Log("ask");
						
						//decide the quantity of the reward (if it not is select by the player)
						if (target_ad.my_ad_reward != ad_reward.select_by_the_player)
						{
							if (target_ad.randrom_reward)
							{
								if (show_debug_messages)
									Debug.Log("random reward");
								
								//select the reward
								int reward_slot = (int)Choose(target_ad.chance_to_give_this_reward);
								current_reward_selected = (ad_reward)reward_slot;
								
								//selet item id
								if (current_reward_selected == ad_reward.consumable_item)
									current_item_id_reward_selected = UnityEngine.Random.Range(target_ad.min_item_id_randrom_reward[reward_slot],
									                                                           target_ad.max_item_id_randrom_reward[reward_slot]);
								
								//select the quantity
								if (target_ad.randrom_reward_quantity_for_random_reward[reward_slot])
								{
									if (show_debug_messages)
										Debug.Log("set random quantity");
									current_quantity_reward_selected = UnityEngine.Random.Range(target_ad.min_reward_quantity_for_random_reward[reward_slot],
									                                                            target_ad.max_reward_quantity_for_random_reward[reward_slot]);
								}
								else
								{
									if (show_debug_messages)
										Debug.Log("set quantity");
									current_quantity_reward_selected = target_ad.reward_quantity_for_random_reward[reward_slot];
								}
							}
							else
							{
								if (show_debug_messages)
									Debug.Log("specific reward reward");
								
								current_reward_selected = target_ad.my_ad_reward;
								
								if (target_ad.my_ad_reward == ad_reward.consumable_item)
								{
									if(target_ad.choose_a_random_consumable)
									{
										current_item_id_reward_selected = UnityEngine.Random.Range(target_ad.min_random_consumable_item_id,
										                                                           target_ad.max_random_consumable_item_id+1);
									}
									else 
										current_item_id_reward_selected = target_ad.consumable_item_id;
								}
								
								if (target_ad.randrom_reward_quantity)
									current_quantity_reward_selected = UnityEngine.Random.Range(target_ad.min_reward_quantity,
									                                                            target_ad.max_reward_quantity);
								else
									current_quantity_reward_selected = target_ad.reward_quantity;
							}
						}
						else 
						{
							if (show_debug_messages)
								Debug.Log("the reward will be chosen by the player");
							current_reward_selected = target_ad.my_ad_reward;
							
						}
						
						current_ad = target_ad;
						my_gift_manager.Start_me(target_ad.asking_text, //the message to show in the window
						                         current_reward_selected,//the kind of reward
						                         current_quantity_reward_selected);//quantity
					}
					else
					{
						if (show_debug_messages)
							Debug.Log("ad start automatically");
						current_ad = target_ad;
						
						
						if (current_ad == target_ad)
							Set_app_start_ad_countdown();
						
						//star ad
						Show_ad(false);//false = not rewarded
					}
				}
				else
				{
					if (show_debug_messages)
						Debug.Log("ad - random fail");
				}
			}
			else
			{
				if (show_debug_messages)
					Debug.Log("ad don't pass time check");
			}
		}
	}

	#region start ad countdown
	public void Set_app_start_ad_countdown()
	{
		start_app_ad_target_time = DateTime.Now;
		start_app_ad_target_time = start_app_ad_target_time.AddDays((double)ads_just_after_logo_when_game_start_as_daily_reward.start_app_ad_wait_for_days);
		start_app_ad_target_time = start_app_ad_target_time.AddHours((double)ads_just_after_logo_when_game_start_as_daily_reward.start_app_ad_wait_for_hours);
		start_app_ad_target_time = start_app_ad_target_time.AddMinutes((double)ads_just_after_logo_when_game_start_as_daily_reward.start_app_ad_wait_for_minutes);
		
		//save
		PlayerPrefs.SetString("start_app_ad_target_time",start_app_ad_target_time.ToString());
		
		if (show_debug_messages)
			Debug.Log("start app ad target time: " + start_app_ad_target_time);
		
	}
	
	public bool Check_app_start_ad_countdown()
	{
		//load
		string temp_string = PlayerPrefs.GetString("start_app_ad_target_time");
		
		Debug.Log("Check_app_start_ad_countdown: " + temp_string);
		
		if (temp_string != "")
		{
			start_app_ad_target_time = DateTime.Parse(temp_string);
			
			if (show_debug_messages)
				Debug.Log("start app ad target time: " + start_app_ad_target_time + " *** DateTime.Now = " + DateTime.Now);
			
			if (start_app_ad_target_time != null)
			{
				if (start_app_ad_target_time <= DateTime.Now)
					return true;
				else
					return false;
			}
			else
				return true;
		}
		else
			return true;
		
		
	}
	
	#endregion
}

 */