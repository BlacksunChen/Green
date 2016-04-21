using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class item_demo : MonoBehaviour {

	public int item_id;
	public Image my_ico;
	public Text my_quantity;

	public game_master my_game_master;

	// Use this for initialization
	void Start () {

		if (game_master.game_master_obj)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			my_ico.sprite = my_game_master.my_store_item_manager.consumable_item_list[item_id].icon;
			my_quantity.text = my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][item_id].ToString("N0");
			}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (game_master.game_master_obj)
			my_quantity.text = my_game_master.consumable_item_current_quantity[my_game_master.current_profile_selected][item_id].ToString("N0");

	}
}
