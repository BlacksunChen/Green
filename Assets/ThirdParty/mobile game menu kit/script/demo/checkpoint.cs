using UnityEngine;
using System.Collections;

public class checkpoint : MonoBehaviour {


	public Transform target_position;
	public enum activation_cost
	{
		free,
		//virtual_money,
		unity_ad,
		virtual_money_or_unity_ad

	}
	public activation_cost activation_cost_selected;
	public int virtual_money_cost;

	public Color enabled_color;
	//Color base_color;
	bool checkpoint_enabled;
	bool checkpoint_triggered;

	game_uGUI my_game_uGUI;
	game_master my_game_master;
	public demo_controller my_demo_controller;
	
	// Use this for initialization
	void Start () {

		if (game_master.game_master_obj)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			//selet the rule
			if (virtual_money_cost <= 0)
				{
				if (my_game_master.my_ads_master.ads_when_reach_a_check_point.this_ad_is_enabled)
					activation_cost_selected = activation_cost.unity_ad;
				else
					activation_cost_selected = activation_cost.free;
				}
			else //can use money to activate the checkpoint
				{
				if (my_game_master.my_ads_master.ads_when_reach_a_check_point.this_ad_is_enabled)
					activation_cost_selected = activation_cost.virtual_money_or_unity_ad;
				//else
					//activation_cost_selected = activation_cost.virtual_money;
				}
			}

		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();
		checkpoint_triggered = false;
		checkpoint_enabled = false;
		//base_color = GetComponent<Renderer>().material.color;


	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && !checkpoint_enabled && !checkpoint_triggered)
		{
			checkpoint_triggered = true;

			if (my_game_uGUI.show_debug_messages)
				Debug.Log("checkpoint: " + activation_cost_selected);

			switch (activation_cost_selected)
			{
			case activation_cost.free:
					Enable_this_checkpoint();
				break;

			case activation_cost.unity_ad:
					my_game_master.my_ads_master.buy_button_cost = 0;
					my_game_master.my_ads_master.target_checkpoint = this;
					my_game_master.my_ads_master.Call_ad(my_game_master.my_ads_master.ads_when_reach_a_check_point);
				break;

			//case activation_cost.virtual_money:
				//break;

			case activation_cost.virtual_money_or_unity_ad:
					my_game_master.my_ads_master.buy_button_cost = virtual_money_cost;
					my_game_master.my_ads_master.target_checkpoint = this;
					my_game_master.my_ads_master.Call_ad(my_game_master.my_ads_master.ads_when_reach_a_check_point);
				break;
			}
		}
	}

	public void Enable_this_checkpoint()
	{
		checkpoint_enabled = true;
		GetComponent<Renderer>().material.color = enabled_color;

		//update restart point
		my_demo_controller.start_player_position = target_position.position;
		my_demo_controller.restart_from_checkpoint = true;
	}
}
