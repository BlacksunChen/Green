using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class character_button : MonoBehaviour {

	public Text my_text;
	public new_profile_pad my_container;

	// Use this for initialization
	void Start () {
		my_text.text = this.gameObject.name;
	}
	
	public void Click_me()
	{
		my_container.Add_to_string(this.gameObject.name);
	}
}
