using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class store_button : MonoBehaviour {


	public string my_name;
	public Sprite my_ico;

	public float my_price;
	public enum price_currency
	{
		virtual_money, 
		real_money 
	}
	public price_currency price_currency_selected;

	public enum give_this
	{
		virtual_money, 
		new_live,
		unlock_world,
		continue_token,
		incremental_item,
		consumable_item
	}
	public give_this give_this_selected;
	public int quantity;
	public bool show_quantity;
	public bool disable_me_after_purchased;
	bool purchased;
	public int my_item_ID;//for consumable and incremental items

	bool you_have_enough_money;
	bool this_buy_hit_the_cap;//so disable it

	public Sprite can_buy_ico;
	public Sprite cant_buy_ico;
	public Sprite virtual_money_ico;
	public Sprite real_money_ico;

	[HideInInspector]public Text my_name_tx;
	[HideInInspector]public Text my_price_tx;
	public Text my_quantity_tx;
	public Text my_buy_tx;
	[HideInInspector]public Image my_ico_img;
	[HideInInspector]public Image my_buy_ico_img;
	[HideInInspector]public Image my_money_ico_img;


	[HideInInspector] public game_master my_game_master;
	public manage_menu_uGUI my_manage_menu_uGUI;
	[HideInInspector]public store_tabs my_store_tabs;
	[HideInInspector]public feedback_window my_feedback_window;

	// Use this for initialization
	void Start () {
		My_start();
	}

	public void My_start()
	{
		if (game_master.game_master_obj && my_game_master == null)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");


		if (Check_if_show_this_button())
			{
			Debug.Log(give_this_selected + " id " + my_item_ID);
			my_name_tx.text = my_name;
			my_ico_img.sprite = my_ico;

			if (price_currency_selected == price_currency.real_money)
				{
				my_money_ico_img.sprite = real_money_ico;
				you_have_enough_money = true;
				}
			else if (price_currency_selected == price_currency.virtual_money)
				{
				my_money_ico_img.sprite = virtual_money_ico;
				Check_if_you_have_enough_virtual_money();
				}

			my_price_tx.text = my_price.ToString();

			Check_if_this_purchase_dont_hit_the_cap();

			Show_quantity();
			Show_buy_ico();

			}
		else
			this.gameObject.SetActive(false);

	}

	void Show_buy_ico()
	{
		if (!this_buy_hit_the_cap && you_have_enough_money)
			my_buy_ico_img.sprite = can_buy_ico;
		else
			my_buy_ico_img.sprite = cant_buy_ico;
	}

	public void Incremental_item_MAX()
	{
		Debug.Log("Incremental_item_MAX()");
		my_quantity_tx.gameObject.SetActive(true);
		my_quantity_tx.text = "MAX";
		my_buy_tx.text = "MAX";
		my_ico = my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].icon[my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].icon.Length-1];
		my_price_tx.gameObject.SetActive(false);
		this_buy_hit_the_cap = true;

	}

	void Show_quantity()
	{
		if (show_quantity && quantity > 1)
		{
			my_quantity_tx.gameObject.SetActive(true);
			my_quantity_tx.text = quantity.ToString();
		}
		else
			my_quantity_tx.gameObject.SetActive(false);
		
		if (this_buy_hit_the_cap)
			{
			my_buy_tx.text = "MAX";
			if (give_this_selected == give_this.incremental_item)
				my_quantity_tx.gameObject.SetActive(true);
			}
		else
			my_buy_tx.text = "Buy";
	}

	void Check_if_this_purchase_dont_hit_the_cap()
	{
		switch(give_this_selected)
		{
		case give_this.virtual_money:
			if ((my_game_master.current_virtual_money[my_game_master.current_profile_selected] + quantity) > my_game_master.virtual_money_cap)
				this_buy_hit_the_cap = true;
			else
				this_buy_hit_the_cap = false;
			break;

		case give_this.new_live:
			if ((my_game_master.current_lives[my_game_master.current_profile_selected] + quantity) > my_game_master.live_cap)
				this_buy_hit_the_cap = true;
			else
				this_buy_hit_the_cap = false;
			break;

		case give_this.continue_token:
			if ((my_game_master.current_continue_tokens[my_game_master.current_profile_selected] + quantity) > my_game_master.continue_tokens_cap)
				this_buy_hit_the_cap = true;
			else
				this_buy_hit_the_cap = false;
			break;

		case give_this.consumable_item:
			if ((my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][my_item_ID] + quantity) > my_game_master.my_store_item_manager.consumable_item_list[my_item_ID].quantity_cap)
				this_buy_hit_the_cap = true;
			else
				this_buy_hit_the_cap = false;
			break;

		case give_this.incremental_item:
			if (my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][my_item_ID] >= my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].max_level)
				this_buy_hit_the_cap = true;
			else
				this_buy_hit_the_cap = false;
			break;

		}
	}

	bool Check_if_show_this_button()
	{
		bool my_check = true;

		switch(give_this_selected)
		{
		case give_this.virtual_money:
			if ((quantity > my_game_master.virtual_money_cap)
				|| (!my_game_master.show_virtual_money_even_if_cap_reached && (my_game_master.current_virtual_money[my_game_master.current_profile_selected] + quantity > my_game_master.virtual_money_cap)))
				{
				this.gameObject.SetActive(false);
				my_check = false;
				}

			break;

		case give_this.new_live://check if you risk to hit the live cap
			if ((my_game_master.infinite_lives)
				||((quantity > my_game_master.live_cap)
				   || (!my_game_master.show_lives_even_if_cap_reached && (my_game_master.current_lives[my_game_master.current_profile_selected] + quantity > my_game_master.live_cap))))
				{
				this.gameObject.SetActive(false);
				my_check = false;
				}

			break;

		case give_this.unlock_world: //this button will be disable afther purchase
			if (my_game_master.this_world_is_unlocked_after_selected[quantity] == game_master.this_world_is_unlocked_after.buy_it)
			{
				disable_me_after_purchased = true;
				if (my_game_master.world_purchased[my_game_master.current_profile_selected][quantity])
				{
					this.gameObject.SetActive(false);
					purchased = true;
					my_check = false;
				}
			}
			else 
				my_check = false;
			break;

		case give_this.continue_token:
			if (my_game_master.infinite_lives || (my_game_master.continue_rule_selected != game_master.continue_rule.continue_cost_a_continue_token) || (my_game_master.my_ads_master.ads_when_continue_screen_appear.this_ad_is_enabled) 
				|| (quantity > my_game_master.continue_tokens_cap)
				    || (!my_game_master.show_continue_tokens_even_if_cap_reached && (my_game_master.current_continue_tokens[my_game_master.current_profile_selected] + quantity > my_game_master.continue_tokens_cap)))
			{
				this.gameObject.SetActive(false);
				my_check = false;
			}

			break;

		case give_this.incremental_item:
			if 	(!my_game_master.show_incremental_item_even_if_cap_reached 
			&& (my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][my_item_ID] >= my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].max_level))
			{
				this.gameObject.SetActive(false);
				my_check = false;
			}
			break;

		case give_this.consumable_item:
			if 	(!my_game_master.show_consumable_item_even_if_cap_reached 
				&& (my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][my_item_ID] >= my_game_master.my_store_item_manager.consumable_item_list[my_item_ID].quantity_cap))
			{
				this.gameObject.SetActive(false);
				my_check = false;
			}
			break;
		}

		return my_check;
	}

	void Check_if_you_have_enough_virtual_money()
	{
		if (my_price > my_game_master.current_virtual_money[my_game_master.current_profile_selected])
		{
			you_have_enough_money = false;
		}
		else
		{
			you_have_enough_money = true;
		}
	}

	public void Click_me () {
		if (you_have_enough_money && !this_buy_hit_the_cap)
			{

			my_game_master.Gui_sfx(my_game_master.tap_sfx);
			if (price_currency_selected == price_currency.real_money)
				Pay_with_real_money();
			else if (price_currency_selected == price_currency.virtual_money)
				Pay_with_virtual_money();
			}
		else
			{
			my_game_master.Gui_sfx(my_game_master.tap_error_sfx);
			}
	}

	void Pay_with_real_money()
	{
		if (my_game_master.show_debug_messages)
			Debug.Log("Pay_with_real_money");

		if(my_game_master.buy_virtual_money_with_real_money_with_soomla)
		{
			if (give_this_selected == give_this.virtual_money)
			{
				/* //DELETE THIS LINE FOR SOOMLA
				my_game_master.my_Soomla_billing_script.Buy_virutal_money_with_real_money(my_game_master.current_profile_selected,quantity);
				my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);

				if (my_game_master.show_purchase_feedback)
					my_feedback_window.Start_me(my_ico,quantity,my_name);
				
				my_store_tabs.Update_buttons_in_windows();
				purchased = true;
				*/ //DELETE THIS LINE FOR SOOMLA
			}
			else
			{
				if (my_game_master.show_debug_warnings)
					Debug.LogWarning("Soomla - You can buy with real money ONLY virtual money, not items or other stuff");
			}
		}
		else
		{
		//put here your code
		Give_the_stuff(); //call this when money operation is done
		}
	}

	void Pay_with_virtual_money()
	{
		if (my_game_master.show_debug_messages)
			Debug.Log("Pay_with_virtual_money");
		if(my_game_master.buy_virtual_money_with_real_money_with_soomla)
			{
			/* //DELETE THIS LINE FOR SOOMLA
			if (my_game_master.my_Soomla_billing_script.Buy_stuff_with_virtual_money(my_game_master.current_profile_selected,Mathf.RoundToInt(my_price)))
			    {
				my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
				Give_the_stuff();
				}
			else
				{
				if (my_game_master.show_debug_warnings)
					Debug.LogWarning("Soomla - pay fail");
				}
			*/ //DELETE THIS LINE FOR SOOMLA
			}
		else
			{
			my_game_master.current_virtual_money[my_game_master.current_profile_selected] -= Mathf.RoundToInt(my_price);
			Give_the_stuff();
			}
	}

	void Give_the_stuff()
	{
		Debug.Log("Give_the_stuff: " + give_this_selected);
		switch(give_this_selected)
			{
			case give_this.virtual_money:
				my_game_master.current_virtual_money[my_game_master.current_profile_selected] += quantity;

			//if (my_game_master.reward_feedback_after_ad)
				//my_feedback_window.Start_me(my_ico,quantity,my_game_master.virtual_money_name);
			break;

			case give_this.new_live:
				my_game_master.current_lives[my_game_master.current_profile_selected] += quantity;

				//if (my_game_master.reward_feedback_after_ad)
					//my_feedback_window.Start_me(my_ico,quantity,my_game_master.lives_name);
			break;

			case give_this.unlock_world:
				my_game_master.Unlock_this_world(quantity);
				my_game_master.world_purchased[my_game_master.current_profile_selected][quantity] = true;
				my_manage_menu_uGUI.Update_profile_name(true);//this update also world and stage screen to show the new world unlock
			break;

			case give_this.continue_token:
				my_game_master.current_continue_tokens[my_game_master.current_profile_selected] += quantity;
			break;

			case give_this.incremental_item:
				my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][my_item_ID]++;
			break;

			case give_this.consumable_item:
				my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][my_item_ID]++;
			break;
		}

		my_game_master.Save(my_game_master.current_profile_selected);

		if (my_game_master.show_purchase_feedback)
			my_feedback_window.Start_me(my_ico,quantity,my_name);

		my_store_tabs.Update_buttons_in_windows();
		purchased = true;
	}
	

	public void Update_me()
	{
		if (give_this_selected == give_this.incremental_item)
		{
			quantity = my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][my_item_ID]+1;
			my_quantity_tx.text = quantity.ToString();

			if (quantity > my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].max_level)
			{
				Incremental_item_MAX();
			}
			else
			{
				my_buy_tx.text = "Buy";

				if (my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].icon.Length > my_item_ID)
					my_ico = my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].icon[quantity-1];
				else
					my_ico = my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].icon[0];

				my_price = my_game_master.my_store_item_manager.incremental_item_list[my_item_ID].price[my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][my_item_ID]];
				my_price_tx.text = my_price.ToString();
			}
			my_ico_img.sprite = my_ico;
		}


		if (disable_me_after_purchased && purchased)
			{
			this.gameObject.SetActive(false);
			return;
			}

		if (price_currency_selected == price_currency.virtual_money)
			{
			Check_if_you_have_enough_virtual_money();
			Check_if_this_purchase_dont_hit_the_cap();
			}
		else
			Check_if_this_purchase_dont_hit_the_cap();

		Check_if_show_this_button();
		Show_quantity();
		Show_buy_ico();


	}
}
