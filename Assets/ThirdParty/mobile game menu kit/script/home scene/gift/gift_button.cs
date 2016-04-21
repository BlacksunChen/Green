using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gift_button : MonoBehaviour {


	[HideInInspector]public ads_master.ad_reward give_this_selected;
	[HideInInspector]public int item_id;
	
	public Text quantity_text;
	[HideInInspector]public int quantity;
	
	public Image my_ico;

	public gift_manager my_gift_manager;
	game_master my_game_master;
	[HideInInspector]public int button_id;

	public void Start_me (int my_id) {

		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

			button_id = my_id;

			my_gift_manager.gift_buttons_is_active++;
			this.gameObject.SetActive(true);

			//show ico
			switch(give_this_selected)
			{
			case ads_master.ad_reward.virtual_money:
				my_ico.sprite = my_gift_manager.virtual_money_ico;
				break;

			case ads_master.ad_reward.new_live:
				my_ico.sprite = my_gift_manager.live_ico;
				break;

			case ads_master.ad_reward.consumable_item:
					my_ico.sprite = my_game_master.my_store_item_manager.consumable_item_list[item_id].icon;
					break;
			}
			

			//decide if you need to show quantity
			if (quantity > 1)
				{
				quantity_text.gameObject.SetActive(true);
				quantity_text.text = quantity.ToString("N0");
				}
			else
				quantity_text.gameObject.SetActive(false);

		if (my_gift_manager.gift_buttons_is_active == 1)
			my_gift_manager.Select_this_button(my_id);

	}

}
