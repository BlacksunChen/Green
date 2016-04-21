using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Green;

// ReSharper disable InconsistentNaming

public class manage_menu_uGUI : MonoBehaviour
{

    [SerializeField] private EventSystem my_eventSystem = null;

    [HideInInspector] public Transform logo_screen;
    public float show_logo_for_n_seconds;
    [HideInInspector] public Transform home_screen;
    [HideInInspector] public Transform credit_screen;

    [HideInInspector] public Transform options_screen;
    options_menu my_options;
    [HideInInspector] public gift_manager my_gift_manager;
    [HideInInspector] public feedback_window my_feedback_window;
    [HideInInspector] public Transform no_lives_left_screen;
    [HideInInspector] public Text no_lives_left_countdown;
    [HideInInspector] public Transform worlds_screen_automatic;
    [HideInInspector] public Transform stages_screen_automatic;
    [HideInInspector] public Transform worlds_screen_manual;
    [HideInInspector] public manual_world_screen my_manual_world_screen;
    [HideInInspector] public Transform[] stages_screen_manual;
    [HideInInspector] public Transform manual_stage_screens_list;
    [HideInInspector] public Transform map_screen;
    [HideInInspector] public Transform multi_page_screen;
    [HideInInspector] public Transform store_screen;
    [HideInInspector] public Transform loading_screen;
    [HideInInspector] public Transform current_screen;
    [HideInInspector] public Transform score_ranck_screen;

    [HideInInspector] public GameObject store_ico;
    [HideInInspector] public Info_bar info_bar;
    [HideInInspector] public bool show_info_bar;
    [HideInInspector] public bool stage_ico_update_animation_is_running;
    [HideInInspector] public GameObject score_ranck_ico;



    //profiles
    [HideInInspector] public GameObject my_new_profile_window;
    [HideInInspector] public new_profile_pad my_new_profile_pad;
    [HideInInspector] public Text current_profile_name;
    [HideInInspector] public GameObject profile_button;
    [HideInInspector] public Transform profile_screen;
    [HideInInspector] public GameObject ask_confirmation_window_prefab;
    [HideInInspector] public bool update_world_and_stage_screen;

    //target buttons for gamepad navigation
    [HideInInspector] public GameObject home_screen_target_button;
    [HideInInspector] public GameObject credit_screen_target_button;
    [HideInInspector] public GameObject options_screen_target_button;
    [HideInInspector] public GameObject profile_screen_target_button;
    [HideInInspector] public GameObject new_profile_window_pad_target_button;
    [HideInInspector] public GameObject store_screen_target_button;
    [HideInInspector] public GameObject world_screen_target_button;
    [HideInInspector] public GameObject stage_screen_target_button;
    [HideInInspector] public GameObject no_live_screen_button;
    [HideInInspector] public GameObject score_ranck_target_button;

    //store
    [HideInInspector] public store_tabs my_store_tabs;
    //ads
    [HideInInspector] public GameObject internet_off_ico;

    #region stage screen

    int total_stage_icons_for_page;
    int stage_row_x;
    int stage_line_y;

    int[] pages; //[world_n]
    [HideInInspector] public GameObject[] first_stage_ico_in_this_page;
    [HideInInspector] public RectTransform stage_pages_container;
    [HideInInspector] public GameObject stage_page;
    [HideInInspector] public RectTransform scroll_pages;

    [HideInInspector] public GameObject stage_ico;
    [HideInInspector] public Transform pages_counter;
    [HideInInspector] public GameObject page_count_dot;
    public Sprite curret_page_count_dot;
    Image[] dots_array;
    Sprite page_off_dot;
    [HideInInspector] public GameObject scroll_snap_obj;

    [HideInInspector] public Transform world_container;
    [HideInInspector] public ScrollRect world_scroll;
    [HideInInspector] public GameObject world_ico;

    public AudioClip[] show_mini_star;

    public Sprite[] worlds_ico_imanges;
    public Sprite[] worlds_bk_imanges;
    [HideInInspector] public Sprite[] worlds_stage_icons;
    int current_world_show_in_stage_page = -1;


    #endregion

    CanvasScaler my_scale;

    game_master my_game_master;
    GameManager my_game_manager;

    manage_menu_uGUI this_script;

    void Check_internet()
    {
        if ((Application.internetReachability == NetworkReachability.NotReachable) ||
            !my_game_master.my_ads_master.Advertisement_isInitialized())
        {
            internet_off_ico.SetActive(true);
            Invoke("Check_internet", 1);
        }
        else
        {
            internet_off_ico.SetActive(false);
            my_game_master.my_ads_master.Initiate_ads();
        }
    }

    Transform level_select_screen;
    // Use this for initialization
    void Start()
    {
        /*
        if (game_master.game_master_obj)
        {
            my_game_master = (game_master) game_master.game_master_obj.GetComponent("game_master");
            my_gift_manager.my_game_master = my_game_master;

            if (my_game_master.my_ads_master)
            {
                my_game_master.my_ads_master.my_feedback_window = my_feedback_window;
                my_game_master.my_ads_master.my_gift_manager = my_gift_manager;
                my_game_master.my_ads_master.my_info_bar = info_bar;
            }

            score_ranck_ico.SetActive(my_game_master.show_int_score_rank);

            if (my_game_master.my_ads_master.enable_ads)
                Check_internet();
            else
                internet_off_ico.SetActive(false);
        }*/

        if (GameManager.Exists)
        {
            my_game_manager = GameManager.Instance;
        }

        this_script = this.gameObject.GetComponent("manage_menu_uGUI") as manage_menu_uGUI;
        my_options = options_screen.GetComponent<options_menu>();

        //adjust canvas scale
        my_scale = this.gameObject.GetComponent<CanvasScaler>();
        if (my_scale)
        {
            if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)
                my_scale.matchWidthOrHeight = 0.7f;
            else
                my_scale.matchWidthOrHeight = 0.75f;
        }

        //stage pages
        if (my_game_master.press_start_and_go_to_selected ==
            game_master.press_start_and_go_to.level_select_screen)
        {
            Generate_stage_screen();
        }

        //info_bar.Show_info_bar(false);
       
        if (my_game_master.go_to_this_screen == game_master.this_screen.level_select_screen)
            //return to home stage from a game stage
        {
            if (my_game_master.show_debug_messages)
                Debug.Log("return to home stage from a game stage");

            home_screen.gameObject.SetActive(false);

            if (my_game_master.press_start_and_go_to_selected ==
                game_master.press_start_and_go_to.level_select_screen)
            { 
                    Mark_current_screen(level_select_screen);
            }

            //start music when retur to home from a game stage
            my_game_master.Start_music(my_game_master.music_menu, true);

        }
    }

    public void Close_logo()
    {
        //start music when the game start
        my_game_master.Start_music(my_game_master.music_menu, true);

        Show_home_screen();

        if (!my_game_master.show_new_profile_window && my_game_master.my_ads_master.Check_app_start_ad_countdown())
            my_game_master.my_ads_master.Call_ad(
                my_game_master.my_ads_master.ads_just_after_logo_when_game_start_as_daily_reward);
    }

    public void Mark_current_screen(Transform this_screen)
    {
        if (my_game_master.show_debug_messages)
            Debug.Log(this_screen.name);

        current_screen = this_screen;
        current_screen.gameObject.SetActive(true);
        Show_info_bar();
    }

    public void Mark_this_button(GameObject target_button)
    {
        //Debug.Log(target_button.name + " = " + target_button.activeSelf);

        if (my_game_master.use_pad)
            my_eventSystem.SetSelectedGameObject(target_button);
    }

    void Show_info_bar()
    {
        if (show_info_bar)
        {
            if (current_screen == map_screen
                || current_screen == multi_page_screen
                || current_screen == worlds_screen_manual
                || current_screen == worlds_screen_automatic
                || current_screen == no_lives_left_screen
                || current_screen == store_screen
                )
                info_bar.Show_info_bar(true);
            else
                info_bar.Show_info_bar(false);
        }
        else
            info_bar.Show_info_bar(false);
    }

    void Setup_stage_page()
    {
        float stage_page_x = this.gameObject.GetComponent<RectTransform>().rect.width;
        float stage_page_y = stage_pages_container.rect.height;

        stage_page.GetComponent<LayoutElement>().minWidth = stage_page_x;
        stage_page.GetComponent<LayoutElement>().minHeight = stage_page_y;

        stage_row_x =
            Mathf.FloorToInt(stage_page_x/
                             (stage_page.GetComponent<GridLayoutGroup>().cellSize.x +
                              stage_page.GetComponent<GridLayoutGroup>().spacing.x));
        stage_line_y =
            Mathf.FloorToInt(stage_page_y/
                             (stage_page.GetComponent<GridLayoutGroup>().cellSize.y +
                              stage_page.GetComponent<GridLayoutGroup>().spacing.y));

        //Debug.Log (stage_page_y + " / (" + stage_page.GetComponent<GridLayoutGroup> ().cellSize.y + " + " + stage_page.GetComponent<GridLayoutGroup> ().spacing.y + ") = " + stage_line_y);
        //Debug.Log( stage_page_x + "," +  stage_page_y + " °°° " + stage_row_x + " *** " + stage_line_y);

        page_off_dot = page_count_dot.GetComponent<Image>().sprite;
        total_stage_icons_for_page = stage_row_x*stage_line_y;
        //Debug.Log (stage_row_x + " * " + stage_line_y + " = " + total_stage_icons_for_page);
        pages = new int[my_game_master.total_stages_in_world_n.Length];

    }

    void Show_home_screen()
    {

        bool you_must_refresh_stage_and_world_screen = my_game_master.refresh_stage_and_world_screens;
        my_game_master.refresh_stage_and_world_screens = false;

        game_master.game_is_started = true;
        logo_screen.gameObject.SetActive(false);
        Mark_current_screen(home_screen);

        Mark_this_button(home_screen_target_button);
    }

    void Manage_ESC()
        //device button work like back button in every screen, except home screen. In home screen it close the app (this behavior is REQUIRED for winphone store)
    {
        if (!my_game_master.a_window_is_open)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && my_game_master.allow_ESC)
            {
                //if homescreen
                if (home_screen.gameObject.activeSelf)
                {
                    if (my_game_master.show_debug_messages)
                        Debug.Log("Application.Quit()");
                    Application.Quit();
                }
                else
                    Back();

            }
        }
    }

    void Manage_pad_start()
    {
        if (!my_game_master.a_window_is_open)
        {
            if (Input.GetKeyDown(my_game_master.pad_start_button))
            {
                if (my_new_profile_pad.gameObject.activeSelf)
                    my_new_profile_pad.OK_button();
                else
                {
                    if (current_screen == home_screen)
                        Press_start();
                }
            }
        }
    }

    void Manage_pad_back()
    {
        if (Input.GetKeyDown(my_game_master.pad_back_button) && my_game_master.use_pad)
        {
            if (my_new_profile_pad.gameObject.activeSelf)
                my_new_profile_pad.Delete_last_character();
            else
            {
                if (!home_screen.gameObject.activeSelf)
                    Back();
            }
        }
    }

    void Update()
    {
        Manage_ESC();

        Manage_pad_back();
        Manage_pad_start();

    }

    void Generate_stage_screen()
    {
        
    }

    public void Press_start()
    {
        if (!my_game_master.a_window_is_open)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            home_screen.gameObject.SetActive(false);

            if (my_game_master.infinite_lives ||
                my_game_master.current_lives[my_game_master.current_profile_selected] > 0)
                Start_to_play();
            else //you not have any live to play
            {
                if (my_game_master.when_restart_selected == game_master.when_restart.give_lives_after_countdown)
                {
                    Mark_current_screen(no_lives_left_screen);
                    Mark_this_button(no_live_screen_button);
                    StartCoroutine(Update_lives_countdown());
                }
                else //give new lives now
                {
                    my_game_master.current_lives[my_game_master.current_profile_selected] =
                        my_game_master.if_not_continue_restart_with_lives;
                    Start_to_play();
                }
            }
        }
    }

    void Start_to_play()
    {
        if (my_game_master.press_start_and_go_to_selected ==
            game_master.press_start_and_go_to.level_select_screen)
        {
            Mark_current_screen(level_select_screen);
        }
    }

    IEnumerator Update_lives_countdown()
    {
        if (current_screen == no_lives_left_screen) //keep update the text only when the page is active
        {
            //if not exist a target time yet, note it now
            if (my_game_master.target_time[my_game_master.current_profile_selected].Year == 0001)
            {
                my_game_master.Set_date_countdown();
                my_game_master.Save(my_game_master.current_profile_selected);
            }

            no_lives_left_countdown.text = my_game_master.Show_how_much_time_left();
            yield return new WaitForSeconds(1);
            if (my_game_master.current_lives[my_game_master.current_profile_selected] > 0)
            {
                no_lives_left_screen.gameObject.SetActive(false);
                Mark_current_screen(home_screen);
            }
            else
                StartCoroutine(Update_lives_countdown());
        }
    }

    public void Go_to_score_rank_screen()
    {
        my_game_master.Gui_sfx(my_game_master.tap_sfx);
        home_screen.gameObject.SetActive(false);
        Mark_current_screen(score_ranck_screen);
        Mark_this_button(score_ranck_target_button);
    }

    public void Go_to_credit_screen()
    {
        my_game_master.Gui_sfx(my_game_master.tap_sfx);
        home_screen.gameObject.SetActive(false);
        Mark_current_screen(credit_screen);
        Mark_this_button(credit_screen_target_button);
    }

    public void Go_to_options_screen()
    {
        my_game_master.Gui_sfx(my_game_master.tap_sfx);
        home_screen.gameObject.SetActive(false);
        my_options.Start_me();
        Mark_current_screen(options_screen);
        Mark_this_button(options_screen_target_button);
    }

    public void Back()
    {
        if (!my_new_profile_window.activeSelf 
         && !my_new_profile_pad.gameObject.activeSelf 
         && !ask_confirmation_window_prefab.activeSelf)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            current_screen.gameObject.SetActive(false);

            if (my_game_master.press_start_and_go_to_selected ==
                game_master.press_start_and_go_to.level_select_screen)
            {
                Mark_current_screen(home_screen);
                Mark_this_button(home_screen_target_button);
            }
        }

        if (update_world_and_stage_screen)
        {
            my_options.Start_me();
        }
    }
}