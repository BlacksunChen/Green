using UnityEngine;
using System.Collections;

public class store_item_manager: MonoBehaviour {
	
	
	[System.Serializable]
	public class incremental_item
	{

		public string name;
		public Sprite[] icon;
		public float[] price;
		public string[] description;
		public bool require_real_money;
		public int avaible_from_world;
		public int avaible_from_stage;

		public int max_level;

	}

	[System.Serializable]
	public class consumable_item
	{
		
		public string name;
		public Sprite icon;
		public float price;
		public string description;
		public bool require_real_money;
		public int avaible_from_world;
		public int avaible_from_stage;

		public int quantity_cap;//how many of it the player can accumulate
		
	}

	public incremental_item[] incremental_item_list;
	public consumable_item[] consumable_item_list;
}
