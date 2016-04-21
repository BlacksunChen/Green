using UnityEngine;
using System.Collections;

public class demo_controller : MonoBehaviour {

	public GameObject player;
	[HideInInspector] public Vector3 start_player_position;
	public GameObject[] stars;
	public GameObject[] money;
	[HideInInspector] public bool restart_from_checkpoint;
	game_uGUI my_game_uGUI;

	void Start()
	{
		start_player_position = player.transform.position;
		my_game_uGUI = GameObject.FindGameObjectWithTag("_gui_").GetComponent<game_uGUI>();

	}

	void Update()
	{
		if (my_game_uGUI.restarting)
			Restart();
	}

	public void Restart()
	{
		//reset palyer position
		player.transform.position = start_player_position;

		if (!restart_from_checkpoint)
			{
			//reset stars
			for (int i = 0; i < stars.Length; i++)
				stars[i].SetActive(true);

			//reset money
			for (int i = 0; i < money.Length; i++)
				money[i].SetActive(true);
			}

		my_game_uGUI.restarting = false;
	}
}
