using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Green;
// ReSharper disable InconsistentNaming

public class game_uGUI : MonoBehaviour
{

    [SerializeField]
    private EventSystem my_eventSystem = null;

    public int n_world;//the current world. It is need to save and load in the corret slot
    public int n_stage;//the number of this stage. It is need to save and load in the corret slot
    public AudioClip stage_music;

    public bool ignore_game_master_preferences;

    //ads
    public gift_manager my_gift_manager;
    public feedback_window my_feedback_window;
    [HideInInspector]
    public GameObject double_score;
    bool score_doubled;

    [HideInInspector]
    public Transform play_screen;
    [HideInInspector]
    public Transform pause_screen;
    [HideInInspector]
    public Transform loading_screen;
    [HideInInspector]
    public Transform options_screen;
    options_menu my_options;
    [HideInInspector]
    public Transform win_screen;
    [HideInInspector]
    public Transform lose_screen;
    [HideInInspector]
    public GameObject retry_button;
    [HideInInspector]
    public GameObject stage_button;
    [HideInInspector]
    public continue_window my_continue_window;

    //what button select with the pad when open this screen
    [HideInInspector]
    public GameObject options_screen_target_button;
    [HideInInspector]
    public GameObject pause_screen_target_button;
    [HideInInspector]
    public GameObject win_screen_target_button;
    [HideInInspector]
    public GameObject lose_screen_target_button;
    [HideInInspector]
    public GameObject continue_window_target_button;

    [HideInInspector]
    public GameObject lives_ico;
    [HideInInspector]
    public Text lives_count;

    public bool show_virtual_money;
    public bool keep_money_taken_in_this_stage_only_if_you_win;
    int temp_money_count;
    [HideInInspector]
    public GameObject virtual_money_ico;
    [HideInInspector]
    public Text virtual_money_count;

    [HideInInspector]
    public GameObject int_score_ico;
    [HideInInspector]
    public Text int_score_count;
    [HideInInspector]
    public GameObject int_score_record_ico;
    [HideInInspector]
    public Text int_score_record;
    [HideInInspector]
    public Text win_screen_int_score_title;
   // [HideInInspector]
    public GameObject int_score_record_anim;
    string temp_score_name;
    [HideInInspector]
    public Text win_screen_int_score_count;
    [HideInInspector]
    public Text win_screen_int_score_record;
    bool new_record;

    public bool show_star_score;
    public bool show_star_count;
    public bool show_progress_bar;
    public progress_bar my_progress_bar;
    public bool progress_bar_use_score;

    public bool show_int_score;
    public bool show_stage_record;
    [HideInInspector]
    public GameObject stars_ico;
    [HideInInspector]
    public Text stars_count;

    [HideInInspector]
    public GameObject lose_screen_lives_ico;
    [HideInInspector]
    public Text lose_screen_lives_count;

    [HideInInspector]
    public GameObject next_stage_ico;

    //win screen
    public float delay_start_star_score_animation = 1;
    public float delay_star_creation = 1; // recommend value = 1
    [HideInInspector]
    public GameObject star_container;
    [HideInInspector]
    public GameObject[] stars_on;
    int invoke_count = 0;
    [HideInInspector]
    public int star_number;
    [HideInInspector]
    public int int_score;
    public Sprite perfect_emoticon;
    [HideInInspector]
    public Image perfect_target;
    Sprite normal_emoticon;

    public static bool allow_game_input;//this is false when a menu is open
    public static bool in_pause;
    public static bool stage_end;

    game_master my_game_master;
    GameManager my_game_manager;
    
    public bool restart_without_reload_the_stage;
    [HideInInspector]
    public bool restarting;

    public bool show_debug_messages;
    public bool show_debug_warnings;

    // Use this for initialization
    void Start()
    {

        my_options = options_screen.GetComponent<options_menu>();
        normal_emoticon = perfect_target.sprite;

        if (game_master.game_master_obj)
        {
            my_game_master = (game_master)game_master.game_master_obj.GetComponent("game_master");
            my_game_master.my_ads_master.my_game_uGUI = this;
        }

        my_game_manager = GameManager.Instance;

        if (my_game_manager)
        {
            //if (my_game_master.score_name != "")
                //int_score_ico.GetComponent<Text>().text = my_game_master.score_name;

            //my_game_master.latest_stage_played_world[my_game_master.current_profile_selected] = n_world;
            //my_game_master.latest_stage_played_stage[my_game_master.current_profile_selected] = n_stage;


            //my_game_master.current_world[my_game_master.current_profile_selected] = n_world - 1;
        }

        /*
        if (my_game_master)
        {
            //set ads gui
            my_game_master.my_ads_master.Initiate_ads();
            my_game_master.my_ads_master.my_feedback_window = my_feedback_window;
            my_game_master.my_ads_master.my_gift_manager = my_gift_manager;
            my_gift_manager.my_game_master = my_game_master;

            //star score
            if (!ignore_game_master_preferences)
            {
                show_star_score = my_game_master.show_star_score;
                show_progress_bar = my_game_master.show_progres_bar;
                //int score
                show_int_score = my_game_master.show_int_score;
            }
            show_stage_record = my_game_master.show_int_score_stage_record_in_game_stage;


            if (my_game_master.score_name != "")
                int_score_ico.GetComponent<Text>().text = my_game_master.score_name;

            if (!ignore_game_master_preferences)
            {
                show_debug_messages = my_game_master.show_debug_messages;
                show_debug_warnings = my_game_master.show_debug_warnings;
            }

            my_game_master.latest_stage_played_world[my_game_master.current_profile_selected] = n_world;
            my_game_master.latest_stage_played_stage[my_game_master.current_profile_selected] = n_stage;


            my_game_master.current_world[my_game_master.current_profile_selected] = n_world - 1;
        }
        else
        {
            temp_score_name = win_screen_int_score_title.text;
            if (show_debug_warnings)
                Debug.LogWarning("In order to allow saves and play music and menu sfx, you must star from Home scene and open this stage using the in game menu");
        }
        */
        //star score
        if (show_star_score)
            star_container.SetActive(true);
        else
        {
            show_star_count = false;
            star_container.SetActive(false);
        }
        if (show_progress_bar)
        {
            my_progress_bar.Start_me(this);
            my_progress_bar.gameObject.SetActive(true);
        }
        else
            my_progress_bar.gameObject.SetActive(false);

        //int score
        if (show_int_score)
        {
            int_score_ico.SetActive(true);
        }
        else
            int_score_ico.SetActive(false);

        Reset_me();
    }

    public void Reset_me()
    {
        if (show_debug_messages)
            Debug.Log("reset stage game gui");

        Time.timeScale = 1;
        
        if (my_game_master)
        {
            //music
            my_game_master.Start_music(stage_music, true);

            //lives
            if (my_game_master.infinite_lives)
                lives_ico.SetActive(false);
            else
                Update_lives(0);

            my_game_master.star_score_difference = 0;

            if (!keep_money_taken_in_this_stage_only_if_you_win)
                virtual_money_count.text = my_game_master.current_virtual_money[my_game_master.current_profile_selected].ToString();
        }

        if (my_game_manager)
        {
            //my_game_manager.StartMusic();

        }


        //reset int score
        double_score.SetActive(false);
        score_doubled = false;
        int_score = 0;
        int_score_count.text = (0).ToString("N0");
        win_screen_int_score_title.gameObject.SetActive(false);
        int_score_record_anim.SetActive(false);
        if (my_game_master)
            win_screen_int_score_title.text = my_game_master.score_name;
        else
            win_screen_int_score_title.text = temp_score_name;
        win_screen_int_score_count.text = (0).ToString("N0");
        new_record = false;
        win_screen_int_score_record.gameObject.SetActive(new_record);
        win_screen_int_score_record.text = "";
        if (show_stage_record && my_game_master && !ignore_game_master_preferences)
        {
            int_score_record.text = (my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1]).ToString("N0");
            int_score_record_ico.SetActive(true);
        }
        else
            int_score_record_ico.SetActive(false);

        //virtual money
        temp_money_count = 0;
        if (keep_money_taken_in_this_stage_only_if_you_win || !my_game_master)
            virtual_money_count.text = temp_money_count.ToString();

        //reset star score
        star_number = 0;
        if (show_star_count)
        {
            stars_count.text = (0).ToString();
            stars_ico.gameObject.SetActive(true);
        }
        else
            stars_ico.gameObject.SetActive(false);

        //reset win screen
        win_screen.gameObject.SetActive(false);
        perfect_target.sprite = normal_emoticon;
        for (int i = 0; i < 3; i++)
        {
            stars_on[i].transform.localScale = Vector3.zero;
            stars_on[i].SetActive(false);
        }

        //reset lose screen
        lose_screen.gameObject.SetActive(false);


        loading_screen.gameObject.SetActive(false);
        pause_screen.gameObject.SetActive(false);

        //start
        allow_game_input = true;
        in_pause = false;
        stage_end = false;
        play_screen.gameObject.SetActive(true);

        if (my_game_master)
            my_game_master.my_ads_master.Call_ad(my_game_master.my_ads_master.ads_when_stage_start);

    }



    void Update()
    {
        if (my_game_master)
        {
            if (Input.GetKeyDown(my_game_master.pad_pause_button) && !my_continue_window.gameObject.activeSelf
                && (play_screen.gameObject.activeSelf || pause_screen.gameObject.activeSelf))
                Pause();

            Manage_ESC();
            Manage_pad_back();
        }
    }

    void Manage_pad_back()
    {
        if ((Input.GetKeyDown(my_game_master.pad_back_button) && my_game_master.use_pad))
        {
            if (!play_screen.gameObject.activeSelf)
            {
                if (play_screen.gameObject.activeSelf || pause_screen.gameObject.activeSelf)
                {
                    Pause();
                }
                else
                {
                    if (options_screen.gameObject.activeSelf)
                        Close_options_menu(true);
                    else
                        Go_to_stage_screen();
                }
            }
        }
    }

    void Manage_ESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && my_game_master.allow_ESC)
        {
            if (!my_continue_window.gameObject.activeSelf)
            {
                if ((play_screen.gameObject.activeSelf || pause_screen.gameObject.activeSelf))
                    Pause();
                else if (options_screen.gameObject.activeSelf)
                    Close_options_menu(true);
                else
                    Go_to_stage_screen();
            }
        }
    }

    public void Open_options_menu(bool from_pause_screen)
    {
        if (my_game_master)
        {
            if (from_pause_screen)
                pause_screen.gameObject.SetActive(false);
            else
            {
                in_pause = true;
                allow_game_input = false;
                play_screen.gameObject.SetActive(false);
                Time.timeScale = 0;
            }

            options_screen.gameObject.SetActive(true);
            my_options.Start_me();
            Mark_this_button(options_screen_target_button);
        }
        else
        {
            if (show_debug_warnings)
                Debug.LogWarning("In order to allow saves and play music and menu sfx, you must star from Home scene and open this stage using the in game menu");
        }
    }

    public void Close_options_menu(bool back_to_pause_screen)
    {
        options_screen.gameObject.SetActive(false);
        if (back_to_pause_screen)
        {
            pause_screen.gameObject.SetActive(true);
            Mark_this_button(pause_screen_target_button);
        }
        else
        {
            in_pause = false;
            allow_game_input = true;
            play_screen.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void Pause()
    {
        if (my_game_master)
            my_game_master.Gui_sfx(my_game_master.tap_sfx);

        if (in_pause)
        {
            in_pause = false;
            allow_game_input = true;
            play_screen.gameObject.SetActive(true);           
            pause_screen.gameObject.SetActive(false);
            Mark_this_button(null);
            my_game_manager.ChangeState(GameState.Playing);
        }
        else
        {
            in_pause = true;
            allow_game_input = false;
            play_screen.gameObject.SetActive(false);
            pause_screen.gameObject.SetActive(true);           
            Mark_this_button(pause_screen_target_button);
            my_game_manager.ChangeState(GameState.Pause);
        }

    }

    public void Mark_this_button(GameObject target_button)
    {
        if (show_debug_messages)
        {
            if (target_button)
                Debug.Log("Mark_this_button: " + target_button.name);
            else
                if (show_debug_messages)
                Debug.Log("NULL");
        }

        if (my_game_master && my_game_master.use_pad)
        {
            my_eventSystem.SetSelectedGameObject(target_button);
        }
    }

    public void Mark_continue()
    {
        if (show_debug_messages)
            Debug.Log("Mark_continue()");
        Mark_this_button(continue_window_target_button);
    }

    public void Retry()
    {
        if (my_game_master)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            //my_game_master.Unlink_me_to_camera();
            if (my_game_master.show_loading_screen)
                loading_screen.gameObject.SetActive(true);
        }

        if (my_game_manager)
        {
            //²¥·ÅÒôÀÖ
            //my_game_master.Gui_sfx(my_game_master.tap_sfx);
            //my_game_master.Unlink_me_to_camera();

            //if (my_game_master.show_loading_screen)
            //    loading_screen.gameObject.SetActive(true);
        }

        //reload this stage
        if (restart_without_reload_the_stage)
        {
            restarting = true;
            Reset_me();
        }
        else
            Application.LoadLevel(Application.loadedLevel);
    }

    public void Next()
    {
        if (my_game_master)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            //my_game_master.Unlink_me_to_camera();

            if (n_stage < my_game_master.total_stages_in_world_n[n_world - 1])//there are more stages in this world to play
            {
                int next_stage = n_stage + 1;
                int next_world = n_world;
                if (show_debug_messages)
                    Debug.Log("there are more stage in this world, so go to " + "W" + next_world.ToString() + "_Stage_" + next_stage.ToString());
                if (my_game_master.show_loading_screen)
                    loading_screen.gameObject.SetActive(true);
                Application.LoadLevel("W" + next_world.ToString() + "_Stage_" + next_stage.ToString());
            }
            else //go to next word if exist
            {
                if (n_world < my_game_master.total_stages_in_world_n.Length)
                {
                    if (my_game_master.world_playable[my_game_master.current_profile_selected][n_world] && my_game_master.stage_playable[my_game_master.current_profile_selected][n_world, 0])
                    {
                        int next_world = n_world + 1;
                        if (show_debug_messages)
                            Debug.Log("go to next world " + ("W" + next_world.ToString() + "_Stage_1"));
                        if (my_game_master.show_loading_screen)
                            loading_screen.gameObject.SetActive(true);
                        Application.LoadLevel("W" + next_world.ToString() + "_Stage_1");
                    }
                    else
                        Go_to_stage_screen();
                }
                else //this was the last stage, so...
                    my_game_master.All_stages_solved();
            }
        }
        else
        {
            if (show_debug_warnings)
                Debug.LogWarning("You must start the game from Home scene in order to use this button");
        }
    }

    public void Go_to_stage_screen()
    {
        if (my_game_master)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            //my_game_master.Unlink_me_to_camera();
            my_game_master.go_to_this_screen = game_master.this_screen.stage_screen;
            Time.timeScale = 1;
            if (my_game_master.show_loading_screen)
                loading_screen.gameObject.SetActive(true);
            Application.LoadLevel(my_game_master.home_scene_name);
        }
        else
        {
            if (show_debug_warnings)
                Debug.LogWarning("You must start the game from Home scene in order to use this button");
        }
    }

    public void Go_to_Home_screen()
    {
        if (my_game_master)
        {
            my_game_master.refresh_stage_and_world_screens = true;
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            //my_game_master.Unlink_me_to_camera();
            my_game_master.go_to_this_screen = game_master.this_screen.home_screen;
            Time.timeScale = 1;
            if (my_game_master.show_loading_screen)
                loading_screen.gameObject.SetActive(true);
            Application.LoadLevel(my_game_master.home_scene_name);
        }
        else
        {
            if (show_debug_warnings)
                Debug.LogWarning("You must start the game from Home scene in order to use this button");
        }
    }

    public void Update_virtual_money(int money)
    {
        Debug.Log("money: " + money);
        Debug.Log(my_game_master.current_virtual_money[my_game_master.current_profile_selected]);
        Debug.Log((my_game_master.current_virtual_money[my_game_master.current_profile_selected] + money));
        if (keep_money_taken_in_this_stage_only_if_you_win)
        {
            temp_money_count += money;
            virtual_money_count.text = temp_money_count.ToString();
        }
        else
        {
            if (my_game_master)
            {
                if ((my_game_master.current_virtual_money[my_game_master.current_profile_selected] + money) <= my_game_master.virtual_money_cap)
                {
                    if (my_game_master.buy_virtual_money_with_real_money_with_soomla)
                    {
                        /* DELETE THIS LINE FOR SOOMLA
						my_game_master.my_Soomla_billing_script.Give_virtual_money_for_free(my_game_master.current_profile_selected,money);
						my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
						*/ //DELETE THIS LINE FOR SOOMLA
                    }
                    else
                        my_game_master.current_virtual_money[my_game_master.current_profile_selected] += money;

                    if (show_debug_messages)
                        Debug.Log("add money: " + money);
                }
                else
                {
                    if (my_game_master.buy_virtual_money_with_real_money_with_soomla)
                    {
                        /* DELETE THIS LINE FOR SOOMLA
						my_game_master.my_Soomla_billing_script.Give_virtual_money_for_free(my_game_master.current_profile_selected,(my_game_master.virtual_money_cap-my_game_master.current_virtual_money[my_game_master.current_profile_selected]));
						my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
						*/ //DELETE THIS LINE FOR SOOMLA
                    }
                    else
                        my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.virtual_money_cap;

                    if (show_debug_messages)
                        Debug.Log("virtual money cap");
                }

                PlayerPrefs.SetInt("profile_" + my_game_master.current_profile_selected.ToString() + "_virtual_money", my_game_master.current_virtual_money[my_game_master.current_profile_selected]);
                virtual_money_count.text = my_game_master.current_virtual_money[my_game_master.current_profile_selected].ToString();


            }
            else
            {
                if (show_debug_warnings)
                    Debug.LogWarning("You must start the game from Home scene in order to save virtual money in a game profile");
            }
        }
    }

    public void Update_lives(int live_variation)
    {
        bool dead = false;
        if (my_game_master)
        {
            if (!my_game_master.infinite_lives)
            {
                my_game_master.current_lives[my_game_master.current_profile_selected] += live_variation;
                if (show_debug_messages)
                    Debug.Log("lives = " + my_game_master.current_lives[my_game_master.current_profile_selected]);


                if (my_game_master.current_lives[my_game_master.current_profile_selected] > my_game_master.live_cap)
                    my_game_master.current_lives[my_game_master.current_profile_selected] = my_game_master.live_cap;
                else if (my_game_master.current_lives[my_game_master.current_profile_selected] <= 0)
                {
                    my_game_master.current_lives[my_game_master.current_profile_selected] = 0;
                    dead = true;
                }
                else
                    dead = false;
                my_game_master.Save(my_game_master.current_profile_selected);

                if (dead)
                {
                    if (my_game_master.lose_lives_selected == game_master.lose_lives.in_game)
                    {
                        if (my_game_master.continue_rule_selected != game_master.continue_rule.never_continue)
                        {//ask if you want continue
                            if (my_game_master.my_ads_master.ads_when_continue_screen_appear.this_ad_is_enabled)
                                my_continue_window.Start_me_with_ad(my_game_master.my_ads_master.ads_when_continue_screen_appear);
                            else
                                my_continue_window.Start_me();
                        }
                        else
                        {//ultimate lose
                            Defeat();
                        }
                    }
                    else if (my_game_master.lose_lives_selected == game_master.lose_lives.when_show_lose_screen)
                    {
                        if (my_game_master.continue_rule_selected != game_master.continue_rule.never_continue)
                        {//ask if you want continue
                            if (my_game_master.my_ads_master.ads_when_continue_screen_appear.this_ad_is_enabled)
                                my_continue_window.Start_me_with_ad(my_game_master.my_ads_master.ads_when_continue_screen_appear);
                            else
                                my_continue_window.Start_me();
                        }
                    }
                }
                else
                {
                    //player continue to play from check point in this state or after new live animation, etcetera...
                    //these behavior are managed from yours scripts and not from this generic gui system
                }

                lives_count.text = my_game_master.current_lives[my_game_master.current_profile_selected].ToString();
                lose_screen_lives_count.text = my_game_master.current_lives[my_game_master.current_profile_selected].ToString();
            }
        }
        else
        {
            if (show_debug_warnings)
                Debug.LogWarning("You must start the game from Home scene in order to keep track of lives");
        }
    }

    public void Update_int_score(int points)
    {
        int_score += points;
        int_score_count.text = int_score.ToString("N0");
        if (show_progress_bar && progress_bar_use_score)
            my_progress_bar.Update_fill(int_score);
    }

    public void Update_int_score()
    {
        int_score_count.text = int_score.ToString("N0");
        if (show_progress_bar && progress_bar_use_score)
            my_progress_bar.Update_fill(int_score);
    }

    public void Add_stars(int quantity)
    {
        star_number += quantity;//add star
        stars_count.text = star_number.ToString();//update gui
    }

    public void New_star_score(int star_total)
    {
        star_number = star_total;//add star
        stars_count.text = star_number.ToString();//update gui
    }

    void Update_int_score_record()
    {
        if (int_score > 0)
        {
            new_record = true;

            if (show_debug_messages)
                Debug.Log("new stage int record!");

            if (my_game_master.what_say_if_new_stage_record != "")
                win_screen_int_score_record.text = my_game_master.what_say_if_new_stage_record;

            my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1] = int_score;
            PlayerPrefs.SetInt("profile_" + my_game_master.current_profile_selected.ToString() + "_array_W" + (n_world - 1).ToString() + "S" + (n_stage - 1).ToString() + "_" + "stage_int_score", my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1]);


            if (int_score > my_game_master.best_int_score_for_current_player[my_game_master.current_profile_selected])
            {
                if (show_debug_messages)
                    Debug.Log("new personal record!");

                if (my_game_master.what_say_if_new_personal_record != "")
                    win_screen_int_score_record.text = my_game_master.what_say_if_new_personal_record;

                my_game_master.best_int_score_for_current_player[my_game_master.current_profile_selected] = int_score;
                PlayerPrefs.SetInt("profile_" + my_game_master.current_profile_selected.ToString() + "_best_int_score_for_this_profile", my_game_master.best_int_score_for_current_player[my_game_master.current_profile_selected]);



                if (my_game_master.number_of_save_profile_slot_avaibles > 1)
                {
                    if (int_score > my_game_master.best_int_score_on_this_device)
                    {
                        if (show_debug_messages)
                            Debug.Log("new device record!");

                        if (my_game_master.what_say_if_new_device_record != "")
                            win_screen_int_score_record.text = my_game_master.what_say_if_new_device_record;

                        my_game_master.best_int_score_on_this_device = int_score;
                        PlayerPrefs.SetInt("best_int_score_on_this_device", my_game_master.best_int_score_on_this_device);
                        my_game_master.best_int_score_for_current_player[my_game_master.current_profile_selected] = int_score;
                        PlayerPrefs.SetInt("profile_" + my_game_master.current_profile_selected.ToString() + "_best_int_score_for_this_profile", my_game_master.best_int_score_for_current_player[my_game_master.current_profile_selected]);

                    }
                }
            }
        }
    }

    public void Victory()
    {
        if (!stage_end)
        {
            stage_end = true;


            if (show_debug_messages)
                Debug.Log("you win " + "W" + (n_world) + "_Stage_" + (n_stage));
            allow_game_input = false;
            in_pause = true;

            //go to win screen
            play_screen.gameObject.SetActive(false);
            win_screen.gameObject.SetActive(true);

            if (show_star_score)
                StartCoroutine(Show_star_score(star_number));

            if (my_game_master)
            {
                //music
                if (my_game_master.when_win_play_selected == game_master.when_win_play.music)
                    my_game_master.Start_music(my_game_master.music_stage_win, my_game_master.play_win_music_in_loop);
                else if (my_game_master.when_win_play_selected == game_master.when_win_play.sfx)
                    my_game_master.Gui_sfx(my_game_master.music_stage_win);

                if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)
                    next_stage_ico.SetActive(false);
                else
                    next_stage_ico.SetActive(true);

                //virtual money
                if (keep_money_taken_in_this_stage_only_if_you_win)
                {
                    if (my_game_master.virtual_money_cap < (my_game_master.current_virtual_money[my_game_master.current_profile_selected] + temp_money_count))
                    {
                        if (my_game_master.buy_virtual_money_with_real_money_with_soomla)
                        {
                            /* //DELETE THIS LINE FOR SOOMLA
							my_game_master.my_Soomla_billing_script.Give_virtual_money_for_free(my_game_master.current_profile_selected,temp_money_count);
							my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
							*/ //DELETE THIS LINE FOR SOOMLA
                        }
                        else
                            my_game_master.current_virtual_money[my_game_master.current_profile_selected] += temp_money_count;
                    }
                    else
                    {
                        if (my_game_master.buy_virtual_money_with_real_money_with_soomla)
                        {
                            /* //DELETE THIS LINE FOR SOOMLA
							my_game_master.my_Soomla_billing_script.Give_virtual_money_for_free(my_game_master.current_profile_selected,(my_game_master.virtual_money_cap-my_game_master.current_virtual_money[my_game_master.current_profile_selected]));
							my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.my_Soomla_billing_script.Show_how_many_virtual_money_there_is_in_this_profile(my_game_master.current_profile_selected);
							*/ //DELETE THIS LINE FOR SOOMLA
                        }
                        else
                            my_game_master.current_virtual_money[my_game_master.current_profile_selected] = my_game_master.virtual_money_cap;

                        if (show_debug_messages)
                            Debug.Log("virtual money cap");
                    }
                }

                //if you have solved this stage for the first time
                if (!my_game_master.stage_solved[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
                {
                    if (show_debug_messages)
                        Debug.Log("first time win");
                    //update stage count
                    my_game_master.total_number_of_stages_in_the_game_solved[my_game_master.current_profile_selected]++;
                    my_game_master.stage_solved[my_game_master.current_profile_selected][n_world - 1, n_stage - 1] = true;
                    //update star score
                    my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1] = star_number;
                    my_game_master.star_score_in_this_world[my_game_master.current_profile_selected][n_world - 1] += star_number;
                    my_game_master.stars_total_score[my_game_master.current_profile_selected] += star_number;
                    my_game_master.star_score_difference = star_number;
                    //update int score
                    Update_int_score_record();

                }
                else //you have solved this level more than once
                {
                    if (show_debug_messages)
                        Debug.Log("rewin same stage: " + star_number + " - " + my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1] + " = " + (star_number - my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
                                  + " *** int score = " + int_score);

                    //if your star score is better than the previous
                    if (star_number > my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
                    {
                        //update star score
                        my_game_master.star_score_difference = (star_number - my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1]);

                        my_game_master.stars_total_score[my_game_master.current_profile_selected] += (star_number - my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1]);
                        my_game_master.stage_stars_score[my_game_master.current_profile_selected][n_world - 1, n_stage - 1] = star_number;
                        my_game_master.star_score_in_this_world[my_game_master.current_profile_selected][n_world - 1] += my_game_master.star_score_difference;
                        if (show_debug_messages)
                            Debug.Log("...with better score = " + my_game_master.star_score_difference);
                    }
                    else
                    {
                        if (show_debug_messages)
                            Debug.Log("...but without better star score");
                        my_game_master.star_score_difference = 0;
                    }

                    //if your int score is better than the previous
                    if (int_score > my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
                    {
                        //update int score
                        Update_int_score_record();
                    }
                    else
                    {
                        if (show_debug_messages)
                            Debug.Log("no new int_score record");
                    }

                }

                //unlock the next stage if it exist
                if (n_stage < my_game_master.total_stages_in_world_n[n_world - 1])
                {
                    if (!my_game_master.stage_playable[my_game_master.current_profile_selected][n_world - 1, n_stage])
                    {
                        my_game_master.stage_playable[my_game_master.current_profile_selected][n_world - 1, n_stage] = true;
                        my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected] = n_world - 1;
                        my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected] = n_stage;
                    }
                }
                //unlock next world if it exist
                else if (n_world < my_game_master.total_stages_in_world_n.Length)
                {
                    my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected] = n_world;
                    my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected] = 0;

                    if (my_game_master.this_world_is_unlocked_after_selected[n_world] == game_master.this_world_is_unlocked_after.previous_world_is_finished)
                    {
                        my_game_master.world_playable[my_game_master.current_profile_selected][n_world] = true;
                        my_game_master.stage_playable[my_game_master.current_profile_selected][n_world, 0] = true;
                    }
                    else if (my_game_master.this_world_is_unlocked_after_selected[n_world] == game_master.this_world_is_unlocked_after.reach_this_star_score)
                    {
                        if (my_game_master.stars_total_score[my_game_master.current_profile_selected] >= my_game_master.star_score_required_to_unlock_this_world[n_world])
                        {
                            my_game_master.world_playable[my_game_master.current_profile_selected][n_world] = true;
                            my_game_master.stage_playable[my_game_master.current_profile_selected][n_world, 0] = true;
                        }
                    }


                }
                my_game_master.Save(my_game_master.current_profile_selected);
                if (show_debug_messages)
                    Debug.Log("stage score: " + star_number + " *** total score: " + my_game_master.stars_total_score[my_game_master.current_profile_selected]);
            }

            if (show_int_score && !show_star_score)
                StartCoroutine(Int_score_animation(0.5f, 0));

            Invoke("Mark_win", 0.1f);
        }
    }


    IEnumerator Int_score_animation(float wait, int start_from)
    {
        yield return new WaitForSeconds(wait);

        win_screen_int_score_title.gameObject.SetActive(true);

        //animation
        if (int_score > 0)
        {
            int temp_score = start_from;
            int add_this = int_score / 100;
            float seconds = int_score / (10 * int_score);

            if (add_this < 1)
                add_this = 1;

            if (seconds == 0)
                seconds = 0.01f;

            while (temp_score < int_score)
            {
                if ((temp_score + add_this) < int_score)
                    temp_score += add_this;
                else
                    temp_score = int_score;

                win_screen_int_score_count.text = (temp_score).ToString("N0");
                yield return new WaitForSeconds(seconds);
            }
        }

        //end animation
        win_screen_int_score_count.text = (int_score).ToString("N0");
        win_screen_int_score_record.gameObject.SetActive(new_record);
        if (new_record)
            int_score_record_anim.SetActive(true);
        //ads
        if (my_game_master)
        {
            if (my_game_master.my_ads_master.ask_if_double_int_score.this_ad_is_enabled && !score_doubled && int_score > 0 && my_game_master.my_ads_master.Advertisement_isInitialized())
            {
                if (my_game_master.my_ads_master.ask_if_double_int_score_when_selected == ads_master.ask_if_double_int_score_when.random)
                {

                    if (UnityEngine.Random.Range(1, 100) <= my_game_master.my_ads_master.ask_if_double_int_score.chance_to_open_an_ad_here)
                        double_score.SetActive(true);
                }
            }
        }

        if (win_screen.gameObject.activeSelf)
            Invoke("Mark_win", 0.1f);
        else if (lose_screen.gameObject.activeSelf)
            Invoke("Mark_lose", 0.1f);


    }

    public void Double_score_button()
    {
        my_game_master.Gui_sfx(my_game_master.tap_sfx);
        double_score.SetActive(false);
        my_game_master.my_ads_master.current_ad = my_game_master.my_ads_master.ask_if_double_int_score;
        //star ad
        my_game_master.my_ads_master.Show_ad(true);//true = rewarded

    }

    public void Score_doubled()
    {
        double_score.SetActive(false);
        score_doubled = true;
        new_record = false;
        int old_score = int_score;
        int_score = int_score * 2;

        //check if new record
        if (int_score > my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
        {
            Update_int_score_record();
            my_game_master.Save(my_game_master.current_profile_selected);
        }

        StartCoroutine(Int_score_animation(0.25f, old_score));
    }


    void Mark_win()
    {
        if (show_debug_messages)
            Debug.Log("Mark_win()");
        Mark_this_button(win_screen_target_button);

    }

    public void Defeat()
    {
        if (!stage_end)
        {
            stage_end = true;
            if (show_debug_messages)
                Debug.Log("you lose");

            allow_game_input = false;
            in_pause = true;

            if (my_game_master)
            {
                if (my_game_master.infinite_lives)
                {
                    lose_screen_lives_ico.SetActive(false);
                    retry_button.SetActive(true);
                    stage_button.SetActive(true);
                }
                else
                {
                    lose_screen_lives_ico.SetActive(true);
                    if (my_game_master.lose_lives_selected == game_master.lose_lives.when_show_lose_screen)
                        Update_lives(-1);

                    if (my_game_master.current_lives[my_game_master.current_profile_selected] > 0)
                    {
                        retry_button.SetActive(true);
                        stage_button.SetActive(true);
                    }
                    else
                    {
                        retry_button.SetActive(false);
                        stage_button.SetActive(false);
                        if (my_game_master.continue_rule_selected == game_master.continue_rule.never_continue)
                        {
                            my_continue_window.my_game_master = my_game_master;
                            my_continue_window.Continue_no(false);
                        }
                    }
                }

            }

            //go to lose screen
            play_screen.gameObject.SetActive(false);
            lose_screen.gameObject.SetActive(true);

            if (my_game_master)
            {
                if (my_game_master.when_lose_play_selected == game_master.when_lose_play.music)
                    my_game_master.Start_music(my_game_master.music_stage_lose, my_game_master.play_lose_music_in_loop);
                else if (my_game_master.when_lose_play_selected == game_master.when_lose_play.sfx)
                    my_game_master.Gui_sfx(my_game_master.music_stage_lose);

                if (my_game_master.show_score_in_lose_screen_too && show_int_score)
                {
                    StartCoroutine(Int_score_animation(0.5f, 0));

                    //if your int score is better than the previous
                    if (int_score > my_game_master.best_int_score_in_this_stage[my_game_master.current_profile_selected][n_world - 1, n_stage - 1])
                    {
                        //update int score
                        Update_int_score_record();
                    }
                }
            }
            Invoke("Mark_lose", 0.1f);
        }
    }



    void Mark_lose()
    {
        Mark_this_button(lose_screen_target_button);
    }

    void Show_defeat_ad()
    {
        my_game_master.my_ads_master.Call_ad(my_game_master.my_ads_master.ask_if_double_int_score);
    }

    IEnumerator Show_star_score(int stars_number)
    {
        if (stars_number == 3)
            perfect_target.sprite = perfect_emoticon;

        yield return new WaitForSeconds(delay_start_star_score_animation);

        invoke_count = 0;

        if (stars_number == 1)
        {
            Show_star(0);
        }
        else if (stars_number == 2)
        {
            Show_star(0);
            Show_star(1);
        }
        else if (stars_number == 3)
        {
            Show_star(0);
            Show_star(1);
            Show_star(2);
        }

        if (show_int_score)
            StartCoroutine(Int_score_animation(delay_star_creation * stars_number, 0));

    }

    void Show_star(int n_star)
    {
        stars_on[n_star].SetActive(true);
        Invoke("Star_sfx", delay_star_creation * n_star);
    }

    void Star_sfx()
    {
        if (invoke_count < 3)
        {
            stars_on[invoke_count].GetComponent<Animation>().Play("star");
            if (my_game_master)
            {
                my_game_master.Gui_sfx(my_game_master.show_big_star_sfx[invoke_count]);
            }

            invoke_count++;
        }
    }




}
