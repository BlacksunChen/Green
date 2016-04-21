using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gift_manager : MonoBehaviour {
	
	//window
	public Text window_text_heading;
	public Text window_text_message;
	public Text window_text_item_name;
	public Text window_text_item_description;
	//window rect transform
		public RectTransform my_window_rect;
		float normal_height;
		public float small_height;

	//buy button
	public GameObject buy_button;
	public Text buy_button_text;
	public Sprite buy_button_on;
	public Sprite buy_button_off;
	game_uGUI my_game_uGUI;

	//ads
	public Image ads_button_ico;
	public Sprite ads_button_internet_on;
	public Sprite ads_button_internet_off;

	[HideInInspector]public int gift_buttons_is_active;
	[HideInInspector]public int selected_button;
	int child_buttons;

	public Sprite select_button_sprite;
	public Sprite not_select_button_sprite;

	public Sprite virtual_money_ico;
	public Sprite live_ico;

	public GameObject gift_screen;

	//for single reward
	public GameObject single_reward;
	public Image single_reward_ico;
	public Text single_reward_quantity;

	[HideInInspector]public game_master my_game_master;
	public GameObject EventSystem_obj;

	float currentTimeScale;

	void Awake()
	{
		if (Application.loadedLevelName != "Home")
			my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
	}
	
	public void Start_me (string window_message, ads_master.ad_reward kind_of_reward, int quantity) {

		if (normal_height == 0)
			normal_height = my_window_rect.rect.height;

		//start pause
		currentTimeScale = Time.timeScale;
		Time.timeScale = 0; 

		if (my_game_master.use_pad)
			EventSystem_obj.SetActive(false);//in order to avoid pad input out the of the window

		if (my_game_master.show_debug_messages)
			Debug.Log("gift_manager - Start_me("+kind_of_reward+","+quantity+")");

		//setup window proprieties
		window_text_message.text = window_message;
		child_buttons = this.transform.childCount;
		my_game_master.a_window_is_open = true;

		//reset variables
		selected_button = -1;
		gift_buttons_is_active = 0;

		if (my_game_master.my_ads_master.current_ad.my_special_ad_reward == ads_master.special_ad_reward.none)
		{
			my_window_rect.sizeDelta = new Vector2(my_window_rect.rect.width,normal_height);
			if (buy_button)
				buy_button.SetActive(false);

			switch(kind_of_reward)
			{
			case ads_master.ad_reward.virtual_money:
				//be sure to not break the cap
				if ((my_game_master.current_virtual_money[my_game_master.current_profile_selected] + quantity) > my_game_master.virtual_money_cap)
					{
					quantity = my_game_master.virtual_money_cap-my_game_master.current_virtual_money[my_game_master.current_profile_selected];
					my_game_master.my_ads_master.current_quantity_reward_selected = quantity;
					}
				window_text_item_name.text = my_game_master.virtual_money_name;
				window_text_item_description.text = "";// quantity.ToString("N0");
				Single_reward_setup(virtual_money_ico,quantity);
			break;

			case ads_master.ad_reward.new_live:
				//be sure to not break the cap
				if ((my_game_master.current_lives[my_game_master.current_profile_selected] + quantity) > my_game_master.live_cap)
				{
					quantity = my_game_master.live_cap-my_game_master.current_lives[my_game_master.current_profile_selected];
					my_game_master.my_ads_master.current_quantity_reward_selected = quantity;
				}
				window_text_item_name.text = my_game_master.lives_name;
				window_text_item_description.text = "";// quantity.ToString("N0");
				Single_reward_setup(live_ico,quantity);
			break;

			case ads_master.ad_reward.consumable_item:
				if ((my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].avaible_from_world <= my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected]) //world
				    && (my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].avaible_from_stage <= my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected]))//stage
					{
					//be sure to not break the cap
					if ((my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][my_game_master.my_ads_master.current_item_id_reward_selected] + quantity) > my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].quantity_cap)
						{
						quantity = my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].quantity_cap - my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][my_game_master.my_ads_master.current_item_id_reward_selected];
						my_game_master.my_ads_master.current_quantity_reward_selected = quantity;
						}
					window_text_item_name.text = my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].name;
					window_text_item_description.text = my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].description;
					Single_reward_setup(my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].icon,quantity);
					}
				else
					{
					if (my_game_master.show_debug_messages)
						Debug.Log(my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].name + " not is avaible yet");
					}
			break;

			case ads_master.ad_reward.select_by_the_player:
				//deactivate the single reward ico
				single_reward.SetActive(false);
				//activate the choices
				for (int i = 0; i < child_buttons; i++)
					{
					if (i < my_game_master.my_ads_master.current_ad.player_choices)
						{
						gift_button child_script = this.transform.GetChild(i).GetComponent<gift_button>();
						this.transform.GetChild(i).GetComponent<Image>().sprite = not_select_button_sprite;

						//setup quantity
						if (my_game_master.my_ads_master.current_ad.player_choice_random_quantity[i])
							quantity = UnityEngine.Random.Range(my_game_master.my_ads_master.current_ad.player_choice_random_quantity_min[i],
							                                    my_game_master.my_ads_master.current_ad.player_choice_random_quantity_max[i]);
						
						else
							quantity = my_game_master.my_ads_master.current_ad.player_choice_quantity[i];

						//setup kind	
						switch(my_game_master.my_ads_master.current_ad.ad_reward_by_player_choice_selected[i])
						{
						case ads_master.ad_reward_by_player_choice.virtual_money:
							child_script.give_this_selected = ads_master.ad_reward.virtual_money;
							//cap quantity
							if ((my_game_master.current_virtual_money[my_game_master.current_profile_selected] + quantity) > my_game_master.virtual_money_cap)
								quantity = my_game_master.virtual_money_cap-my_game_master.current_virtual_money[my_game_master.current_profile_selected];

						break;

						case ads_master.ad_reward_by_player_choice.new_live:
							child_script.give_this_selected = ads_master.ad_reward.new_live;
							//cap quantity
							if ((my_game_master.current_lives[my_game_master.current_profile_selected] + quantity) > my_game_master.live_cap)
								quantity = my_game_master.live_cap-my_game_master.current_lives[my_game_master.current_profile_selected];
						break;

						case ads_master.ad_reward_by_player_choice.consumable_item:
							child_script.give_this_selected = ads_master.ad_reward.consumable_item;
							if (my_game_master.my_ads_master.current_ad.player_choice_random_item_id[i])
								child_script.item_id = UnityEngine.Random.Range(my_game_master.my_ads_master.current_ad.player_choice_random_item_id_min[i],
								                                                my_game_master.my_ads_master.current_ad.player_choice_random_item_id_max[i]+1);
							else
								child_script.item_id = my_game_master.my_ads_master.current_ad.player_choice_consumable_item_id[i];
							//cap quantity
							if ((my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][child_script.item_id] + quantity) > my_game_master.my_store_item_manager.consumable_item_list[child_script.item_id].quantity_cap)
								quantity = my_game_master.my_store_item_manager.consumable_item_list[child_script.item_id].quantity_cap - my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][child_script.item_id];
						break;
						}

						child_script.quantity = quantity;
						if (quantity > 0)
							child_script.Start_me(i);
						else
							this.transform.GetChild(i).gameObject.SetActive(false);

						}
					else
						this.transform.GetChild(i).gameObject.SetActive(false);

					}
			break;

			}
			//if there is some gift to give, active the gift window
			if (gift_buttons_is_active > 0)
				gift_screen.SetActive(true);
		}
		else //special reward
		{
			Debug.Log(my_game_master.my_ads_master.current_ad.my_special_ad_reward);

			//setup window appearance:
				//deactivate all buttons
				for (int i = 0; i < child_buttons; i++)
					this.transform.GetChild(i).gameObject.SetActive(false);
				//deactivate the single reward ico
				single_reward.SetActive(false);

				window_text_item_name.text = "";
				window_text_item_description.text = "";
				my_window_rect.sizeDelta = new Vector2(my_window_rect.rect.width,small_height);

				if(buy_button)
				{
				if (my_game_master.my_ads_master.buy_button_cost > 0)
						{
						buy_button.SetActive(true);
						buy_button_text.text = my_game_master.my_ads_master.buy_button_cost.ToString("N0");
						
						if (my_game_master.my_ads_master.buy_button_cost <= my_game_master.current_virtual_money[my_game_master.current_profile_selected])//you have enough money
							buy_button.GetComponent<Image>().sprite = buy_button_on;
						else//you can't effort this purchase
							buy_button.GetComponent<Image>().sprite = buy_button_off;
						}
					else
						buy_button.SetActive(false);
				}

			gift_screen.SetActive(true);
		}

		Check_internet();

	}

	void Check_internet()
	{
		Debug.Log("Check_internet()");
		if (gift_screen.activeSelf)
		{
			Debug.Log("gift_screen.activeSelf");
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork && my_game_master.my_ads_master.Advertisement_isInitialized())
				{
				//ads_button_ico.sprite = ads_button_internet_on;
				}
			else
				{
				ads_button_ico.sprite = ads_button_internet_off;//if ads can't work because no internet connection
				//Invoke("Check_internet",1);
				}
		}
	}

	void Single_reward_setup(Sprite ico, int quantity)
	{
		if (quantity > 0)
			{
			//deactivate all buttons
			for (int i = 0; i < child_buttons; i++)
				this.transform.GetChild(i).gameObject.SetActive(false);
			//activate only a the ico that show the reward
			single_reward_ico.sprite = ico;

			single_reward.SetActive(true);
			if (quantity > 1)
			{
				single_reward_quantity.gameObject.SetActive(true);
				single_reward_quantity.text = quantity.ToString("N0");
			}
			else
				single_reward_quantity.gameObject.SetActive(false);
			
			gift_buttons_is_active++;
			}
		else
			{
			Close_me();
			}
	}
		
	public void Select_this_button(int button_id)
	{
		if (selected_button >= 0 && selected_button != button_id)
			my_game_master.Gui_sfx(my_game_master.tap_sfx);

		for (int i = 0; i < child_buttons; i++)
		{
			if (i ==  button_id)
				{
				selected_button = i;
				this.transform.GetChild(i).GetComponent<Image>().sprite = select_button_sprite;

				gift_button child_script = this.transform.GetChild(i).GetComponent<gift_button>();
					my_game_master.my_ads_master.current_quantity_reward_selected = child_script.quantity;
					my_game_master.my_ads_master.current_item_id_reward_selected = child_script.item_id;
					my_game_master.my_ads_master.current_reward_selected = child_script.give_this_selected;

				//update item description
				switch(my_game_master.my_ads_master.current_reward_selected)
					{
					case ads_master.ad_reward.virtual_money:
						window_text_item_name.text = my_game_master.virtual_money_name;
						window_text_item_description.text = "";//my_game_master.current_quantity_reward_selected.ToString("N0");

					break;

					case ads_master.ad_reward.new_live:
						window_text_item_name.text = my_game_master.lives_name;
						window_text_item_description.text = "";// my_game_master.current_quantity_reward_selected.ToString("N0");
					break;

					case ads_master.ad_reward.consumable_item:
						window_text_item_name.text= my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].name;
						window_text_item_description.text = my_game_master.my_store_item_manager.consumable_item_list[my_game_master.my_ads_master.current_item_id_reward_selected].description;
					break;
					}
				}
			else
				this.transform.GetChild(i).GetComponent<Image>().sprite = not_select_button_sprite;
		}
	}

	public void Watch_the_video_ad()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork && my_game_master.my_ads_master.Advertisement_isInitialized()) 
			{
			my_game_master.Gui_sfx(my_game_master.tap_sfx);

			if (my_game_master.my_ads_master.current_ad == my_game_master.my_ads_master.ads_just_after_logo_when_game_start_as_daily_reward)
				my_game_master.my_ads_master.Set_app_start_ad_countdown();


			//close gift window
			gift_screen.SetActive(false);
			my_game_master.a_window_is_open = false;
			Debug.Log("Watch_the_video_ad() " + Time.timeScale + currentTimeScale);
			Time.timeScale = currentTimeScale; 
			if (my_game_master.use_pad)
				EventSystem_obj.SetActive(true);

			//star ad
			my_game_master.my_ads_master.Show_ad(true);//true = rewarded
			}
		else
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
	}

	public void Pay_instead_of_watch()
	{
		if (my_game_master.my_ads_master.buy_button_cost <= my_game_master.current_virtual_money[my_game_master.current_profile_selected])
		{
			//you have enough money
			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			//pay
			my_game_uGUI.Update_virtual_money(-my_game_master.my_ads_master.buy_button_cost);
			//gain
			my_game_master.my_ads_master.Give_reward();
			//close gift window
			gift_screen.SetActive(false);
			my_game_master.a_window_is_open = false;
			Time.timeScale = currentTimeScale; 
			if (my_game_master.use_pad)
				EventSystem_obj.SetActive(true);

		}
		else//you can't effort this purchase
		{
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
		}
	}

	public void Close_me()
	{		

		//end pause
		Time.timeScale = currentTimeScale; 

		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_game_master.my_ads_master.Reset_reward();
		my_game_master.a_window_is_open = false;
		if (my_game_master.use_pad)
			EventSystem_obj.SetActive(true);
		gift_screen.SetActive(false);
	}

	//pad input
	void Update()
	{
		if (my_game_master.use_pad)
		{
			if (Input.GetKeyDown(my_game_master.pad_next_button))
				Next();
			else if (Input.GetKeyDown(my_game_master.pad_previous_button))
				Previous();
			
			if (Input.GetButtonDown("Submit"))
				Watch_the_video_ad();
			else if (Input.GetKeyDown(my_game_master.pad_back_button))
				Close_me();
		}
		
		if (Input.GetKeyDown (KeyCode.Escape) && my_game_master.allow_ESC)
			Close_me();
		
		
	}
	
	void Next()
	{
		if (selected_button+1 < child_buttons)
			Select_this_button(selected_button+1);
		else
			Select_this_button(0);
		
		
	}
	
	void Previous()
	{
		if (selected_button > 0)
			Select_this_button(selected_button-1);
		else
			Select_this_button(child_buttons-1);
	}

}
