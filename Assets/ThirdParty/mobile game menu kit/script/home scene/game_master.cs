using UnityEngine;
using System.Collections;
using System;

public class game_master : MonoBehaviour
{

    //editor
    public bool editor_show_worlds;
    public bool editor_show_lives;
    public bool editor_show_audio;
    public bool editor_show_debug;
    public bool show_debug_messages;
    public bool show_debug_warnings;
    public bool editor_show_pad;
    public bool editor_show_store;
    public bool editor_show_score;

    //device button work like back button in every screen, except home screen. In home screen it close the app (this behavior is REQUIRED for winphone store)
    public bool allow_ESC;
    public bool store_enabled;
    public bool show_loading_screen;

    public string home_scene_name;

    //score
    public string score_name;
    public string what_say_if_new_stage_record;
    public string what_say_if_new_personal_record;
    public string what_say_if_new_device_record;
    public bool show_star_score;
    public bool show_progres_bar;
    public bool show_int_score;
    public bool show_int_score_rank;
    public bool show_int_score_stage_record_in_game_stage;
    public bool show_score_in_lose_screen_too;

    //ads
    public ads_master my_ads_master;

    //store
    public bool show_purchase_feedback;
    public bool show_lives_even_if_cap_reached;
    public bool show_continue_tokens_even_if_cap_reached;
    public bool show_consumable_item_even_if_cap_reached;
    public bool show_incremental_item_even_if_cap_reached;
    public bool show_virtual_money_even_if_cap_reached;
    public string virtual_money_name;


    //pad input
    public bool use_pad;
    public Color normal_button_color;
    public Color highlighted_button_color;
    public KeyCode pad_start_button;
    public KeyCode pad_ok_button;
    public KeyCode pad_back_button;
    public KeyCode pad_next_button;
    public KeyCode pad_previous_button;
    public KeyCode pad_pause_button;

    //lives
    public bool infinite_lives;
    public string lives_name;
    public int start_lives;
    public int live_cap;
    public int[] current_lives;

    public enum lose_lives
    {
        in_game, //like platform games
        when_show_lose_screen //like puzzle games or 
    }

    public lose_lives lose_lives_selected;


    //continues
    public bool refresh_stage_and_world_screens;
    public int start_continue_tokens;
    public int[] current_continue_tokens;
    public int continue_tokens_cap;
    public bool continue_menu_have_countdown;
    public int continue_menu_countdown_seconds;

    public enum continue_rule
    {
        never_continue,
        infinite_continues,
        continue_cost_a_continue_token
        //continue_cost_virtual_money

    }

    public continue_rule continue_rule_selected;
    public int continue_cost_virtual_money;

    public enum if_player_not_continue
    {
        restart_from_W1_Stage_1,
        restart_from_current_world_Stage_1,
        restart_from_current_world_and_current_stage,
        restart_from_level_select_screen
    }

    public if_player_not_continue if_player_not_continue_selected;

    public enum when_restart
    {
        give_immediately_new_lives,
        give_lives_after_countdown

    }

    public when_restart when_restart_selected;
    public int if_not_continue_restart_with_lives;

    public bool if_not_continue_lose_gained_stars;
    public int wait_for_days;
    public int wait_for_hours;
    public int wait_for_minutes;
    public int wait_for_seconds;
    public DateTime[] target_time; //[profile]
    bool[] recharge_live_countdown_active; //[profile]

    public enum if_player_continue
    {
        restart_from_current_world_Stage_1,
        restart_from_current_world_and_current_stage,
        continue_playing_this_stage,
        restart_from_level_select_screen
    }

    public if_player_continue if_player_continue_selected;
    public int continue_give_new_lives;

    //virtual money

    public int start_virtual_money;
    public int virtual_money_cap;
    public int[] current_virtual_money; //[profile]
    public bool can_buy_virtual_money_with_real_money;
    public bool buy_virtual_money_with_real_money_with_soomla;
    public Soomla.Store.my_Soomla_billing my_Soomla_billing_script;
    public store_item_manager my_store_item_manager;
    public int[][] incremental_item_current_level; //[profile][item_array_slot]
    public int[][] consumable_item_current_quantity; //[profile][item_array_slot]

    //worlds and stages
    int max_stages_in_a_world; //this set the array capacity (it is set automatically, don't touch this!)
    public int[] current_world; //what is the last world played by the player 
    [SerializeField] public int[] total_stages_in_world_n; //how many game scenes has your game in every world
    public int total_number_of_stages_in_the_game;
    public int[] total_number_of_stages_in_the_game_solved;
    public string[] world_name;

    public enum this_world_is_unlocked_after
    {
        start,
        previous_world_is_finished,
        reach_this_star_score,
        buy_it
    }

    public this_world_is_unlocked_after[] this_world_is_unlocked_after_selected;
    [SerializeField] public int[] star_score_required_to_unlock_this_world; //[world]
    //for map
    public bool[][,] dot_tail_turn_on; //[profile][w,s];
    public int star_score_difference;
    public int[] latest_stage_played_world;
    public int[] latest_stage_played_stage;
    public int[] play_this_stage_to_progress_in_the_game_world;
    public int[] play_this_stage_to_progress_in_the_game_stage;

    //allow manual personalization
    public enum press_start_and_go_to
    {
        nested_world_stage_select_screen,
        map,
        single_screen_with_a_page_for_every_world,
        straight_to_the_next_game_stage,
        level_select_screen
    }

    public press_start_and_go_to press_start_and_go_to_selected = press_start_and_go_to.level_select_screen;

    public enum world_screen_generation
    {
        automatic,
        manual
    }

    public world_screen_generation world_screen_generation_selected = world_screen_generation.automatic;
    public bool show_world_name_on_world_ico;
    public bool show_world_number_on_world_ico;

    public enum stage_screen_generation
    {
        automatic,
        manual
    }

    public stage_screen_generation stage_screen_generation_selected = stage_screen_generation.automatic;

    public bool use_world_screen_to_show_stages_too;

    //manage music
    public enum when_win_play
    {
        no,
        music,
        sfx
    }

    public when_win_play when_win_play_selected = when_win_play.music;
    public bool play_win_music_in_loop;

    public enum when_lose_play
    {
        no,
        music,
        sfx
    }

    public when_lose_play when_lose_play_selected = when_lose_play.music;
    public bool play_lose_music_in_loop;

    public AudioSource music_source;
    public static bool[] music_on; //[profile]
    public static float[] music_volume; //[profile]
    public static bool[] sfx_on; //[profile] if game play sound or not
    public static float[] sfx_volume; //[profile]
    public static bool[] voice_on; //[profile]
    public static float[] voice_volume; //[profile]
    public float fade_music = 0.5f; //music fade time
    public AudioClip music_menu;
    public AudioClip music_stage_win;
    public AudioClip music_stage_lose;
    public AudioClip[] show_big_star_sfx;
    AudioClip music_playing_now;

    //manage sfx
    public AudioSource sfx_source;
    public AudioClip tap_sfx;
    public AudioClip tap_error_sfx;

    //manage savestates
    public bool show_new_profile_window;
    public int number_of_save_profile_slot_avaibles;
    public int current_profile_selected;
    public static bool exist_a_save_state;
    public bool[] this_profile_have_a_save_state_in_it; //[profile]
    public string[] profile_name;
    public bool require_a_name_for_profiles;
    public bool[][] world_playable; //[profile][world]
    public bool[][] world_purchased; //[profile][world]
    public bool[][,] stage_playable; //[profile][world,stage]
    public bool[][,] stage_solved; //[profile][world,stage]
    //star score
    public int[][,] stage_stars_score; //[profile][world,stage]
    public int[][] star_score_in_this_world; //[profile][world]

    public int[] stars_total_score;
        //[profile] this can be helpful if you want to unlock worlds when player get enough stars 

    //int score
    public int[][,] best_int_score_in_this_stage; //[profile][world,stage]
    public int[] best_int_score_for_current_player; //[profile]
    public int best_int_score_on_this_device; //the best score among all profiles
    public static bool[] all_stages_solved; //[profile]

    public static GameObject game_master_obj;

    public static bool game_is_started = false;
    public static bool logo_already_show = false;

    public enum this_screen
    {
        home_screen,
        stage_screen
    }

    public this_screen go_to_this_screen = this_screen.home_screen;
    public bool a_window_is_open;

    //avoid multiple istances of this prefab
    bool keep_me;


    // Use this for initialization
    void Awake()
    {

        if (!game_is_started)
        {
            keep_me = true;
            home_scene_name = Application.loadedLevelName;
            my_store_item_manager = this.gameObject.GetComponent<store_item_manager>();

            my_ads_master = GetComponent<ads_master>();
            if (my_ads_master)
            {
                my_ads_master.my_game_master = this;
                my_ads_master.Initiate_ads();
            }
        }

        if (keep_me)
        {
            game_master_obj = this.gameObject;
            DontDestroyOnLoad(game_master_obj);
                //this prefab will be used as reference from the others, so don't destry it when load a new scene

            //sum the stages in every world to know the total number of stages in the game
            for (int w = 0; w < total_stages_in_world_n.Length; w++)
            {
                total_number_of_stages_in_the_game += total_stages_in_world_n[w];
                if (total_stages_in_world_n[w] > max_stages_in_a_world)
                    max_stages_in_a_world = total_stages_in_world_n[w];
            }
            if (show_debug_messages)
                Debug.Log("total_number_of_stages_in_the_game = " + total_number_of_stages_in_the_game +
                          " max_stages_in_a_world = " + max_stages_in_a_world);

            //create multy arrays for multy profile saves
            this_profile_have_a_save_state_in_it = new bool[number_of_save_profile_slot_avaibles];
            profile_name = new string[number_of_save_profile_slot_avaibles];

            music_on = new bool[number_of_save_profile_slot_avaibles];
            music_volume = new float[number_of_save_profile_slot_avaibles];
            sfx_on = new bool[number_of_save_profile_slot_avaibles];
            sfx_volume = new float[number_of_save_profile_slot_avaibles];
            voice_on = new bool[number_of_save_profile_slot_avaibles];
            voice_volume = new float[number_of_save_profile_slot_avaibles];

            if (!infinite_lives)
            {
                current_lives = new int[number_of_save_profile_slot_avaibles];
                target_time = new DateTime[number_of_save_profile_slot_avaibles];
                recharge_live_countdown_active = new bool[number_of_save_profile_slot_avaibles];
            }
            if (continue_rule_selected == continue_rule.continue_cost_a_continue_token)
                current_continue_tokens = new int[number_of_save_profile_slot_avaibles];

            world_playable = new bool[number_of_save_profile_slot_avaibles][];
            world_purchased = new bool[number_of_save_profile_slot_avaibles][];
            current_world = new int[number_of_save_profile_slot_avaibles];
            stage_playable = new bool[number_of_save_profile_slot_avaibles][,];
            total_number_of_stages_in_the_game_solved = new int[number_of_save_profile_slot_avaibles];
            stage_solved = new bool[number_of_save_profile_slot_avaibles][,];
            all_stages_solved = new bool[number_of_save_profile_slot_avaibles];
            dot_tail_turn_on = new bool[number_of_save_profile_slot_avaibles][,];
            play_this_stage_to_progress_in_the_game_world = new int[number_of_save_profile_slot_avaibles];
            play_this_stage_to_progress_in_the_game_stage = new int[number_of_save_profile_slot_avaibles];
            latest_stage_played_world = new int[number_of_save_profile_slot_avaibles];
            latest_stage_played_stage = new int[number_of_save_profile_slot_avaibles];

            stage_stars_score = new int[number_of_save_profile_slot_avaibles][,];
            star_score_in_this_world = new int[number_of_save_profile_slot_avaibles][];
            stars_total_score = new int[number_of_save_profile_slot_avaibles];

            best_int_score_in_this_stage = new int[number_of_save_profile_slot_avaibles][,];
            best_int_score_for_current_player = new int[number_of_save_profile_slot_avaibles];

            current_virtual_money = new int[number_of_save_profile_slot_avaibles];

            if (my_store_item_manager)
            {
                incremental_item_current_level = new int[number_of_save_profile_slot_avaibles][];
                consumable_item_current_quantity = new int[number_of_save_profile_slot_avaibles][];
            }


            for (int i = 0; i < number_of_save_profile_slot_avaibles; i++)
            {
                world_playable[i] = new bool[total_stages_in_world_n.Length];
                world_purchased[i] = new bool[total_stages_in_world_n.Length];
                stage_playable[i] = new bool[total_stages_in_world_n.Length, max_stages_in_a_world];
                stage_solved[i] = new bool[total_stages_in_world_n.Length, max_stages_in_a_world];
                stage_stars_score[i] = new int[total_stages_in_world_n.Length, max_stages_in_a_world];
                star_score_in_this_world[i] = new int[total_stages_in_world_n.Length];
                best_int_score_in_this_stage[i] = new int[total_stages_in_world_n.Length, max_stages_in_a_world];
                dot_tail_turn_on[i] = new bool[total_stages_in_world_n.Length, max_stages_in_a_world];

                this_profile_have_a_save_state_in_it[i] =
                    Convert.ToBoolean(PlayerPrefs.GetInt("profile_" + i.ToString() + "_have_a_save_state_in_it"));
                if (my_store_item_manager)
                {
                    incremental_item_current_level[i] = new int[my_store_item_manager.incremental_item_list.Length];
                    consumable_item_current_quantity[i] = new int[my_store_item_manager.consumable_item_list.Length];
                }
            }


            exist_a_save_state = Convert.ToBoolean(PlayerPrefs.GetInt("savestate"));
            current_profile_selected = PlayerPrefs.GetInt("last_profile_used");

            music_on[current_profile_selected] = true;
            music_volume[current_profile_selected] = 1;
            sfx_on[current_profile_selected] = true;
            sfx_volume[current_profile_selected] = 1;
            voice_on[current_profile_selected] = true;
            voice_volume[current_profile_selected] = 1;

            if (exist_a_save_state) //copy the saves in the arrays
            {
                if (PlayerPrefs.GetInt("total_number_of_stages_in_the_game") == total_number_of_stages_in_the_game)
                    //if the _total_stages number not is the same of last time, erase save data to avoid broken array (the _total_stages don't cange in the game, so the player don't chance to lose his saves. This is useful when you decide to change the total stage number through the making of the game and avoid errors because previous save data refer to a old version)
                {
                    if (show_debug_messages)
                    {
                        Debug.Log("same total_stages from the last time");
                        Debug.Log("current_profile_selected " + current_profile_selected);
                    }
                    //check if the last profile used have can be load
                    if (this_profile_have_a_save_state_in_it[current_profile_selected])
                        Load(current_profile_selected);
                    else //I can't find the last profile used
                    {
                        //so ask to select/create a profile
                        show_new_profile_window = true;
                    }
                }
                else
                //you have changed the total_stages from the last time, sto the old save data have a different array_length. 
                {
                    if (show_debug_messages)
                        Debug.Log("different total_stages from the last time");
                    if (this_profile_have_a_save_state_in_it[current_profile_selected])
                    {
                        if (total_number_of_stages_in_the_game >
                            PlayerPrefs.GetInt("total_number_of_stages_in_the_game"))
                            //you have add stages from last time
                        {
                            if (show_debug_messages)
                                Debug.Log("there are more stages that in the previous save data");
                            PlayerPrefs.SetInt("total_number_of_stages_in_the_game", total_number_of_stages_in_the_game);
                            for (int i = 0; i < number_of_save_profile_slot_avaibles; i++)
                            {
                                all_stages_solved[i] = false;
                                PlayerPrefs.SetInt("all_stages_solved", Convert.ToInt32(all_stages_solved[i]));
                            }
                            Load(current_profile_selected);
                        }
                        else //you have remove stages from last time
                        {
                            if (show_debug_messages)
                                Debug.Log("there are less stages that in the previous save data");
                            //delete the old data
                            Erase_saves();
                            for (int i = 0; i < number_of_save_profile_slot_avaibles; i++)
                            {
                                world_playable[i][0] = true;
                                stage_playable[i][0, 0] = true;
                                Save(i);
                            }

                        }
                    }
                    else //I can't find the last profile used
                    {
                        //so ask to select/create a profile
                        show_new_profile_window = true;
                    }
                }

            }
            else //no save data, so start the game from zero
            {
                current_profile_selected = 0;
                if (number_of_save_profile_slot_avaibles == 1) //if no multiple profiles are allowed, start new game
                {
                    if (show_debug_messages)
                        Debug.Log("no save data and only one profile slot allowed");
                    Create_new_profile("Player");
                }
                //else request to activate a new empty profile
                if (show_debug_messages)
                    Debug.Log("no save data and multi profile slot allowed");
                show_new_profile_window = true;
            }

        }
        else
        {
            //this avoid duplication of this istance
            Destroy(this.gameObject);
        }

    }


    #region manage savestates

    public void Create_new_profile(string my_name)
    {
        show_new_profile_window = false;
        if (show_debug_messages)
            Debug.Log("Create_new_profile " + current_profile_selected + " = " + my_name);
        profile_name[current_profile_selected] = my_name;

        stars_total_score[current_profile_selected] = 0;
        total_number_of_stages_in_the_game_solved[current_profile_selected] = 0;
        all_stages_solved[current_profile_selected] = false;


        for (int i = 0; i < total_stages_in_world_n.Length; i++)
        {
            if (this_world_is_unlocked_after_selected[i] == this_world_is_unlocked_after.start)
            {
                world_playable[current_profile_selected][i] = true;
                world_purchased[current_profile_selected][i] = false;
                stage_playable[current_profile_selected][i, 0] = true;
            }

        }

        play_this_stage_to_progress_in_the_game_world[current_profile_selected] = 0;
        play_this_stage_to_progress_in_the_game_stage[current_profile_selected] = 0;

        if (!infinite_lives)
            current_lives[current_profile_selected] = start_lives;
        if (continue_rule_selected == continue_rule.continue_cost_a_continue_token)
            current_continue_tokens[current_profile_selected] = start_continue_tokens;

        if (buy_virtual_money_with_real_money_with_soomla)
        {
            /* //DELETE THIS LINE FOR SOOMLA
			my_Soomla_billing_script.Remove_all_virtual_money_from_this_profile(current_profile_selected);
			my_Soomla_billing_script.Give_virtual_money_for_free (current_profile_selected, start_virtual_money);
			current_virtual_money[current_profile_selected] = my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(current_profile_selected);
			*/ //DELETE THIS LINE FOR SOOMLA
        }
        else
            current_virtual_money[current_profile_selected] = start_virtual_money;

        if (my_store_item_manager)
        {
            incremental_item_current_level[current_profile_selected] =
                new int[my_store_item_manager.incremental_item_list.Length];
            consumable_item_current_quantity[current_profile_selected] =
                new int[my_store_item_manager.consumable_item_list.Length];
        }

        Save(current_profile_selected);
    }




    public void Load(int profile_slot)
    {
        best_int_score_on_this_device = PlayerPrefs.GetInt("best_int_score_on_this_device");

        profile_name[profile_slot] = PlayerPrefs.GetString("profile_" + profile_slot.ToString() + "_name");

        music_on[profile_slot] =
            Convert.ToBoolean(PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_music_on_off"));
        music_volume[profile_slot] = PlayerPrefs.GetFloat("profile_" + profile_slot.ToString() + "_music_volume");
        sfx_on[profile_slot] =
            Convert.ToBoolean(PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_sfx_on_off"));
        sfx_volume[profile_slot] = PlayerPrefs.GetFloat("profile_" + profile_slot.ToString() + "_sfx_volume");
        voice_on[profile_slot] =
            Convert.ToBoolean(PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_voice_on_off"));
        voice_volume[profile_slot] = PlayerPrefs.GetFloat("profile_" + profile_slot.ToString() + "_voice_volume");

        if (buy_virtual_money_with_real_money_with_soomla)
        {
            /* //DELETE THIS LINE FOR SOOMLA
			current_virtual_money[profile_slot] = my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(profile_slot);
			*/ //DELETE THIS LINE FOR SOOMLA
        }
        else
            current_virtual_money[profile_slot] =
                PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_virtual_money");

        if (!infinite_lives)
        {
            current_lives[profile_slot] = PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_current_lives");
            recharge_live_countdown_active[profile_slot] =
                Convert.ToBoolean(
                    PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_" + "recharge_live_countdown_active"));
            if (recharge_live_countdown_active[profile_slot])
            {
                string temp_string = PlayerPrefs.GetString("profile_" + profile_slot.ToString() + "_target_time");
                target_time[current_profile_selected] = DateTime.Parse(temp_string);
                Check_countdown();
            }
        }
        if (continue_rule_selected == continue_rule.continue_cost_a_continue_token)
            current_continue_tokens[profile_slot] =
                PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_current_continue_tokens");

        best_int_score_for_current_player[profile_slot] =
            PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_best_int_score_for_this_profile");

        stars_total_score[profile_slot] = PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_total_stars");
        total_number_of_stages_in_the_game_solved[profile_slot] =
            PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_total_number_of_stages_in_the_game_solved");
        all_stages_solved[profile_slot] =
            Convert.ToBoolean(PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_all_stages_solved"));


        play_this_stage_to_progress_in_the_game_world[profile_slot] =
            PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_play_this_world_to_progress");
        play_this_stage_to_progress_in_the_game_stage[profile_slot] =
            PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_play_this_stage_to_progress");

        for (int world = 0; world < total_stages_in_world_n.Length; world++)
        {

            world_purchased[profile_slot][world] =
                Convert.ToBoolean(
                    PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" +
                                       "world_purchased"));
            star_score_in_this_world[profile_slot][world] =
                PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_star_score_in_this_world");

            for (int stage = 0; stage < total_stages_in_world_n[world]; stage++)
            {
                //array bool
                stage_playable[profile_slot][world, stage] =
                    Convert.ToBoolean(
                        PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                           stage.ToString() + "_" + "stages_unlocked"));
                stage_solved[profile_slot][world, stage] =
                    Convert.ToBoolean(
                        PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                           stage.ToString() + "_" + "stage_solved"));
                dot_tail_turn_on[profile_slot][world, stage] =
                    Convert.ToBoolean(
                        PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                           stage.ToString() + "_" + "dots"));

                //array int
                stage_stars_score[profile_slot][world, stage] =
                    PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                       stage.ToString() + "_" + "stage_stars_score");
                best_int_score_in_this_stage[profile_slot][world, stage] =
                    PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                       stage.ToString() + "_" + "stage_int_score");
            }

            if (
                PlayerPrefs.HasKey("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" +
                                   "world_unlocked"))
                world_playable[profile_slot][world] =
                    Convert.ToBoolean(
                        PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" +
                                           "world_unlocked"));
            else
            {
                if (this_world_is_unlocked_after_selected[world] == this_world_is_unlocked_after.start)
                {
                    world_playable[profile_slot][world] = true;
                    stage_playable[profile_slot][world, 0] = true;
                }
            }
        }

        if (my_store_item_manager)
        {
            Load_incremental_items(profile_slot);
            Load_consumable_items(profile_slot);
        }

        if (show_debug_messages)
            Debug.Log("Load savestate profile: " + profile_slot);
    }

    void Load_incremental_items(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.incremental_item_list.Length; i++)
        {
            incremental_item_current_level[profile_slot][i] =
                PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_incremental_item_" + i.ToString() +
                                   "_current_level");
        }
    }

    void Load_consumable_items(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.consumable_item_list.Length; i++)
        {
            consumable_item_current_quantity[profile_slot][i] =
                PlayerPrefs.GetInt("profile_" + profile_slot.ToString() + "_consumable_item_" + i.ToString() +
                                   "_current_quantity");
        }
    }

    public void Save(int profile_slot)
    {
        exist_a_save_state = true;
        this_profile_have_a_save_state_in_it[profile_slot] = true;

        PlayerPrefs.SetInt("best_int_score_on_this_device", best_int_score_on_this_device);

        PlayerPrefs.SetInt("total_number_of_stages_in_the_game", total_number_of_stages_in_the_game);

        PlayerPrefs.SetString("profile_" + profile_slot.ToString() + "_name", profile_name[profile_slot]);

        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_music_on_off",
            Convert.ToInt32(music_on[profile_slot]));
        PlayerPrefs.SetFloat("profile_" + profile_slot.ToString() + "_music_volume", music_volume[profile_slot]);
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_sfx_on_off", Convert.ToInt32(sfx_on[profile_slot]));
        PlayerPrefs.SetFloat("profile_" + profile_slot.ToString() + "_sfx_volume", sfx_volume[profile_slot]);
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_voice_on_off",
            Convert.ToInt32(voice_on[profile_slot]));
        PlayerPrefs.SetFloat("profile_" + profile_slot.ToString() + "_voice_volume", voice_volume[profile_slot]);


        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_all_stages_solved",
            Convert.ToInt32(all_stages_solved[profile_slot]));

        if (!infinite_lives)
        {
            PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_current_lives", current_lives[profile_slot]);
            PlayerPrefs.SetInt(("profile_" + profile_slot.ToString() + "_" + "recharge_live_countdown_active"),
                Convert.ToInt32(recharge_live_countdown_active[profile_slot]));
            if (recharge_live_countdown_active[profile_slot])
            {
                PlayerPrefs.SetString("profile_" + profile_slot.ToString() + "_target_time",
                    target_time[current_profile_selected].ToString());
            }
        }

        if (continue_rule_selected == continue_rule.continue_cost_a_continue_token)
            PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_current_continue_tokens",
                current_continue_tokens[profile_slot]);

        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_virtual_money", current_virtual_money[profile_slot]);

        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_total_number_of_stages_in_the_game_solved",
            total_number_of_stages_in_the_game_solved[profile_slot]);
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_total_stars", stars_total_score[profile_slot]);

        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_best_int_score_for_this_profile",
            best_int_score_for_current_player[profile_slot]);


        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_play_this_world_to_progress",
            play_this_stage_to_progress_in_the_game_world[profile_slot]);
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_play_this_stage_to_progress",
            play_this_stage_to_progress_in_the_game_stage[profile_slot]);

        //PlayerPrefs.SetInt("profile_"+profile_slot.ToString()+"_savestate",Convert.ToInt32(exist_a_save_state[profile_slot]));
        PlayerPrefs.SetInt("savestate", Convert.ToInt32(exist_a_save_state));
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_have_a_save_state_in_it",
            Convert.ToInt32(this_profile_have_a_save_state_in_it[profile_slot]));
        PlayerPrefs.SetInt("last_profile_used", current_profile_selected);


        for (int world = 0; world < total_stages_in_world_n.Length; world++)
        {
            PlayerPrefs.SetInt(
                ("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" + "world_unlocked"),
                Convert.ToInt32(world_playable[profile_slot][world]));
            PlayerPrefs.SetInt(
                ("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" + "world_purchased"),
                Convert.ToInt32(world_purchased[profile_slot][world]));
            PlayerPrefs.SetInt(("profile_" + profile_slot.ToString() + "_star_score_in_this_world"),
                star_score_in_this_world[profile_slot][world]);

            for (int stage = 0; stage < total_stages_in_world_n[world]; stage++)
            {
                //bool arrays
                PlayerPrefs.SetInt(
                    "profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" + stage.ToString() + "_" +
                    "stages_unlocked", Convert.ToInt32(stage_playable[profile_slot][world, stage]));
                PlayerPrefs.SetInt(
                    "profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" + stage.ToString() + "_" +
                    "stage_solved", Convert.ToInt32(stage_solved[profile_slot][world, stage]));
                PlayerPrefs.SetInt(
                    "profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" + stage.ToString() + "_" +
                    "dots", Convert.ToInt32(dot_tail_turn_on[profile_slot][world, stage]));

                //in array
                PlayerPrefs.SetInt(
                    "profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" + stage.ToString() + "_" +
                    "stage_stars_score", stage_stars_score[profile_slot][world, stage]);
                PlayerPrefs.SetInt(
                    "profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" + stage.ToString() + "_" +
                    "stage_int_score", best_int_score_in_this_stage[profile_slot][world, stage]);
            }
        }

        if (my_store_item_manager)
        {
            Save_incremental_items(profile_slot);
            Save_consumable_items(profile_slot);
        }

        if (show_debug_messages)
            Debug.Log("Save savestate profile: " + profile_slot);
    }

    void Save_incremental_items(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.incremental_item_list.Length; i++)
        {
            PlayerPrefs.SetInt(
                "profile_" + profile_slot.ToString() + "_incremental_item_" + i.ToString() + "_current_level",
                incremental_item_current_level[profile_slot][i]);
        }
    }

    void Save_consumable_items(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.consumable_item_list.Length; i++)
        {
            PlayerPrefs.SetInt(
                "profile_" + profile_slot.ToString() + "_consumable_item_" + i.ToString() + "_current_quantity",
                consumable_item_current_quantity[profile_slot][i]);
        }
    }

    public void Erase_saves()
    {
        if (show_debug_messages)
            Debug.Log("erase data");
        PlayerPrefs.DeleteAll();
        exist_a_save_state = false;
    }

    public void Unlock_this_world(int world_n)
    {
        world_playable[current_profile_selected][world_n] = true;
        stage_playable[current_profile_selected][world_n, 0] = true;
    }

    public void Reset_current_world(int world_n, bool can_be_played_from_first_stage)
    {
        world_playable[current_profile_selected][world_n] = false;

        if (if_not_continue_lose_gained_stars)
        {
            //stars_total_score[current_profile_selected] -= star_score_in_this_world[my_game_master.current_profile_selected][n_world];
            star_score_in_this_world[current_profile_selected][world_n] = 0;
        }

        for (int i = 0; i < total_stages_in_world_n[world_n]; i++)
        {
            stage_playable[current_profile_selected][world_n, i] = false;
            if (if_not_continue_lose_gained_stars)
            {
                stars_total_score[current_profile_selected] -= stage_stars_score[current_profile_selected][world_n, i];
                stage_stars_score[current_profile_selected][world_n, i] = 0;
            }
        }

        if (can_be_played_from_first_stage)
            Unlock_this_world(world_n);
        else
        {
            if (this_world_is_unlocked_after_selected[world_n] == this_world_is_unlocked_after.start)
            {
                Unlock_this_world(world_n);
            }
            else if (this_world_is_unlocked_after_selected[world_n] == this_world_is_unlocked_after.buy_it)
            {
                if (world_purchased[current_profile_selected][world_n])
                {
                    Unlock_this_world(world_n);
                }
            }
            else if (this_world_is_unlocked_after_selected[world_n] == this_world_is_unlocked_after.reach_this_star_score)
            {
                if (stars_total_score[current_profile_selected] >= star_score_required_to_unlock_this_world[world_n])
                {
                    Unlock_this_world(world_n);
                }
            }
        }
    }

    public void Reset_all_worlds()
    {
        for (int world = 0; world < total_stages_in_world_n.Length; world++)
            Reset_current_world(world, false);

        play_this_stage_to_progress_in_the_game_world[current_profile_selected] = 0;
        play_this_stage_to_progress_in_the_game_stage[current_profile_selected] = 0;
    }

    public void Delete_this_profile(int profile_slot)
    {
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_name");
        profile_name[profile_slot] = "";

        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_music_on_off");
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_music_volume");
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_sfx_on_off");
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_sfx_volume");
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_voice_on_off");
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_voice_volume");

        if (!infinite_lives)
        {
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_current_lives");
            current_lives[profile_slot] = 0;
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_" + "recharge_live_countdown_active");
            recharge_live_countdown_active[profile_slot] = false;
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_target_time");
        }

        if (continue_rule_selected == continue_rule.continue_cost_a_continue_token)
        {
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_current_continue_tokens");
            current_continue_tokens[profile_slot] = 0;
        }

        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_total_stars");
        stars_total_score[profile_slot] = 0;
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_total_number_of_stages_in_the_game_solved");
        total_number_of_stages_in_the_game_solved[profile_slot] = 0;
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_all_stages_solved");
        all_stages_solved[profile_slot] = false;

        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_play_this_world_to_progress");
        play_this_stage_to_progress_in_the_game_world[profile_slot] = 0;
        PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_play_this_stage_to_progress");
        play_this_stage_to_progress_in_the_game_stage[profile_slot] = 0;

        for (int world = 0; world < total_stages_in_world_n.Length; world++)
        {
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "_" +
                                  "world_unlocked");
            world_playable[profile_slot][world] = false;
            for (int stage = 0; stage < total_stages_in_world_n[world]; stage++)
            {
                //array bool
                PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                      stage.ToString() + "_" + "stages_unlocked");
                stage_playable[profile_slot][world, stage] = false;
                PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                      stage.ToString() + "_" + "stage_solved");
                stage_solved[profile_slot][world, stage] = false;

                dot_tail_turn_on[profile_slot][world, stage] = false;


                //array int
                PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_array_W" + world.ToString() + "S" +
                                      stage.ToString() + "_" + "stage_stars_score");
                stage_stars_score[profile_slot][world, stage] = 0;
            }
        }

        this_profile_have_a_save_state_in_it[profile_slot] = false;
        PlayerPrefs.SetInt("profile_" + profile_slot.ToString() + "_have_a_save_state_in_it",
            Convert.ToInt32(this_profile_have_a_save_state_in_it[profile_slot]));

        if (my_store_item_manager)
        {
            Delete_incremental_items_in_this_profile(profile_slot);
            Delete_consumable_items_in_this_profile(profile_slot);
        }
    }

    void Delete_incremental_items_in_this_profile(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.incremental_item_list.Length; i++)
        {
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_incremental_item_" + i.ToString() +
                                  "_current_level");
            incremental_item_current_level[profile_slot][i] = 0;
        }
    }

    void Delete_consumable_items_in_this_profile(int profile_slot)
    {
        for (int i = 0; i < my_store_item_manager.consumable_item_list.Length; i++)
        {
            PlayerPrefs.DeleteKey("profile_" + profile_slot.ToString() + "_consumable_item_" + i.ToString() +
                                  "_current_quantity");
            consumable_item_current_quantity[profile_slot][i] = 0;
        }
    }

    public void All_stages_solved() //what happen when the player finish the game
    {
        all_stages_solved[current_profile_selected] = true;
        if (show_debug_messages)
            Debug.Log("All stages solved");
        Application.LoadLevel("End_screen");
    }



    #endregion


    #region countdown

    public void Set_date_countdown()
    {
        recharge_live_countdown_active[current_profile_selected] = true;
        target_time[current_profile_selected] = DateTime.Now;

        if (show_debug_messages)
            Debug.Log("now: " + target_time[current_profile_selected]);

        target_time[current_profile_selected] = target_time[current_profile_selected].AddDays((double) wait_for_days);
        target_time[current_profile_selected] = target_time[current_profile_selected].AddHours((double) wait_for_hours);
        target_time[current_profile_selected] =
            target_time[current_profile_selected].AddMinutes((double) wait_for_minutes);
        target_time[current_profile_selected] =
            target_time[current_profile_selected].AddSeconds((double) wait_for_seconds);

        if (show_debug_messages)
            Debug.Log("target time: " + target_time[current_profile_selected]);

        Check_countdown();
    }

    void Check_countdown()
    {
        TimeSpan span = target_time[current_profile_selected].Subtract(DateTime.Now);
        int days = span.Days;
        int hours = span.Hours;
        int minutes = span.Minutes;
        int seconds = span.Seconds;

        if (show_debug_messages)
            Debug.Log("Check_countdown: " + days + "," + hours + "," + minutes + "," + seconds);

        int total_seconds_to_wait = 0;
        if (days > 0)
            total_seconds_to_wait += days*86400;
        if (hours > 0)
            total_seconds_to_wait += hours*3600;
        if (minutes > 0)
            total_seconds_to_wait += minutes*60;
        if (seconds > 0)
            total_seconds_to_wait += seconds;

        if (show_debug_messages)
            Debug.Log("Total seconds to wait = " + total_seconds_to_wait);

        Invoke("Countdown_end", total_seconds_to_wait);
    }

    public string Show_how_much_time_left()
    {
        string my_text = "";

        TimeSpan span = target_time[current_profile_selected].Subtract(DateTime.Now);
        int days = span.Days;
        int hours = span.Hours;
        int minutes = span.Minutes;
        int seconds = span.Seconds;

        if (days > 0)
        {
            if (days == 1)
                my_text = "1 day and: ";
            else
                my_text = days.ToString() + " days and: ";
        }

        if (hours > 0)
        {
            if (hours < 10)
                my_text += "0";

            my_text += hours.ToString() + ":";
        }
        else
            my_text += "00:";

        if (minutes > 0)
        {
            if (minutes < 10)
                my_text += "0";

            my_text += minutes.ToString() + ":";
        }
        else
            my_text += "00:";

        if (seconds > 0)
        {
            if (seconds < 10)
                my_text += "0";

            my_text += seconds.ToString();
        }
        else
            my_text += "00";

        return my_text;
    }

    public void Check_if_interrupt_countdown()
    {
        if (recharge_live_countdown_active[current_profile_selected])
        {
            if (show_debug_messages)
                Debug.Log("interrupt_countdown");
            CancelInvoke("Countdown_end");
        }
    }

    void Countdown_end()
    {
        recharge_live_countdown_active[current_profile_selected] = false;
        current_lives[current_profile_selected] += if_not_continue_restart_with_lives;
        if (show_debug_warnings)
            Debug.LogWarning("Countdown_end");
        Save(current_profile_selected);
    }

    #endregion

    #region manage cameras

    /*
	public static void Link_me_to_camera(Camera camera_target)
		{
		if (camera_target)
			{
			//this put the AudioSource on the active camera
			game_master_obj.transform.parent = camera_target.transform;
				game_master_obj.transform.localPosition = Vector3.zero;
				game_master_obj.transform.localRotation = Quaternion.identity;
			}
		else
			{
				Debug.LogError("camera_target not exist!");
			}
		}
	
	public void Unlink_me_to_camera()
		{
		game_master_obj.transform.parent = null;
		}
		*/

    #endregion

    #region manage music

    public void Music_on_off(bool enabled)
    {
        music_on[current_profile_selected] = enabled;
        if (music_on[current_profile_selected])
        {
            music_source.volume = music_volume[current_profile_selected];
        }
        else
            music_source.volume = 0;

        PlayerPrefs.SetInt("profile_" + current_profile_selected.ToString() + "_music_on_off",
            Convert.ToInt32(music_on[current_profile_selected]));
        PlayerPrefs.SetFloat("profile_" + current_profile_selected.ToString() + "_music_volume",
            music_volume[current_profile_selected]);
    }

    public void Sfx_on_off(bool enabled)
    {
        sfx_on[current_profile_selected] = enabled;
        if (sfx_on[current_profile_selected])
            sfx_source.volume = sfx_volume[current_profile_selected];
        else
            sfx_source.volume = 0;

        PlayerPrefs.SetInt("profile_" + current_profile_selected.ToString() + "_sfx_on_off",
            Convert.ToInt32(sfx_on[current_profile_selected]));
        PlayerPrefs.SetFloat("profile_" + current_profile_selected.ToString() + "_sfx_volume",
            sfx_volume[current_profile_selected]);
    }

    public void Voice_on_off(bool enabled)
    {
        voice_on[current_profile_selected] = enabled;
        //no source because it must be within the game elements and not here
        /*
		if (voice_on[current_profile_selected])
			voice_source.volume = voice_volume[current_profile_selected];
		else
			voice_source.volume = 0;
		 */

        PlayerPrefs.SetInt("profile_" + current_profile_selected.ToString() + "_voice_on_off",
            Convert.ToInt32(voice_on[current_profile_selected]));
        PlayerPrefs.SetFloat("profile_" + current_profile_selected.ToString() + "_voice_volume",
            voice_volume[current_profile_selected]);
    }




    public void Start_music(AudioClip my_music, bool loop)
    {
        if (show_debug_messages)
            Debug.Log("call start music");
        //if you not are playing anything, start play
        if (music_playing_now == null)
        {
            music_playing_now = my_music;
            Music(loop);
        }
        else //if you aready play a music, change it if different
        {
            if (music_source.clip != my_music)
            {
                StartCoroutine(Change_music(my_music, loop));
            }
        }
    }

    void Music(bool loop)
    {
        if (show_debug_messages)
            Debug.Log("call music");
        if (music_source) //if there is an AudioSource
        {
            music_source.clip = music_playing_now; //what music play

            if (!music_source.isPlaying) //if you don't play the music yet, play it!
            {
                if (music_on[current_profile_selected]) //if music is on
                    music_source.volume = music_volume[current_profile_selected];
                else
                    music_source.volume = 0;

                music_source.Play();
                music_source.loop = loop;
            }
        }

    }


    IEnumerator Change_music(AudioClip new_music, bool loop)
    {

        float fade_duration = fade_music*0.5f;

        if (music_on[current_profile_selected])
        {
            if (show_debug_messages)
                Debug.Log("fade down");
            float passed_fade_down_time = 0;

            while (passed_fade_down_time < fade_duration)
            {
                music_source.volume = Mathf.Lerp(music_volume[current_profile_selected], 0,
                    (passed_fade_down_time/fade_duration));
                passed_fade_down_time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        music_playing_now = new_music; //change music
        Music(loop);

        if (music_on[current_profile_selected])
        {
            if (show_debug_messages)
                Debug.Log("fade up");
            float passed_fade_up_time = 0;
            while (passed_fade_up_time < fade_duration)
            {
                music_source.volume = Mathf.Lerp(0, music_volume[current_profile_selected],
                    (passed_fade_up_time/fade_duration));
                passed_fade_up_time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

    }

    public void Gui_sfx(AudioClip gui_sound)
    {
        if (sfx_on[current_profile_selected] && gui_sound)
        {
            if (!sfx_source.isPlaying)
            {
                sfx_source.PlayOneShot(gui_sound);
                sfx_source.loop = false;
            }
            else
            {
                sfx_source.Stop();
                sfx_source.PlayOneShot(gui_sound);
                sfx_source.loop = false;
            }
        }
    }

    #endregion


}
