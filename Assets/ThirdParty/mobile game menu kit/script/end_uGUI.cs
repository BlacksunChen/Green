using UnityEngine;
using System.Collections;

public class end_uGUI : MonoBehaviour {

	game_master my_game_master;

	void Start () {
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
		if (my_game_master.allow_ESC)
			{
			Load_home();
			}
		}

		if (Input.GetKeyDown(my_game_master.pad_back_button) && my_game_master.use_pad)
			Load_home();
	}

	public void Load_home()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		//my_game_master.Unlink_me_to_camera();
		my_game_master.go_to_this_screen = game_master.this_screen.home_screen;
		Application.LoadLevel (my_game_master.home_scene_name); 
	}
	

}
