using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class continue_window : MonoBehaviour {

	public Text window_heading;
	public Text my_timer;
	bool stop_timer;
	public game_uGUI my_game_uGUI;
	public game_master my_game_master;
	bool can_take_input;

	public GameObject yes_button;
	public GameObject buy_button;
		public Text but_button_text;
	public GameObject watch_button;

	float initial_time_scale;
	float time_scale_multiplier = 0.01f;

	

	public void Start_me_with_ad(ads_master.ad_options target_ad)
	{
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
		else
			{
			if (my_game_uGUI.show_debug_warnings)
				Debug.LogWarning("You need to open this stage from Home scene in order to see continue with ads");
			return;
			}

		if (target_ad.ignore_minimum_time_interval_between_ads 
		    || (my_game_master.my_ads_master.minimum_time_interval_between_ads+my_game_master.my_ads_master.time_of_latest_ad_showed) < Time.realtimeSinceStartup)
			{
			if (my_game_uGUI.show_debug_messages)
				Debug.Log("ad pass time check");
			
			if (UnityEngine.Random.Range(1,100) <= target_ad.chance_to_open_an_ad_here)
				{
				my_game_master.my_ads_master.current_ad = target_ad;

				yes_button.SetActive(false);
				watch_button.SetActive(true);

				initial_time_scale = Time.timeScale;
				Time.timeScale = 1 * time_scale_multiplier;

				Update_heading();
				game_uGUI.in_pause = true;
				game_uGUI.allow_game_input = false;
				can_take_input = true;
				
				this.gameObject.SetActive(true);

				if (my_game_master.continue_menu_have_countdown)
				{
					my_timer.text = my_game_master.continue_menu_countdown_seconds.ToString();
					stop_timer = false;
					StartCoroutine(Countdown(my_game_master.continue_menu_countdown_seconds));
					my_timer.gameObject.SetActive(true);
				}
				else
					my_timer.gameObject.SetActive(false);
				}
			else
			{
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("ad - random fail");

				Continue_no(true);
				}
			}
		else
			{
			if (my_game_uGUI.show_debug_messages)
				Debug.Log("ad don't pass time check");

			Continue_no(true);
			}
	}


	public void Start_me()
	{
		yes_button.SetActive(true);
		watch_button.SetActive(false);

		initial_time_scale = Time.timeScale;
		Time.timeScale = 1 * time_scale_multiplier;


		if (game_master.game_master_obj)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

				Debug.Log("check if continue [Time.timeScale = " + Time.timeScale + "]");
			}

		if (my_game_master.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token
		    && my_game_master.current_continue_tokens[my_game_master.current_profile_selected] <= 0)
			{
			if (my_game_uGUI.show_debug_messages)
				Debug.Log("you can't continue");
			Continue_no(true);
			}
		else
			{
			if (my_game_uGUI.show_debug_messages)
				Debug.Log("you can continue");

			Update_heading();
			game_uGUI.in_pause = true;
			game_uGUI.allow_game_input = false;
			can_take_input = true;

			this.gameObject.SetActive(true);

			if (my_game_master.continue_menu_have_countdown)
			{
				my_timer.text = my_game_master.continue_menu_countdown_seconds.ToString();
				stop_timer = false;
				StartCoroutine(Countdown(my_game_master.continue_menu_countdown_seconds));
				my_timer.gameObject.SetActive(true);
			}
			else
				my_timer.gameObject.SetActive(false);

			my_game_uGUI.Invoke("Mark_continue",0.1f*time_scale_multiplier);
			//my_game_uGUI.Mark_this_button(my_game_uGUI.continue_window_target_button);
			}
	}

	void Update_heading()
	{
		if (my_game_master.continue_rule_selected == game_master.continue_rule.infinite_continues)
			window_heading.text = "";
		else if (my_game_master.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token)
			{
			if (my_game_master.my_ads_master.current_ad == my_game_master.my_ads_master.ads_when_continue_screen_appear)
				window_heading.text = "Continue";
			else
				window_heading.text = "Continues    Left   " + my_game_master.current_continue_tokens[my_game_master.current_profile_selected].ToString();
			}
	}

	IEnumerator Countdown(int seconds)
	{
		if (!stop_timer)
			{
			yield return new WaitForSeconds(1 * time_scale_multiplier);
			seconds--;
			my_timer.text = seconds.ToString();

			if(seconds > 0)
				StartCoroutine(Countdown(seconds));
			else
				{
				can_take_input = false;
				yield return new WaitForSeconds(1 * time_scale_multiplier);
				Continue_no(true);
				}
			}

	}


	public void Watch_ad()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);

		//close window
		this.gameObject.SetActive(false);
		my_game_master.a_window_is_open = false;
		Time.timeScale = initial_time_scale; 
		
		//star ad
		my_game_master.my_ads_master.Show_ad(true);//true = rewarded
	}


	public void Continue_yes()
	{
		if (my_game_uGUI.show_debug_messages)
			Debug.Log("Continue_yes(): " + can_take_input);

		if(can_take_input)
			{
			can_take_input = false;
			Update_heading();

			if (my_game_master.continue_menu_have_countdown)
				stop_timer = true;
				
			switch(my_game_master.continue_rule_selected)
				{
				case game_master.continue_rule.continue_cost_a_continue_token:
					if (my_game_uGUI.show_debug_messages)
						Debug.Log("continue_cost_a_continue_token");

					my_game_master.current_continue_tokens[my_game_master.current_profile_selected]--;
					Continue_from();
				break;
				/*
				case game_master.continue_rule.continue_cost_virtual_money:
					Debug.Log("continue_cost_virtual_money");
					if ((my_game_master.current_virtual_money[my_game_master.current_profile_selected] - my_game_master.continue_cost_virtual_money) >= 0)
						{
						my_game_master.current_virtual_money[my_game_master.current_profile_selected] -= my_game_master.continue_cost_virtual_money;
						Continue_from();
						}
					else
						Not_have_enough_money();
				break;
				*/
				case game_master.continue_rule.infinite_continues:
					Continue_from();
				break;
				}

			}
	}
	/*
	void Not_have_enough_money()
		{

		}*/

	void Continue_from()
		{
		this.gameObject.SetActive(false);
		Time.timeScale = initial_time_scale;
		my_game_uGUI.Update_lives(my_game_master.continue_give_new_lives);
		switch(my_game_master.if_player_continue_selected)
			{
			case game_master.if_player_continue.continue_playing_this_stage:
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("continue playing this stage");
				game_uGUI.in_pause = false;
				game_uGUI.allow_game_input = true;
				if (my_game_master.lose_lives_selected == game_master.lose_lives.when_show_lose_screen)
					{
					my_game_uGUI.retry_button.SetActive(true);
					my_game_uGUI.stage_button.SetActive(true);
					}
			break;

				case game_master.if_player_continue.restart_from_current_world_and_current_stage:
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("restart this stage");
				Application.LoadLevel (Application.loadedLevel); 
			break;

			case game_master.if_player_continue.restart_from_current_world_Stage_1:
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("restart_from_current_world_Stage_1");
			break;
	
			}
		}


	public void Continue_no(bool call_defeat_screen)
	{
		if (my_game_uGUI.show_debug_messages)
			Debug.Log("Continue_no()");

		if (call_defeat_screen)
			{
			Time.timeScale = initial_time_scale;
			can_take_input = false;
			}
		
			if (my_game_master.if_player_not_continue_selected == game_master.if_player_not_continue.restart_from_W1_Stage_1)
				{

				if (my_game_uGUI.show_debug_messages)
					Debug.Log("Reset_all_worlds");

				if(my_game_master.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token)
					my_game_master.current_continue_tokens[my_game_master.current_profile_selected] = my_game_master.start_continue_tokens;

				my_game_master.Reset_all_worlds();
				}
			else if (my_game_master.if_player_not_continue_selected == game_master.if_player_not_continue.restart_from_current_world_Stage_1)
				{
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("Reset_current_world if the playar not have already completed it");
				if (my_game_master.current_world[my_game_master.current_profile_selected] == my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected])
					{
					my_game_master.Reset_current_world(my_game_master.current_world[my_game_master.current_profile_selected],true);
					my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected] = my_game_master.current_world[my_game_master.current_profile_selected];
					my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected] = 0;
					}
				}
			else if (my_game_master.if_player_not_continue_selected == game_master.if_player_not_continue.restart_from_current_world_and_current_stage)
				{
				if (my_game_uGUI.show_debug_messages)
					Debug.Log("restart from here");
				}

			if (my_game_master.when_restart_selected == game_master.when_restart.give_lives_after_countdown)
				my_game_master.Set_date_countdown();

			//my_game_master.current_lives[my_game_master.current_profile_selected] = my_game_master.if_not_continue_restart_with_lives;

			my_game_master.refresh_stage_and_world_screens = true;
			my_game_master.Save(my_game_master.current_profile_selected);
			
			this.gameObject.SetActive(false);

			if (call_defeat_screen && (my_game_master.lose_lives_selected == game_master.lose_lives.in_game))
				{
				my_game_uGUI.Defeat();
				}

	}


}
