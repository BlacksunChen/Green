using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class store_tabs : MonoBehaviour {

	[HideInInspector]public int tab_selected = -1;
	public GameObject[] windows;
	public Image[] tab_buttons_ico;
	public store_manager[] my_store_manager;
	int tabs_length;

	public Sprite selected_tab;
	public Sprite not_selected_tab;

	public GameObject buy_money_tab;
	game_master my_game_master;
	public manage_menu_uGUI my_manage_menu_uGUI;

	// Use this for initialization
	void Start () {

		if (game_master.game_master_obj)
			{
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
			buy_money_tab.SetActive(my_game_master.can_buy_virtual_money_with_real_money);
			}

		tabs_length = 0;
		for (int i = 0; i < this.transform.childCount; i++)
			{
			if (this.transform.GetChild(i).gameObject.activeSelf)
					tabs_length++;
			}

	}

	void Update()
	{
		if (my_game_master.use_pad)
		{
			if (Input.GetKeyDown(my_game_master.pad_next_button))
				Next();
			else if (Input.GetKeyDown(my_game_master.pad_previous_button))
				Previous();

		}

	}
	
	void Next()
	{
		if (tab_selected+1 < tabs_length)
			Select_this_tab(tab_selected+1);
		else
			Select_this_tab(0);
		
		
	}
	
	void Previous()
	{
		if (tab_selected > 0)
			Select_this_tab(tab_selected-1);
		else
			Select_this_tab(tabs_length-1);
	}
	
	public void Select_this_tab(int tab_id)
	{

		if (tab_selected != tab_id)
			{
			if (tab_selected >= 0)
				my_game_master.Gui_sfx(my_game_master.tap_sfx);

			for (int i = 0; i < tabs_length; i++)
				{
				windows[i].SetActive(false);
				tab_buttons_ico[i].sprite = not_selected_tab;
				}

			windows[tab_id].SetActive(true);
			tab_buttons_ico[tab_id].sprite = selected_tab;
			tab_selected = tab_id;

			if (!my_game_master)
				my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

			if (my_game_master.use_pad)
				Invoke("Focus_on_first_button_of_this_window",0.1f);

			}
	}

	void Focus_on_first_button_of_this_window()
	{
		GameObject target = windows[tab_selected].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
		if (target)
			my_manage_menu_uGUI.Mark_this_button(target);
	}

	public void Update_buttons_in_windows()
	{
		for (int i = 0; i < my_store_manager.Length; i++)
			my_store_manager[i].Update_buttons();
	}
}
