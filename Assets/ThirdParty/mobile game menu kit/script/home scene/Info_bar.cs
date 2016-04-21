using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Info_bar : MonoBehaviour {

	public Transform my_stars;
	public Transform my_lives;
	public Transform my_tokens;
	public Transform my_virtual_money;

	game_master my_game_master;
	public manage_menu_uGUI my_manage_menu_uGUI;
	public store_tabs my_store_tabs;

	// Use this for initialization
	void Start () {
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
	}
	
	public void Show_info_bar(bool show)
	{
		if (show)
			{
			this.gameObject.SetActive(true);

			if (my_game_master == null)
				{
				if (game_master.game_master_obj)
					my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
				}

			//show star score
			if (my_game_master.show_star_score)
				{
				my_stars.gameObject.SetActive(true);
				my_stars.GetChild(0).GetComponent<Text>().text = my_game_master.stars_total_score[my_game_master.current_profile_selected].ToString("N0");
				}
			else
				my_stars.gameObject.SetActive(false);

			//show lives
			if (my_game_master.infinite_lives)
				{
				my_lives.gameObject.SetActive(false);
				my_tokens.gameObject.SetActive(false);
				}
			else
				{
				my_lives.gameObject.SetActive(true);
				my_lives.GetChild(0).GetComponent<Text>().text = my_game_master.current_lives[my_game_master.current_profile_selected].ToString();

				if ((my_game_master.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token) && (!my_game_master.my_ads_master.ads_when_continue_screen_appear.this_ad_is_enabled))
					{
					my_tokens.gameObject.SetActive(true);
					my_tokens.GetChild(0).GetComponent<Text>().text = my_game_master.current_continue_tokens[my_game_master.current_profile_selected].ToString();
					}
				else
					my_tokens.gameObject.SetActive(false);
				}

			//show virtual money
			if (my_game_master.store_enabled)
				{
				my_virtual_money.gameObject.SetActive(true);
				my_virtual_money.GetChild(0).GetComponent<Text>().text = my_game_master.current_virtual_money[my_game_master.current_profile_selected].ToString("N0");
				if (my_game_master.can_buy_virtual_money_with_real_money)
					my_virtual_money.GetChild(1).gameObject.SetActive(true);
				else
					my_virtual_money.GetChild(1).gameObject.SetActive(false);
				}
			else
				my_virtual_money.gameObject.SetActive(false);
			}
		else
			{
			this.gameObject.SetActive(false);
			}
	}

	public void Update_me()
	{
		if (my_game_master== null)
		{
			if (game_master.game_master_obj)
				my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
		}

		if (my_game_master.show_star_score)
			my_stars.GetChild(0).GetComponent<Text>().text = my_game_master.stars_total_score[my_game_master.current_profile_selected].ToString();

		if (!my_game_master.infinite_lives)
			{
			my_lives.GetChild(0).GetComponent<Text>().text = my_game_master.current_lives[my_game_master.current_profile_selected].ToString();
			if ((my_game_master.continue_rule_selected == game_master.continue_rule.continue_cost_a_continue_token)&& (!my_game_master.my_ads_master.ads_when_continue_screen_appear.this_ad_is_enabled))
				my_tokens.GetChild(0).GetComponent<Text>().text = my_game_master.current_continue_tokens[my_game_master.current_profile_selected].ToString();
			}
		my_virtual_money.GetChild(0).GetComponent<Text>().text = my_game_master.current_virtual_money[my_game_master.current_profile_selected].ToString("N0");
	}

	public void Buy_virtual_money_with_real_money()
	{
		//open store screen if not open yet
		if (my_manage_menu_uGUI.current_screen != my_manage_menu_uGUI.store_screen)
			{
			my_manage_menu_uGUI.Go_to_store_screen(3);
			}
		else 
			my_store_tabs.Select_this_tab(3);

	}
}
