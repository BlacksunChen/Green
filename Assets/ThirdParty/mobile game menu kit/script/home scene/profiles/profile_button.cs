using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class profile_button : MonoBehaviour {

	public int profile_number;
	public Text profile_number_text;

	public Text profile_name;
	public Text profile_progress;
	public Text lives;
	public GameObject star_ico;
	public Text stars;

	public GameObject profile_on;
	public GameObject profile_off;

	public GameObject lives_ico;

	public Image profile_on_image;
	public Sprite profile_on_sprite;
	public Sprite profile_selected_sprite;

	public profile_manager my_profile_manager;
	public game_master my_game_master;

	// Use this for initialization
	void Start () {
	
	}

	public void Start_me (int my_number) {
		profile_number = my_number;
		profile_number_text.text = (profile_number+1).ToString();

	}

	public void Set_on(bool selected, string my_name, string my_progress, int my_lives, int my_star_score)
	{
		Show_selection(selected);
		//Debug.Log(my_name);
		if (my_name != "")
			profile_name.text = my_name;
		else
			profile_name.text = "Profile " + profile_number.ToString();

		profile_progress.text = my_progress;

		if (!my_game_master.infinite_lives)
			{
			lives_ico.SetActive(true);
			lives.text = my_lives.ToString();
			}
		else
			{
			lives_ico.SetActive(false);
			}

		if (my_game_master.show_star_score)
			stars.text = my_star_score.ToString();
		else
			{
			star_ico.SetActive(false);
			}

		profile_on.SetActive(true);
		profile_off.SetActive(false);

		if (profile_number==0)
			{
			my_profile_manager.my_manage_menu_uGUI.Mark_this_button(profile_on);
			}
	}

	public void Set_off()
	{
		profile_on.SetActive(false);
		profile_off.SetActive(true);

		if (profile_number==0)
			{
			my_profile_manager.my_manage_menu_uGUI.Mark_this_button(profile_off);
			}

	}

	public void Show_selection(bool selected)
	{
		if (selected)
			profile_on_image.sprite = profile_selected_sprite;
		else
			profile_on_image.sprite = profile_on_sprite;
	}

	public void Click_on()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_profile_manager.Select_this_profile(profile_number);
	}

	public void Click_off()
	{
		my_game_master.Gui_sfx(my_game_master.tap_sfx);
		my_profile_manager.Create_new_profile(profile_number,true);
	}
	

}
