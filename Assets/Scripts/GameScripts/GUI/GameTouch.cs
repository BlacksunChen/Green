using UnityEngine;
using BE;
using UnityEngine.UI;
using Utilities;
using System;

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
            PropertyPanel = GameObject.FindObjectOfType<PlanetPropertyPanel>();
            if (PropertyPanel == null)
            {
                Debug.LogError("Need PlanetPropertyPanel in canvas!");
            }
            PropertyPanel.gameObject.SetActive(false);
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Need Canvas in canvas!");
            }
            ProgressBar = GameObject.Find("ProgressBar").GetComponent<ProgressRadialBehaviour>();
            if (ProgressBar == null)
            {
                Debug.LogError("Need Progress Active in Canvas");
            }
            ProgressBar.Value = 1;
            ProgressBar.gameObject.SetActive(false);
        }

        public float ProgressDragSpeed = 1f;

        public void OnDrag(Ray ray)
        {        
            //_curSelectedPlanet = 
            if (DragProgressBar)
            {
                if (!_lastDragPosition.HasValue && !_curDragPosition.HasValue)
                {
                    return;
                }
                _lastDragPosition = _curDragPosition;
                float enter;
                xzPlane.Raycast(ray, out enter);
                _curDragPosition = ray.GetPoint(enter) - CameraTransform.position;
                
                float addProgress = (_curDragPosition.Value.y - _lastDragPosition.Value.y) * ProgressDragSpeed;
                ProgressBar.Value += addProgress;
                return;
            }
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
            if (DragProgressBar)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var planet = hit.collider.GetComponent<Planet>();
                if (planet == null) Debug.LogError("OnTouchDown() Planet == null");
                if (planet != _curSelectedPlanet)
                {
                    _destinationPlanet = planet;
                    _arrowRenderer.Draw(_curSelectedPlanet.transform.position,
                        planet.transform.position);
                    _destinationSelected = true;
                    OnShowProgressBar();
                }
            }
            else
            {
                _destinationSelected = false;
                ClearDragArrow();
                OnCloseProgressBar();
            }
            _dragStart = false;
            _curDragPosition = null;
            _lastDragPosition = null;
        }

        public ProgressRadialBehaviour ProgressBar;

        public bool DragProgressBar = false;

        void OnShowProgressBar()
        {
            DragProgressBar = true;
            ProgressBar.gameObject.SetActive(true);
            var screenPos = GameSceneCamera.WorldToScreenPoint(_destinationPlanet.transform.position);
            var tran = ProgressBar.GetComponent<RectTransform>();
          //  RectTransformUtility.WorldToScreenPoint()
            //RectTransformUtility.WorldToScreenPoint(canvas.)
            tran.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);    
        }

        public void OnCloseProgressBar()
        {
            ProgressBar.gameObject.SetActive(false);
            DragProgressBar = false;
            _destinationSelected = false;
        }

        void ClearDragArrow()
        {
            _arrowRenderer.Clear();
        }

        Vector2? _lastDragPosition;
        Vector2? _curDragPosition;
        public void OnDragStart(Ray ray)
        {
            if (DragProgressBar)
            {
                float enter;
                xzPlane.Raycast(ray, out enter);
                _curDragPosition = ray.GetPoint(enter) - CameraTransform.position;
                MobileRTSCam.instance.camPanningUse = false;
                return;
            }
            RaycastHit hit;
      
            if (Physics.Raycast(ray, out hit))
            {
                MobileRTSCam.instance.camPanningUse = false;
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
            if (DragProgressBar) return;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if raycasted object was founded, keep it to thr trPreClicked
                var planet = hit.collider.GetComponent<Planet>();
                if (planet == null) Debug.LogError("OnTouchDown() Planet == null");

                ShowPanel(planet);
                /*
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
                */
            }
            else
            {
                // UnselectPlanet();
                UnshowPanel();
                 _curSelectedPlanet = null;
                //_destinationPlanet = null;
                // _planetClickState = PlanetClickState.Nothing;
                // _preparedToSendSoldier = false;
            }

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

        void CancelAllAction()
        {
            UnshowPanel();
            OnCloseProgressBar();
            _destinationSelected = false;
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

        public PlanetPropertyPanel PropertyPanel;
        public Camera GameSceneCamera;
        public Canvas canvas;
        public float PanelOffsetX = 0f;
        public float PanelOffsetY = 0f;
        void ShowPanel(Planet planet)
        {
            var pos = planet.transform.position;
            var star = planet.GetComponent<Star>();
            PropertyPanel.gameObject.SetActive(true);
            var screenPos = GameSceneCamera.WorldToScreenPoint(pos);
            var tran = PropertyPanel.GetComponent<RectTransform>();
            //RectTransformUtility.WorldToScreenPoint(canvas.)
            PropertyPanel.transform.position = new Vector3(screenPos.x + PanelOffsetX, screenPos.y + PanelOffsetY, screenPos.z);
            PropertyPanel.Show(star);
        }

        void UnshowPanel()
        {
            PropertyPanel.gameObject.SetActive(false);
        }

        public void OnTouchZoom(float value)
        {
            if (_curSelectedPlanet)
            {
                var screenPos = GameSceneCamera.WorldToScreenPoint(_curSelectedPlanet.transform.position);
                var ray = GameSceneCamera.ScreenPointToRay(screenPos);
                MobileRTSCam.instance.SetOrthographicTouchZoom(ray, value);
            }
        }
    }

}