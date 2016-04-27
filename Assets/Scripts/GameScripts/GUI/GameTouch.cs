using UnityEngine;
using System.Collections;
using BE;
using System;
using UnityEngine.UI;
namespace  Green
{
    public class GameTouch : MonoBehaviour, MobileRTSCamListner
    {
        public Plane xzPlane;
        public Transform CameraTransform;
        ArrowRenderer _arrowRenderer;
        void Awake()
        {
            MobileRTSCam.instance.Listner = this;
            xzPlane = new Plane(new Vector3(0f, 0f, 1f), 0f);
            _arrowRenderer = GetComponent<ArrowRenderer>();
            if(_arrowRenderer == null)
                Debug.LogError("Need Script: ArrowRender on GmaeWorld!");
        }
        public void OnDrag(Ray ray)
        {
            if (!_dragStart) return;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var planet = hit.collider.GetComponent<Planet>();
                if (planet == null) Debug.LogError("OnTouchDown() Planet == null");
                _arrowRenderer.Draw(_curSelectedPlanet.transform.position,
                    planet.transform.position);
            }
            else
            {
                Vector3 vTouch = Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(vTouch);
                float enter;
                xzPlane.Raycast(ray, out enter);

                var vPickStart = ray.GetPoint(enter);
                _arrowRenderer.Draw(_curSelectedPlanet.transform.position,
                                   vPickStart);
            }
            
            
        }

        bool _dragStart = false;
        bool _destinationSelected = false;
        public void OnDragEnd(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var planet = hit.collider.GetComponent<Planet>();
                if (planet == null) Debug.LogError("OnTouchDown() Planet == null");
                _destinationPlanet = planet;
                _arrowRenderer.Draw(_curSelectedPlanet.transform.position,
                    planet.transform.position);
                _destinationSelected = true;
            }
            else
            {
                _destinationSelected = false;
                ClearDragArrow();
            }
            _dragStart = false;
        }

        void ClearDragArrow()
        {
            _arrowRenderer.Clear();
        }
        public void OnDragStart(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var planet = hit.collider.GetComponent<Planet>();
                if (planet == null) Debug.LogError("OnTouchDown() Planet == null");
                _curSelectedPlanet = planet;
                _dragStart = true;
            }
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
            /*
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if raycasted object was founded, keep it to thr trPreClicked
                var planet = hit.collider.GetComponent<Planet>();
                if(planet == null) Debug.LogError("OnTouchDown() Planet == null");
                if (_destinationSelected)
                {
                    _destinationSelected = false;                    
                }
                if (_planetClickState == PlanetClickState.Nothing)
                {
                    _curSelectedPlanet = planet;
                    _planetClickState = PlanetClickState.ShowProperty;
                    SelectPlanet(planet);
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
            */
            _destinationSelected = false;
            ClearDragArrow();
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
            if (_destinationSelected)
            {
                int count = (int) (_soldierSelectBar.value*_curSelectedPlanet.PlayerSoldiers.Count);
                GameWorld.Instance.SendSoldier(_curSelectedPlanet, _destinationPlanet, count, SoldierType.Player);
                _destinationSelected = false;
                ClearDragArrow();
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
            
            ShowProperty
            
        }

        bool _preparedToSendSoldier = false;

        PlanetClickState _planetClickState = PlanetClickState.Nothing;
        // Update is called once per frame
        void Update()
        {
            if (_destinationSelected)
            {
                _arrowRenderer.Draw(_curSelectedPlanet.transform.position,
                    _destinationPlanet.transform.position);
            }
            if (_destinationSelected)
            {
                var count = (int) (_curSelectedPlanet.PlayerSoldiers.Count * _soldierSelectBar.value);
                _soldierCount.text = count.ToString();
            }
            else
            {
                _soldierCount.text = "";
            }
        }

        void OnSelectSoliderBarScrollChanged(float dt)
        {
            if (_destinationSelected)
                _soldierCount.text = (dt*_curSelectedPlanet.PlayerSoldiers.Count).ToString();
            else
            {
                _soldierCount.text = "";
            }
        }
    }

}

