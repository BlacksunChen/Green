using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class feedback_window : MonoBehaviour {

	public Image my_ico;
	public Text my_quantity;
	public Text my_name;
	game_master my_game_master;

	public Sprite virtual_money_ico;
	public Sprite live_ico;

	public GameObject EventSystem_obj;

	float currentTimeScale;

	public void Start_me(Sprite ico, int quantity, string name)
	{
		//start pause
		currentTimeScale = 1;
		Time.timeScale = 0; 

		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");

		my_game_master.a_window_is_open = true;

		my_ico.sprite = ico;
		my_quantity.text = quantity.ToString("N0");
		my_name.text = name;
		this.gameObject.SetActive(true);

		if (my_game_master.use_pad)
			EventSystem_obj.SetActive(false);//in order to avoid pad input out the of the window

	}
	
	// Update is called once per frame
	void Update () {

		if(my_game_master.use_pad)
			{
			if ( (Input.GetButtonDown("Submit")) || (Input.GetKeyDown(my_game_master.pad_back_button)) )
				Close_me();
			}

		if (Input.GetKeyDown (KeyCode.Escape) && my_game_master.allow_ESC)
			Close_me();
	}

	public void Close_me()
	{
		//end pause
		Time.timeScale = currentTimeScale; 

		if (my_game_master.use_pad)
			EventSystem_obj.SetActive(true);//in order to avoid pad input out the of the window

		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_game_master.a_window_is_open = false;
		this.gameObject.SetActive(false);
	}
}
