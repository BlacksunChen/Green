using UnityEngine;
using System.Collections;
using BE;
using System;
using UnityEngine.UI;
namespace  Green
{
    public class GameTouch : MonoBehaviour, MobileRTSCamListner
    {
        void Awake()
        {
            MobileRTSCam.instance.Listner = this;
        }
        public void OnDrag(Ray ray)
        {
        }

        public void OnDragEnd(Ray ray)
        {

        }

        public void OnDragStart(Ray ray)
        {

        }

        public void OnLongPress(Ray ray)
        {

        }

        public void OnMouseWheel(float fValue)
        {

        }

        public void OnTouch(Ray ray)
        {

        }


        public void OnTouchDown(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if raycasted object was founded, keep it to thr trPreClicked
                var planet = hit.collider.GetComponent<Planet>();
                if(planet == null) Debug.LogError("OnTouchDown() Planet == null");
                if (_planetClickState == PlanetClickState.Nothing)
                {
                    _curSelectedPlanet = planet;
                    _planetClickState = PlanetClickState.ClickOnePlanet;
                    SelectPlanet(planet);
                }
                else if(_planetClickState == PlanetClickState.ClickOnePlanet)
                {
                    if (_preparedToSendSoldier)
                    {
                        _destinationPlanet = planet;
                        _planetClickState = PlanetClickState.WaitToClickDestinationPlanet;
                    }
                    else
                    {
                        UnselectPlanet();
                        SelectPlanet(planet);
                        _preparedToSendSoldier = false;
                    }
                }
            }
            else
            {
                UnselectPlanet();
                _curSelectedPlanet = null;
                _destinationPlanet = null;
                _planetClickState = PlanetClickState.Nothing;
                _preparedToSendSoldier = false;
            }
        }

        Color _selectedColor = new Color(173f/255f, 173f/255f, 173f/255f, 1f);
        Color _defaultColor = Color.white;

        void SelectPlanet(Planet p)
        {
            var render = p.GetComponent<SpriteRenderer>();
            render.color = _selectedColor;
        }

        public Button _okButton;
        public Text _soldierCount;

        public Scrollbar _soldierSelectBar;

        void EnableOKButtonToSendSoldier()
        {
            _okButton.enabled = true;
        }

        //按下OK确认派兵
        public void OnClickOK()
        {
            if (_preparedToSendSoldier)
            {
                int count = (int)(_soldierSelectBar.value *_curSelectedPlanet.PlayerSoldiers.Count);
                GameWorld.Instance.SendSoldier(_curSelectedPlanet, _destinationPlanet, count, SoldierType.Player);
                _preparedToSendSoldier = false;
            }
        }

        public void OnClickSendSoldier()
        {
            _preparedToSendSoldier = true;
        }

        void UnselectPlanet()
        {
            if (_curSelectedPlanet != null)
            {
                var render = _curSelectedPlanet.GetComponent<SpriteRenderer>();
                render.color = _defaultColor;
            }
            if (_destinationPlanet != null)
            {
                var render = _destinationPlanet.GetComponent<SpriteRenderer>();
                render.color = _defaultColor;
            }
        }
        public void OnTouchUp(Ray ray)
        {
            
        }

        // Use this for initialization
        void Start()
        {
            _soldierSelectBar.onValueChanged.AddListener(OnSelectSoliderBarScrollChanged);
        }

        Planet _curSelectedPlanet = null;
        Planet _destinationPlanet = null;
        enum PlanetClickState
        {
            //什么都没点
            Nothing,
            //点了一个星球
            ClickOnePlanet,
            //等待点击到达的星球
            WaitToClickDestinationPlanet,
            
        }

        bool _preparedToSendSoldier = false;

        PlanetClickState _planetClickState = PlanetClickState.Nothing;
        // Update is called once per frame
        void Update()
        {
        }

        void OnSelectSoliderBarScrollChanged(float dt)
        {
            if (_preparedToSendSoldier)
                _soldierCount.text = (dt*_curSelectedPlanet.PlayerSoldiers.Count).ToString();
        }
    }

}

