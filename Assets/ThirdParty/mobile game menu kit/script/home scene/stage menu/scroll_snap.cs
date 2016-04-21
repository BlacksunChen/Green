using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scroll_snap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler{

	float snap_sensibility = 0.5f;
	public ScrollRect my_scrollRect;
	int how_many_elements;
	public RectTransform container;

	float[] useful_positions;
	float a_step;
	float last_position;
	int target_position;

	bool drag_started;

	public manage_menu_uGUI my_manage_menu_uGUI;
	game_master my_game_master;

	void Start()
	{
		if (game_master.game_master_obj)
			my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
	}

	public void Start_me () {
	
		how_many_elements = container.childCount;
		useful_positions = new float[how_many_elements];
		target_position = 0;

		if (how_many_elements > 2)
			a_step = 1.0f/(how_many_elements-1.0f);

			for (int i = 0; i < how_many_elements; i++)
				{
				if (i == 0)
					useful_positions[i] = 0;
				else if (i == how_many_elements-1)
					useful_positions[i] = 1;
				else
					{
					useful_positions[i] = a_step*i;
					}
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
		if (target_position < how_many_elements-1)
		{
			target_position++;
			my_manage_menu_uGUI.Update_page_dot(target_position);
			StartCoroutine(Move_to(my_scrollRect.normalizedPosition,new Vector2(useful_positions[target_position],my_scrollRect.normalizedPosition.y) ,my_scrollRect.elasticity));
			my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.first_stage_ico_in_this_page[target_position]);
		}
	}

	void Previous()
	{
		if (target_position > 0)
		{
			target_position--;
			my_manage_menu_uGUI.Update_page_dot(target_position);
			StartCoroutine(Move_to(my_scrollRect.normalizedPosition,new Vector2(useful_positions[target_position],my_scrollRect.normalizedPosition.y) ,my_scrollRect.elasticity));
			my_manage_menu_uGUI.Mark_this_button(my_manage_menu_uGUI.first_stage_ico_in_this_page[target_position]);
		}
	}



	public void OnBeginDrag(PointerEventData data)
	{
		last_position = my_scrollRect.horizontalNormalizedPosition;
	}

	public void OnDrag(PointerEventData data)
	{
		drag_started = true;
		if (my_scrollRect.horizontalNormalizedPosition >= useful_positions[target_position]+(a_step*snap_sensibility))
			{
			if (target_position < how_many_elements-1)
				target_position++;
			else
				target_position = how_many_elements-1;

			my_manage_menu_uGUI.Update_page_dot(target_position);
			}
		else if (my_scrollRect.horizontalNormalizedPosition < useful_positions[target_position]-(a_step*snap_sensibility))
			{
			if (target_position > 0)
				target_position--;
			else 
				target_position = 0;

			my_manage_menu_uGUI.Update_page_dot(target_position);
			}

	}

	public void OnEndDrag(PointerEventData data)
	{
		if (drag_started)
			{
			if ( Mathf.Abs(last_position-my_scrollRect.horizontalNormalizedPosition) > a_step*snap_sensibility)
				{
				//go to new position
				my_scrollRect.horizontal = false;
				StartCoroutine(Move_to(my_scrollRect.normalizedPosition,new Vector2(useful_positions[target_position],my_scrollRect.normalizedPosition.y) ,my_scrollRect.elasticity));

				}
			else
				{
				//return to last position
				my_scrollRect.horizontal = false;
				StartCoroutine(Move_to(my_scrollRect.normalizedPosition,new Vector2(last_position,my_scrollRect.normalizedPosition.y) ,my_scrollRect.elasticity));
				}
			}
	}

	IEnumerator Move_to(Vector2 start, Vector2 end, float time_seconds)
	{
		float t = 0.0f;
		while (t <= 1.0f) 
		{
			t += Time.deltaTime/time_seconds;
			my_scrollRect.normalizedPosition = Vector2.Lerp(start, end, Mathf.SmoothStep(0.0f, 1.0f, t));
			yield return new WaitForSeconds(0.03f);
		}
		my_scrollRect.horizontal = true;
		drag_started = false;
	}
}
