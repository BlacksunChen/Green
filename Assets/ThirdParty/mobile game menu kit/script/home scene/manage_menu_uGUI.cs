using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
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
    //[HideInInspector]
    public gift_manager my_gift_manager;
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

    public Transform level_sclect_screen;

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

    // Use this for initialization
    void Start()
    {

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
        stages_screen_manual = new Transform[manual_stage_screens_list.childCount];
        for (int i = 0; i < stages_screen_manual.Length; i++)
            stages_screen_manual[i] = manual_stage_screens_list.GetChild(i);

        Setup_stage_page();


        if (my_game_master.press_start_and_go_to_selected ==
            game_master.press_start_and_go_to.nested_world_stage_select_screen)
        {
            if (my_game_master.total_stages_in_world_n.Length == 1)
            {
                if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
                {
                    Generate_stage_screen(my_game_master.current_world[my_game_master.current_profile_selected]);
                }

            }
            else
            {
                if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.automatic)
                    Generate_world_screen();
                else if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.manual)
                    my_manual_world_screen.My_start();
            }
        }

        info_bar.Show_info_bar(false);

        if (my_game_master.go_to_this_screen == game_master.this_screen.home_screen)
            //is this scene not is load from a game stage
        {

            //show logo at the start
            if (logo_screen != null && show_logo_for_n_seconds > 0 && !game_master.logo_already_show)
            {
                Mark_current_screen(logo_screen);
                home_screen.gameObject.SetActive(false);
                game_master.logo_already_show = true;

                if (!my_game_master.show_new_profile_window)
                {
                    Update_profile_name(true);
                }
                Invoke("Close_logo", show_logo_for_n_seconds);
            }
            else //don't show logo at the start
            {
                if (!game_master.logo_already_show)
                {
                    game_master.logo_already_show = true;
                    if (!my_game_master.show_new_profile_window)
                    {
                        Update_profile_name(true);
                        if (my_game_master.my_ads_master.Check_app_start_ad_countdown())
                            my_game_master.my_ads_master.Call_ad(
                                my_game_master.my_ads_master.ads_just_after_logo_when_game_start_as_daily_reward);
                    }
                }
                //start music when the game start
                my_game_master.Start_music(my_game_master.music_menu, true);
                Show_home_screen();
            }
        }
        else if (my_game_master.go_to_this_screen == game_master.this_screen.stage_screen)
            //return to home stage from a game stage
        {
            if (my_game_master.show_debug_messages)
                Debug.Log("return to home stage from a game stage");

            home_screen.gameObject.SetActive(false);
            //Update_profile_name(true);
            if (my_game_master.press_start_and_go_to_selected ==
                game_master.press_start_and_go_to.nested_world_stage_select_screen)
            {
                if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
                {
                    //Mark_current_screen(stages_screen_automatic);
                    Go_to_this_world_stage_menu(
                        my_game_master.current_world[my_game_master.current_profile_selected]);
                    //Generate_stage_screen(my_game_master.current_world[my_game_master.current_profile_selected]);

                    Mark_this_button(stage_screen_target_button);
                }
                else if (my_game_master.stage_screen_generation_selected ==
                         game_master.stage_screen_generation.manual)
                {
                    Mark_current_screen(
                        stages_screen_manual[
                            my_game_master.current_world[my_game_master.current_profile_selected]]);
                }
            }
            else if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)
            {
                Mark_current_screen(map_screen);
            }
            else if (my_game_master.press_start_and_go_to_selected ==
                     game_master.press_start_and_go_to.single_screen_with_a_page_for_every_world)
            {
                Mark_current_screen(multi_page_screen);
            }
            else if (my_game_master.press_start_and_go_to_selected ==
                     game_master.press_start_and_go_to.straight_to_the_next_game_stage)
            {
                Mark_current_screen(home_screen);
            }
            else if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.level_select_screen)
            {
                if (my_game_master.show_debug_messages)
                    Debug.Log("return to home stage from a game stage");

                Mark_current_screen(level_sclect_screen);
            }

            Update_profile_name(true);

            my_game_master.my_ads_master.Call_ad(
                my_game_master.my_ads_master.ads_when_return_to_home_scene_from_a_stage);

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

        //profiles
        if (my_game_master.number_of_save_profile_slot_avaibles > 1)
        {
            if (my_game_master.show_new_profile_window)
            {
                if (my_game_master.require_a_name_for_profiles)
                {
                    if (my_game_master.use_pad)
                    {
                        my_new_profile_pad.My_start(0, false, home_screen_target_button);
                        home_screen.gameObject.SetActive(false);
                        Mark_this_button(new_profile_window_pad_target_button);
                    }
                    else
                    {
                        my_new_profile_window.GetComponent<new_profile_window>().My_start(0, false);
                        //my_new_profile_window.SetActive(true);
                    }
                }
                else
                {
                    my_game_master.Create_new_profile("");
                    you_must_refresh_stage_and_world_screen = true;
                }
            }
            //else
            //{
            //my_new_profile_window.SetActive(false);
            //}

            //Update_profile_name(false);
            profile_button.SetActive(true);
        }
        else
        {
            profile_button.SetActive(false);
            you_must_refresh_stage_and_world_screen = true;
        }

        //store
        if (my_game_master.store_enabled)
            store_ico.SetActive(true);
        else
            store_ico.SetActive(false);

        Update_profile_name(you_must_refresh_stage_and_world_screen);

    }

    public void Show_profile_screen()
    {
        home_screen.gameObject.SetActive(false);
        my_game_master.Gui_sfx(my_game_master.tap_sfx);

        Mark_current_screen(profile_screen);

        if (my_game_master.this_profile_have_a_save_state_in_it[0])
        {
            Mark_this_button(profile_screen_target_button.transform.GetChild(0).gameObject);
        }
        else
        {
            Mark_this_button(profile_screen_target_button.transform.GetChild(1).gameObject);
        }



    }

    public void Update_profile_name(bool update_world_and_stage)
    {
        if (my_game_master.profile_name[my_game_master.current_profile_selected] != "")
            current_profile_name.text = "Welcome   " +
                                        my_game_master.profile_name[my_game_master.current_profile_selected] + "  !";
        else
            current_profile_name.text = "";

        if (update_world_and_stage)
        {
            update_world_and_stage_screen = false;
            Update_world_and_stage_screen();
        }

        my_store_tabs.Update_buttons_in_windows();
    }

    void Update_world_and_stage_screen()
    {
        //Debug.Log("Update_world_and_stage_screen");
        if (my_game_master.press_start_and_go_to_selected ==
            game_master.press_start_and_go_to.nested_world_stage_select_screen)
        {
            //world
            if (my_game_master.total_stages_in_world_n.Length > 1)
            {
                if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.automatic)
                {
                    for (int i = 0; i < world_container.childCount; i++)
                    {
                        world_container.GetChild(i).GetComponent<world_ico_uGUI>().My_padlock();
                    }
                }
                else if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.manual)
                {
                    my_manual_world_screen.My_start();
                }
            }
            //stage
            if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
            {
                current_world_show_in_stage_page = -1;

                if (my_game_master.total_stages_in_world_n.Length == 1)
                    Select_this_world(0);

            }
            else if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.manual)
            {
                for (int i = 0; i < stages_screen_manual.Length; i++)
                    stages_screen_manual[i].GetComponent<manual_stage_screen>().My_start();
            }

        }
        else if (my_game_master.press_start_and_go_to_selected ==
                 game_master.press_start_and_go_to.single_screen_with_a_page_for_every_world)
        {
            multi_page_screen.GetComponent<single_screen>().My_start();
        }
        else if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)
        {
            map_screen.GetComponent<manage_linear_map>().My_start();
        }
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

    void Generate_world_screen()
    {
        if (world_container)
        {
            for (int i = 0; i < my_game_master.total_stages_in_world_n.Length; i++)
            {
                GameObject temp_world_ico = (GameObject) Instantiate(world_ico);
                if (i == 0)
                    world_screen_target_button = temp_world_ico;
                temp_world_ico.transform.SetParent(world_container, false);
                world_ico_uGUI _icon = (world_ico_uGUI) temp_world_ico.GetComponent("world_ico_uGUI");
                _icon.my_number = i;
                _icon.my_game_master = my_game_master;
                _icon.my_manage_menu_uGUI = this.GetComponent<manage_menu_uGUI>();
                _icon.My_start();


                if (i < worlds_ico_imanges.Length)
                {
                    temp_world_ico.GetComponent<Image>().sprite = worlds_ico_imanges[i];
                }
                else
                    temp_world_ico.GetComponent<Image>().sprite = worlds_ico_imanges[0];

            }
        }
    }

    void Select_this_world(int this_world_n)
    {
        my_game_master.current_world[my_game_master.current_profile_selected] = this_world_n;
        if (current_world_show_in_stage_page != this_world_n)
        {
            Reset_stage_screen();
            Generate_stage_screen(this_world_n);
        }
    }


    public void Go_to_this_world_stage_menu(int this_world_n)
    {
        Select_this_world(this_world_n);

        worlds_screen_automatic.gameObject.SetActive(false);
        worlds_screen_manual.gameObject.SetActive(false);
        Mark_current_screen(stages_screen_automatic);



        Mark_this_button(stage_screen_target_button);
    }

    void Reset_stage_screen()
    {
        for (int i = 0; i < stage_pages_container.childCount; i++)
        {
            Destroy(stage_pages_container.GetChild(i).gameObject);
        }
    }

    void Generate_stage_screen(int this_world_n)
    {
        int stage_icons_count = 0;
        current_world_show_in_stage_page = this_world_n;

        if (worlds_bk_imanges[this_world_n])
        {
            stages_screen_automatic.GetComponent<Image>().sprite = worlds_bk_imanges[this_world_n];
        }

        //count how much pages you need to show all stage_icons of this world
        pages[this_world_n] =
            (int)
                Mathf.Ceil((float) my_game_master.total_stages_in_world_n[this_world_n]/
                           (float) total_stage_icons_for_page);
        //Debug.Log (my_game_master.total_stages_in_world_n[this_world_n] + " / " +  total_stage_icons_for_page + " = " + pages[this_world_n]);
        if (pages[this_world_n] > 1)
            dots_array = new Image[pages[this_world_n]];

        //fill all pages with stage_icons
        first_stage_ico_in_this_page = new GameObject[pages[this_world_n]];
        for (int p = 0; p < pages[this_world_n]; p++)
        {
            GameObject temp_page = (GameObject) Instantiate(stage_page);
            temp_page.name = "stage_page_world_" + this_world_n.ToString();
            temp_page.transform.SetParent(stage_pages_container, false);

            for (int i = 0; i < total_stage_icons_for_page; i++) //fill this page with icons
            {
                if (stage_icons_count < my_game_master.total_stages_in_world_n[this_world_n])
                {
                    stage_icons_count++;
                    GameObject temp_ico = (GameObject) Instantiate(stage_ico);
                    temp_ico.transform.SetParent(temp_page.transform, false);
                    temp_ico.name = "Ico_stage_" + stage_icons_count.ToString();

                    //Note the first stage icon in each page to focus on it when page flip
                    if (i == 0)
                    {
                        first_stage_ico_in_this_page[p] = temp_ico;
                        if (p == 0)
                            stage_screen_target_button = temp_ico;
                    }



                    if (worlds_stage_icons[this_world_n])
                    {
                        temp_ico.GetComponent<Image>().sprite = worlds_stage_icons[this_world_n];
                    }



                    stage_ico_uGUI _icon = (stage_ico_uGUI) temp_ico.GetComponent("stage_ico_uGUI");
                    _icon.world_number = this_world_n;
                    _icon.stage_number = stage_icons_count;
                    _icon.my_manage_menu_uGUI = this_script;
                    _icon.my_game_master = my_game_master;
                    _icon.My_start();

                }
            }

            if (pages[this_world_n] > 1)
            {
                GameObject temp_page_dot = (GameObject) Instantiate(page_count_dot);
                dots_array[p] = temp_page_dot.GetComponent<Image>();

                if (p == 0)
                    dots_array[p].sprite = curret_page_count_dot;

                temp_page_dot.transform.SetParent(pages_counter, false);

            }
            else
            {
                pages_counter.gameObject.SetActive(false);
            }
        }

        if (pages[this_world_n] > 1)
            scroll_snap_obj.GetComponent<scroll_snap>().Start_me();
        else
        {
            scroll_snap_obj.GetComponent<ScrollRect>().enabled = false;
            scroll_snap_obj.GetComponent<scroll_snap>().enabled = false;
        }

    }

    public void Update_page_dot(int current_page)
    {
        if (my_game_master.show_debug_messages)
            Debug.Log("Update_page_dot");

        for (int i = 0; i < pages[my_game_master.current_world[my_game_master.current_profile_selected]]; i++)
        {

            if (i == current_page)
                dots_array[i].sprite = curret_page_count_dot;
            else
                dots_array[i].sprite = page_off_dot;
        }
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
            game_master.press_start_and_go_to.nested_world_stage_select_screen)
        {
            if (my_game_master.total_stages_in_world_n.Length > 1) //go to world screen
            {
                if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.automatic)
                {
                    Mark_current_screen(worlds_screen_automatic);
                    Mark_this_button(world_screen_target_button);
                    world_scroll.horizontalNormalizedPosition = 0;
                }
                else if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.manual)
                {
                    Mark_current_screen(worlds_screen_manual);
                    my_manual_world_screen.Focus_on_first_icon();
                }

            }
            else //go to stage screen
            {
                if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
                {
                    //current_world_show_in_stage_page = 0;
                    Mark_current_screen(stages_screen_automatic);
                    Mark_this_button(stage_screen_target_button);
                }
                else
                {
                    Mark_current_screen(stages_screen_manual[0]);
                    Mark_this_button(stage_screen_target_button);
                }
            }
        }
        else if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.map)
        {
            Mark_current_screen(map_screen);
            Mark_this_button(stage_screen_target_button);
            map_screen.GetComponent<manage_linear_map>()
                .Center_to_this_icon(
                    my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected
                        ],
                    my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected
                        ]);
        }
        else if (my_game_master.press_start_and_go_to_selected ==
                 game_master.press_start_and_go_to.single_screen_with_a_page_for_every_world)
        {
            Mark_current_screen(multi_page_screen);
            Mark_this_button(stage_screen_target_button);
        }
        else if (my_game_master.press_start_and_go_to_selected ==
                 game_master.press_start_and_go_to.straight_to_the_next_game_stage)
        {
            if (my_game_master.show_loading_screen)
                loading_screen.gameObject.SetActive(true);
            Application.LoadLevel("W" +
                                  (my_game_master.play_this_stage_to_progress_in_the_game_world[
                                      my_game_master.current_profile_selected] + 1).ToString() + "_Stage_" +
                                  (my_game_master.play_this_stage_to_progress_in_the_game_stage[
                                      my_game_master.current_profile_selected] + 1).ToString());
        }
        else if (my_game_master.press_start_and_go_to_selected == game_master.press_start_and_go_to.level_select_screen)
        {
            Mark_current_screen(level_sclect_screen);
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

    public void Go_to_store_screen(int store_tab)
    {
        my_game_master.Gui_sfx(my_game_master.tap_sfx);
        home_screen.gameObject.SetActive(false);
        no_lives_left_screen.gameObject.SetActive(false);

        map_screen.gameObject.SetActive(false);
        manual_stage_screens_list.gameObject.SetActive(false);

        stages_screen_manual[my_game_master.current_world[my_game_master.current_profile_selected]].gameObject.SetActive
            (false);
        worlds_screen_manual.gameObject.SetActive(false);

        stages_screen_automatic.gameObject.SetActive(false);
        worlds_screen_automatic.gameObject.SetActive(false);

        Mark_current_screen(store_screen);
        my_store_tabs.Select_this_tab(store_tab);
        Mark_this_button(store_screen_target_button);
    }

    public void Back()
    {
        if (!my_new_profile_window.activeSelf && !my_new_profile_pad.gameObject.activeSelf &&
            !ask_confirmation_window_prefab.activeSelf)
        {
            my_game_master.Gui_sfx(my_game_master.tap_sfx);
            current_screen.gameObject.SetActive(false);


            if (my_game_master.press_start_and_go_to_selected ==
                game_master.press_start_and_go_to.nested_world_stage_select_screen)
            {
                //check if go back to world screen or to home screen
                if (my_game_master.total_stages_in_world_n.Length > 1)
                {
                    if (my_game_master.stage_screen_generation_selected == game_master.stage_screen_generation.automatic)
                    {
                        if (current_screen == stages_screen_automatic)
                            Go_to_world_screen(true);
                        else
                        {
                            Mark_current_screen(home_screen);
                            Mark_this_button(home_screen_target_button);
                        }
                    }
                    else if (my_game_master.stage_screen_generation_selected ==
                             game_master.stage_screen_generation.manual)
                    {
                        if (current_screen == store_screen)
                            manual_stage_screens_list.gameObject.SetActive(true);

                        if (current_screen ==
                            stages_screen_manual[
                                my_game_master.current_world[my_game_master.current_profile_selected]])
                            Go_to_world_screen(true);
                        else
                        {
                            Mark_current_screen(home_screen);
                            Mark_this_button(home_screen_target_button);
                        }
                    }
                }
                else
                {
                    Mark_current_screen(home_screen);
                    Mark_this_button(home_screen_target_button);
                }
            }
            else
            {
                Mark_current_screen(home_screen);
                Mark_this_button(home_screen_target_button);
            }
        }

        if (update_world_and_stage_screen)
        {
            Update_profile_name(true);
            my_options.Start_me();
        }

        if (stage_ico_update_animation_is_running)
            Stage_icon_animation_aborted();


    }

    void Stage_icon_animation_aborted()
    {
        if (my_game_master.show_debug_messages)
            Debug.Log("Stage_icon_animation_aborted");

        my_game_master.dot_tail_turn_on[my_game_master.current_profile_selected][
            my_game_master.play_this_stage_to_progress_in_the_game_world[my_game_master.current_profile_selected],
            my_game_master.play_this_stage_to_progress_in_the_game_stage[my_game_master.current_profile_selected]] =
            true;
        Update_profile_name(true);
    }

    void Go_to_world_screen(bool from_stage_screen)
    {
        if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.automatic)
        {
            Mark_current_screen(worlds_screen_automatic);
            world_scroll.horizontalNormalizedPosition = 0;

            if (from_stage_screen)
                Mark_this_button(
                    world_container.GetChild(my_game_master.current_world[my_game_master.current_profile_selected])
                        .gameObject); //mark current world
            else
                Mark_this_button(world_screen_target_button); //mark first world
        }
        else if (my_game_master.world_screen_generation_selected == game_master.world_screen_generation.manual)
        {
            Mark_current_screen(worlds_screen_manual);

            if (from_stage_screen)
                my_manual_world_screen.Focus_on_current_world_icon();
            else
                my_manual_world_screen.Focus_on_first_icon();
        }

    }

    #region ads

    /*
	public void Call_ad(game_master.ad_options target_ad)
	{
		if (my_game_master.enable_ads && target_ad.this_ad_is_enabled)
			{
			if (target_ad.ignore_minimum_time_interval_between_ads 
			    || (my_game_master.minimum_time_interval_between_ads+my_game_master.time_of_latest_ad_showed) < Time.realtimeSinceStartup)
				{
				if (my_game_master.show_debug_messages)
					Debug.Log("ad pass time check");

				if (UnityEngine.Random.Range(1,100) <= target_ad.chance_to_open_an_ad_here)
					{
					if (my_game_master.show_debug_messages)
						Debug.Log("ads_just_after_logo_when_game_start_as_daily_reward" + " - random ok");

					if (target_ad.ask_to_player_if_he_want_to_watch_an_ad_before_start_it)
						{
						if (my_game_master.show_debug_messages)
							Debug.Log("ask");

						//decide the quantity of the reward (if it not is select by the player)
						if (target_ad.my_ad_reward != game_master.ad_reward.select_by_the_player)
						{
							if (target_ad.randrom_reward)
								{
								if (my_game_master.show_debug_messages)
									Debug.Log("random reward");

								//select the reward
								int reward_slot = (int)my_game_master.Choose(target_ad.chance_to_give_this_reward);
								my_game_master.current_reward_selected = (game_master.ad_reward)reward_slot;
								
								//select the quantity
								if (target_ad.randrom_reward_quantity_for_random_reward[reward_slot])
									{
									if (my_game_master.show_debug_messages)
										Debug.Log("set random quantity");
									my_game_master.current_quantity_reward_selected = UnityEngine.Random.Range(target_ad.min_reward_quantity_for_random_reward[reward_slot],
									                                                                           target_ad.max_reward_quantity_for_random_reward[reward_slot]);
									}
								else
									{
									if (my_game_master.show_debug_messages)
										Debug.Log("set quantity");
									my_game_master.current_quantity_reward_selected = target_ad.reward_quantity_for_random_reward[reward_slot];
									}
								}
							else
								{
								if (my_game_master.show_debug_messages)
									Debug.Log("specific reward reward");

								my_game_master.current_reward_selected = target_ad.my_ad_reward;

								if (target_ad.my_ad_reward == game_master.ad_reward.consumable_item)
								{
									if(target_ad.choose_a_random_consumable)
									{
										my_game_master.current_item_id_reward_selected = UnityEngine.Random.Range(target_ad.min_random_consumable_item_id,
										                                                                          target_ad.max_random_consumable_item_id+1);
									}
									else 
										my_game_master.current_item_id_reward_selected = target_ad.consumable_item_id;
								}

								if (target_ad.randrom_reward_quantity)
									my_game_master.current_quantity_reward_selected = UnityEngine.Random.Range(target_ad.min_reward_quantity,
									                                                                           target_ad.max_reward_quantity);
								else
									my_game_master.current_quantity_reward_selected = target_ad.reward_quantity;
								}
						}
						else 
						{
							if (my_game_master.show_debug_messages)
								Debug.Log("the reward will be chosen by the player");
							my_game_master.current_reward_selected = target_ad.my_ad_reward;

						}

						my_game_master.current_ad = target_ad;
						my_gift_manager.Start_me(target_ad.asking_text, //the message to show in the window
						                         my_game_master.current_reward_selected,//the kind of reward
						                         my_game_master.current_quantity_reward_selected);//quantity
						}
					else
						{
						if (my_game_master.show_debug_messages)
							Debug.Log("ad start automatically");
						my_game_master.current_ad = target_ad;


						if (my_game_master.current_ad == target_ad)
							my_game_master.Set_app_start_ad_countdown();

						//star ad
						my_game_master.Show_ad(false);//false = not rewarded
						}
					}
				else
					{
					if (my_game_master.show_debug_messages)
						Debug.Log("ads_just_after_logo_when_game_start_as_daily_reward" + " - random fail");
					}
				}
			else
				{
				if (my_game_master.show_debug_messages)
					Debug.Log("ad don't pass time check");
				}
			}
	}*/

    #endregion


}
