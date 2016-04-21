using UnityEngine;
using System.Collections;

public class call_ad_gift : MonoBehaviour {

	game_master my_game_master;


	// Use this for initialization
	void Start () {
	
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

	}

	void OnMouseDown ()
	{
		if (!game_uGUI.in_pause)
		{
			my_game_master.my_ads_master.Call_ad(my_game_master.my_ads_master.ads_when_player_open_a_gift_packet);
		}
	}
	

}
