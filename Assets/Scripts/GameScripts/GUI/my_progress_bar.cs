using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Green
{
    public class my_progress_bar : MonoBehaviour
    {
        game_uGUI my_game_uGUI;

        public Sprite star_off;
        public Sprite star_on;
        public Image[] star_sprites;

        public int[] star_target_values;

        public Slider main_slider;
        public Slider[] star_slider;


        void Awake()
        {
            my_game_uGUI = GameObject.FindObjectOfType<game_uGUI>();

            for (int i = 0; i < 3; i++)
            {
                //reset sprites
                star_sprites[i].sprite = star_off;
                //set stars positions
                star_slider[i].maxValue = star_target_values[2];
                star_slider[i].value = star_target_values[i];
                //reset fill
                main_slider.maxValue = star_target_values[2];
                main_slider.value = 0;
            }

        }

        public void Update_fill(int new_current_value)
        {
            main_slider.value = new_current_value;

            int star_count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (new_current_value >= star_target_values[i])
                {
                    star_sprites[i].sprite = star_on;
                    star_count++;
                }
                else
                    star_sprites[i].sprite = star_off;
            }
            if (my_game_uGUI)
                my_game_uGUI.New_star_score(star_count);

        }

        public void Add_to_fill(int quantity)
        {
            main_slider.value += quantity;

            int star_count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (main_slider.value >= star_target_values[i])
                {
                    star_sprites[i].sprite = star_on;
                    star_count++;
                }
                else
                    star_sprites[i].sprite = star_off;
            }
        }
    }
}