using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pad_scroll : MonoBehaviour {

	public ScrollRect my_scroll;
	public float speed;
	float x_position;
	game_master my_game_master;

	// Use this for initialization
	void Start () {
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
	}
	
	// Update is called once per frame
	void Update () {
		if (my_game_master.use_pad)
		{
			if (Input.GetKey(my_game_master.pad_next_button))
				Next();
			else if (Input.GetKey(my_game_master.pad_previous_button))
				Previous();
		}
	}

	void Next()
	{
		x_position += speed;
		x_position = Mathf.Clamp01(x_position);

		my_scroll.normalizedPosition = new Vector2(x_position,0);
	}
	
	void Previous()
	{
		x_position -= speed;
		x_position = Mathf.Clamp01(x_position);

		my_scroll.normalizedPosition = new Vector2(x_position,0);
	}
}
