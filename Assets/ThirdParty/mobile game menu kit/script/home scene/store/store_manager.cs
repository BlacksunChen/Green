using UnityEngine;
using System.Collections;

public class store_manager : MonoBehaviour {

	public enum store_tab
	{
		special,
		consumable_items,
		incremental_items,
		virtual_money
		
	}
	public store_tab my_tab;

	public Info_bar my_info_bar;
	store_button[] my_buttons;
	int total_buttons;


	public GameObject store_button_obj;
	bool store_button_generated;
	game_master my_game_master;
	public store_tabs my_store_tabs;
	public manage_menu_uGUI my_manage_menu_uGUI;
	public feedback_window my_feedback_window;

	// Use this for initialization
	void Start () {
	
		if (my_tab == store_tab.consumable_items)
			Generate_consumable_items();
		else if (my_tab == store_tab.incremental_items)
			Generate_incremental_items();

		total_buttons = this.transform.childCount;
		my_buttons = new store_button[total_buttons];
		for (int i = 0; i < total_buttons; i++)
			{
			my_buttons[i] = this.transform.GetChild(i).GetComponent<store_button>();
			my_buttons[i].my_store_tabs = my_store_tabs;
			my_buttons[i].my_manage_menu_uGUI = my_manage_menu_uGUI;
			my_buttons[i].my_feedback_window = my_feedback_window;
			}

	}
	
	public void Update_buttons()
		{
		my_info_bar.Update_me();
		for (int i = 0; i < total_buttons; i++)
			{
			my_buttons[i].Update_me();
			}
		}

	void Generate_consumable_items()
	{
		if (game_master.game_master_obj && !store_button_generated)
		{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			for (int i = 0; i < my_game_master.my_store_item_manager.consumable_item_list.Length; i++)
			{
			//check if this item is avaible at this phase of the game
				if ((my_game_master.my_store_item_manager.consumable_item_list[i].avaible_from_world <= my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected]) //world
					&& (my_game_master.my_store_item_manager.consumable_item_list[i].avaible_from_stage <= my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected]))//stage
					{
					//if the player can buy more item like this...
					if  (my_game_master.show_consumable_item_even_if_cap_reached
						|| (my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][i] < my_game_master.my_store_item_manager.consumable_item_list[i].quantity_cap))
						{
						GameObject temp_button = (GameObject)Instantiate(store_button_obj);
						temp_button.transform.SetParent(this.transform,false);
						
						store_button button_script = temp_button.GetComponent<store_button>();

						button_script.my_item_ID = i;
						button_script.my_name = my_game_master.my_store_item_manager.consumable_item_list[i].name;

						button_script.my_ico = my_game_master.my_store_item_manager.consumable_item_list[i].icon;

						//set quantity
						button_script.quantity = 1;
						
						//set price
						button_script.my_price = my_game_master.my_store_item_manager.consumable_item_list[i].price;
						
						if (my_game_master.my_store_item_manager.consumable_item_list[i].require_real_money)
							button_script.price_currency_selected = global::store_button.price_currency.real_money;
						else
							button_script.price_currency_selected = global::store_button.price_currency.virtual_money;
					


						button_script.show_quantity = false;
						button_script.give_this_selected = global::store_button.give_this.consumable_item;
						button_script.My_start();
						}
					}
			}
		}
	}

	void Generate_incremental_items()
		{
		if (game_master.game_master_obj && !store_button_generated)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			for (int i = 0; i < my_game_master.my_store_item_manager.incremental_item_list.Length; i++)
				{
				//check if this item is avaible at this phase of the game
				if ((my_game_master.my_store_item_manager.incremental_item_list[i].avaible_from_world <= my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected]) //world
				    && (my_game_master.my_store_item_manager.incremental_item_list[i].avaible_from_stage <= my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected]))//stage
					{
					GameObject temp_button = (GameObject)Instantiate(store_button_obj);
					temp_button.transform.SetParent(this.transform,false);
					
					store_button button_script = temp_button.GetComponent<store_button>();
					
					button_script.my_item_ID = i;
					button_script.my_name = my_game_master.my_store_item_manager.incremental_item_list[i].name;
					button_script.give_this_selected = global::store_button.give_this.incremental_item;

					button_script.my_game_master = my_game_master;

					//if this item not is at the maximum
					if (my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][i] < my_game_master.my_store_item_manager.incremental_item_list[i].max_level)
						{


						//set icon
						if (my_game_master.my_store_item_manager.incremental_item_list[i].icon.Length > i)
							button_script.my_ico = my_game_master.my_store_item_manager.incremental_item_list[i].icon[my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][i]];
						else
							button_script.my_ico = my_game_master.my_store_item_manager.incremental_item_list[i].icon[0];

						//set quantity

						//set price
						button_script.my_price = my_game_master.my_store_item_manager.incremental_item_list[i].price[my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][i]];
			
						if (my_game_master.my_store_item_manager.incremental_item_list[i].require_real_money)
							button_script.price_currency_selected = global::store_button.price_currency.real_money;
						else
							button_script.price_currency_selected = global::store_button.price_currency.virtual_money;

							//level
							button_script.show_quantity = true;
							button_script.quantity = my_game_master.incremental_item_current_level[my_game_master.current_profile_selected][i]+1;

						}
					else // max level
						{
						button_script.Incremental_item_MAX();
						}

					button_script.My_start();

					}
				}
			}

		store_button_generated = true;
		}


}
