using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Text;


///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          MobileRTSCam
///   Description:    classes to manage camera input & control
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-02-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public interface MobileRTSCamListner {
		void OnTouchDown(Ray ray);
		void OnTouchUp(Ray ray);
		void OnTouch(Ray ray);
		void OnDragStart(Ray ray);
		void OnDragEnd(Ray ray);
		void OnDrag(Ray ray);
		void OnLongPress(Ray ray);
		void OnMouseWheel(float fValue);
	    void OnTouchZoom(float value);
	}

	public enum PinchType {
		None	= -1,
		Zoom	= 0, 	
		Rotate	= 1,	
		Up		= 2,
		Max		= 3,
	}
	
	public class MobileRTSCam : MonoBehaviour {

		public	static MobileRTSCam instance;

		public  MobileRTSCamListner Listner = null;
		
		public 	Transform 	trCamera=null;		// transform for zoom
		public 	Transform 	trCameraRoot=null;	// transform for move(panning)
		[HideInInspector]
		public	bool 		camPanningUse = true;

	    public  bool        CanTouchMoving = false;
		public	bool 		BorderUse = true;
		public 	float		XMin = -30.0f;		// Camera panning x limit
		public 	float		XMax =  30.0f;		// Camera panning x limit
	    public  float       YMax = 30f;
	    public  float       YMin = -30f;
		public 	float		ZMin = -30.0f;		// Camera panning z limit
		public 	float		ZMax =  30.0f;		// Camera panning z limit
		public 	float 		zoomMax = 50.0f;
		public 	float 		zoomMin = 20.0f;
		public 	float 		zoomCurrent = -34.0f;
		public 	float 		zoomSpeed = 2.0f;

	    public bool InXYPlane = true;
	    public bool Rotate = false;
		private bool 		bInTouch = false;
		private Vector3		vCamRootPosOld = Vector3.zero;
		private Vector3		mousePosStart = Vector3.zero;
		private Camera		camMain;
		private	bool 		Dragged = false;
	    private bool DraggedAndSelectedObj = false;
		public	Plane 		xzPlane;
		public  Ray 		ray;

		private Vector3 	vPickStart;
		private Vector3 	vPickOld;
		private Vector3 	vCameraPanDir;

		// Camera Panning Inertia Movement 
		public	bool		InertiaUse = true;
		private bool		InertiaActive = false;
		private Vector3 	InertiaSpeed;
		private float		InertiaAge = 0.0f;

		private bool    	InZoom = false;

		// variables for long touch down check
		private bool		LongTabCheck = true;
		public 	float	 	LongTabPeriod = 0.5f;
		private float 		ClickAfter = 0.0f;

		// disable cam panning for limited time
		private float		fCamPanLimit = 0.0f;

		private bool   		InPinch = false;
		private PinchType	pinchType = PinchType.None;
		private Vector3 	vPinchDirStart = Vector3.zero;
		private float 		fPinchDistanceStart = 0.0f;
		private Vector3 	vPinchPickCenterStart = Vector3.zero;
		private Vector3		vCamRootRotStart = Vector3.zero;
		private Vector3		vCamRootRot = Vector3.zero;
		private Vector2	[]	vTouchPosStart = new Vector2 [2];
		private float   	ZoomStart = 0.0f;


		// disable ui while camera is in dragging
		private GraphicRaycaster	gr;

	    public bool LockCamera = false;

		void Awake () {
			instance=this;
			gr = GameObject.Find ("Canvas").GetComponent<GraphicRaycaster>();
            if(InXYPlane)
                xzPlane = new Plane(new Vector3(0f, 0f, 1f), 0f);
		    else
                xzPlane = new Plane(new Vector3(0f, 1f, 0f), 0f); // set base plane to xzplane with height zero
			camMain = trCamera.GetComponent<Camera>();
		    if (InXYPlane)
		    {
		        camMain.orthographicSize = zoomMax;
		        zoomCurrent = zoomMax;
		    }
		    else
		    {
		        zoomCurrent = -trCamera.localPosition.z;
		    }

		}

		void Start () {
		}

		void Update ()
		{
		    if (LockCamera) return;
		    if (EventSystem.current.currentSelectedGameObject != null) return;
		    
			//inertia camera panning
			if(InertiaUse) {
				if(InertiaActive && (InertiaSpeed.magnitude > 0.01f)) {
					SetCameraPosition(trCameraRoot.position - InertiaSpeed);
					InertiaSpeed = Vector3.Lerp(InertiaSpeed, Vector3.zero, InertiaAge);
					InertiaAge += Time.smoothDeltaTime;
				}
				else {
					InertiaActive = false;
				}
			}

			if(fCamPanLimit > 0.0f)
				fCamPanLimit -= Time.deltaTime;

			if(Input.touchCount < 2) {
				if(InPinch) {
					InPinch = false;
					bInTouch = false;
					fCamPanLimit = 0.1f;
					pinchType = PinchType.None;
					camPanningUse = true;
				}
			}

			Vector3 vTouch = Input.mousePosition;
			ray = Camera.main.ScreenPointToRay(vTouch);
			float enter;
			
			//if left MouseButton down
			if(Input.GetMouseButton(0)) {
				
				if (EventSystem.current.IsPointerOverGameObject()) {
					//Debug.Log("left-click over a GUI element!");
					return;
				}
				
				xzPlane.Raycast(ray, out enter);

				if(!bInTouch) {
					bInTouch = true;
					gr.enabled = false;
					ClickAfter = 0.0f;
					LongTabCheck = true;
					Dragged = false;
					mousePosStart = vTouch;
					
					if(Listner != null)
						Listner.OnTouchDown(ray);
						
					// Get Picking Position
					xzPlane.Raycast(ray, out enter);
					vPickStart = ray.GetPoint(enter)-trCameraRoot.position;
					vPickOld = vPickStart;
					vCamRootPosOld = trCameraRoot.position;

					if(InertiaUse) {
						InertiaActive = false;
						InertiaAge = 0.0f;
						InertiaSpeed = Vector3.zero;
					}
					//Debug.Log ("Update buildingSelected:"+((buildingSelected != null) ? buildingSelected.name : "none"));
				}
				else {

					if(Input.touchCount < 2) {

						//Mouse Button is in pressed & mouse move certain diatance
						if(Vector3.Distance (vTouch, mousePosStart) > 0.01f) {
							
							// set drag flag on
						    if (!Dragged)
						    {
						        Dragged = true;
						        if (Listner != null) Listner.OnDragStart(ray);                  
						    }

						    if(Listner != null) Listner.OnDrag(ray);

						    if (CanTouchMoving)
						    {
						        if (camPanningUse)
						        {
						            Vector3 vPickNew = ray.GetPoint(enter) - trCameraRoot.position;
						            if (InertiaUse)
						            {
						                InertiaSpeed = 0.3f * InertiaSpeed + 0.7f * (vPickNew - vPickOld);
						            }
						            vCameraPanDir = vPickNew - vPickStart;
						            //Debug.Log ("vCameraPanDir:"+vCameraPanDir);
						            SetCameraPosition(vCamRootPosOld - vCameraPanDir);
						            vPickOld = vPickNew;
						        }
						    }
						}
						// Not Move
						else {

						    if (Dragged)
						    {
						        if (Listner != null) Listner.OnDrag(ray);
						        if (CanTouchMoving)
						        {
						            if (camPanningUse)
						            {
						                Vector3 vPickNew = ray.GetPoint(enter) - trCameraRoot.position;
						                if (InertiaUse)
						                {
						                    InertiaSpeed = 0.3f * InertiaSpeed + 0.7f * (vPickNew - vPickOld);
						                }
						                vPickOld = vPickNew;
						            }
						        }
						    }
						    else
						    {
						        if (!Dragged)
						        {
						            ClickAfter += Time.deltaTime;

						            if (LongTabCheck && (ClickAfter > LongTabPeriod))
						            {
						                LongTabCheck = false;
						                if (Listner != null) Listner.OnLongPress(ray);
						            }
						        }
						    }
						}
					}
				}
			}
			else {
				
				//Release MouseButton
				if(bInTouch) {
					bInTouch = false;
					gr.enabled = true;

					

					// if in drag state
				    if (Dragged)
				    {
				        if (InertiaUse && (InertiaSpeed.magnitude > 0.01f))
				            InertiaActive = true;
				        // if (DraggedAndSelectedObj)
				        // {
				        if (Listner != null) Listner.OnDragEnd(ray);
				        DraggedAndSelectedObj = false;
				        camPanningUse = true;
				        //}
				    }
				    else
				    {
				       // if (Listner != null) Listner.OnTouch(ray);
                        if (Listner != null) Listner.OnTouchUp(ray);
                    }
				}
			}
			
			if (!EventSystem.current.IsPointerOverGameObject()) {
                //zoom with mouse wheel
                float fInputValue = Input.GetAxis("Mouse ScrollWheel");
				if(Listner != null) Listner.OnMouseWheel(fInputValue);
				if(fInputValue != 0.0f) {
					
					if(!InZoom) {
						mousePosStart = vTouch;
						xzPlane.Raycast(ray, out enter);
						vPickStart = ray.GetPoint(enter)-trCameraRoot.position;
						vCamRootPosOld = trCameraRoot.position;
						InZoom = true;
					}
					
					float zoomDelta = fInputValue * zoomSpeed;
					SetOrthographicMouseScrollZoom(zoomCurrent-zoomDelta);
					//UpjustPickPos(vTouch, vPickStart);
				}
				else {
					if(InZoom)
						InZoom = false;
				}
			}

			// pinch zoom for mobile touch input
			if(Input.touchCount != 2) 
				return;

			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector3 vPinchDir = touchOne.position - touchZero.position;
			float   fPinchDistance = vPinchDir.magnitude;
			vPinchDir.Normalize();
				
			Vector3 vPinchTouchCenter = (touchOne.position - touchZero.position) * 0.5f + touchZero.position;
			ray = Camera.main.ScreenPointToRay(vPinchTouchCenter);
			xzPlane.Raycast(ray, out enter);
			Vector3 vPinchPickCenter = ray.GetPoint(enter)-trCameraRoot.position;
				
			if(!InPinch) {
				vTouchPosStart[0] = touchZero.position;
				vTouchPosStart[1] = touchOne.position;
				ZoomStart = zoomCurrent;
				vCamRootRotStart = trCameraRoot.localRotation.eulerAngles;
				vCamRootRot = vCamRootRotStart;
				vCamRootPosOld = trCameraRoot.position;
					
				vPinchDirStart = vPinchDir;
				fPinchDistanceStart = fPinchDistance;
				vPinchPickCenterStart = vPinchPickCenter;
				InPinch = true;
				camPanningUse = false;
			}
			else {
					
				Vector2 vTouchZeroDelta = touchZero.position - vTouchPosStart[0];
				Vector2 vTouchOneDelta = touchOne.position - vTouchPosStart[1];
				if((vTouchZeroDelta.magnitude > 1.0f) && (vTouchOneDelta.magnitude > 1.0f)) {

					float angleWithUp = Vector2.Angle(vTouchOneDelta, Vector2.up);
					float angleBetweenTouches = Vector2.Angle(vTouchZeroDelta, vTouchOneDelta);
					//Debug.Log ("angleWithUp:"+angleWithUp+"angleBetweenTouches:"+angleBetweenTouches);

					// check if pinch up
					if(((angleWithUp < 30.0f) || (150.0f < angleWithUp)) && (angleBetweenTouches < 50.0f)) {
						pinchType = PinchType.Up;
					}
					else if((angleBetweenTouches < 30.0f) || (150.0f < angleBetweenTouches)) {
						pinchType = PinchType.Zoom;
					}
					else {
						pinchType = PinchType.Rotate;
					}
				}
					
				if(pinchType == PinchType.Up) {
				    if (Rotate)
				    {
				        //rotate x
				        float fDelta = touchZero.deltaPosition.y*Time.deltaTime*10.0f;
				        vCamRootRot.x = Mathf.Clamp(vCamRootRot.x - fDelta, 10.0f, 90.0f);
				        trCameraRoot.localRotation = Quaternion.Euler(vCamRootRot);
				    }
				} 
				else { 
					//zoom
					float fDelta = fPinchDistance - fPinchDistanceStart;
				    {
				        var value = ZoomStart - fDelta*zoomSpeed*0.05f;
                        zoomCurrent = Mathf.Clamp(value, zoomMin, zoomMax);

                        Listner.OnTouchZoom(zoomCurrent);
                    }

				    if (Rotate)
				    {
				        // rotate y
				        Vector3 v1 = vPinchDirStart;
				        Vector3 v2 = vPinchDir;
				        float dot = v1.x*v2.x + v1.y*v2.y; //# dot product
				        float det = v1.x*v2.y - v1.y*v2.x; // # determinant
				        float angle = Mathf.Atan2(det, dot); //# atan2(y, x) or atan2(sin, cos)
				        angle *= Mathf.Rad2Deg;

				        vCamRootRot.y = vCamRootRotStart.y + angle;
				        trCameraRoot.localRotation = Quaternion.Euler(vCamRootRot);
				    }
				}

				if((pinchType == PinchType.Zoom) || (pinchType == PinchType.Rotate)) {
					UpjustPickPos(vPinchTouchCenter, vPinchPickCenterStart);
				}
			}
		}
	
		public void UpjustPickPos(Vector3 vTouch, Vector3 vPickStart) {
			Ray ray = Camera.main.ScreenPointToRay(vTouch);
			float enter;
			xzPlane.Raycast(ray, out enter);
			Vector3 vPickNew = ray.GetPoint(enter)-trCameraRoot.position;
			vCameraPanDir = vPickNew - vPickStart;
			SetCameraPosition(vCamRootPosOld - vCameraPanDir);
		}

		public void SetCameraPosition(Vector3 vPos) {
			if(BorderUse) {
			    if (InXYPlane)
			    {
                    vPos.x = Mathf.Clamp(vPos.x, XMin, XMax);
                    vPos.y = Mathf.Clamp(vPos.y, YMin, YMax);
                }
			    else
			    {
                    vPos.x = Mathf.Clamp(vPos.x, XMin, XMax);
                    vPos.z = Mathf.Clamp(vPos.z, ZMin, ZMax);
                }
				
			}
			trCameraRoot.position = vPos;
		}
        
		public void SetCameraZoom(float value) {
			zoomCurrent = Mathf.Clamp(value, zoomMin, zoomMax);
			if(camMain.orthographic) {
                Vector3 vTouch = Input.mousePosition;
                camMain.orthographicSize = zoomCurrent;
			} 
			else {
				trCamera.localPosition = new Vector3(0,0,-zoomCurrent);
			}
		}
        /*
        public void SetCameraZoom(Ray ray, float value)
        {
            zoomCurrent = Mathf.Clamp(value, zoomMin, zoomMax);
            if (camMain.orthographic)
            {
                //camMain.orthographicSize = zoomCurrent;
                SetOrthographicZoom(zoomCurrent);

            }
            else
            {
                trCamera.localPosition = new Vector3(0, 0, -zoomCurrent);
            }
        }
        */
	    void SetOrthographicZoom(Ray ray_, float value)
	    {
            zoomCurrent = Mathf.Clamp(value, zoomMin, zoomMax);
            RaycastHit hit;
            if (Physics.Raycast(ray_, out hit))
            {
                var oldSize = camMain.orthographicSize;
                var oldObjScreenPos = camMain.WorldToScreenPoint(hit.collider.transform.position);
                camMain.orthographicSize = zoomCurrent;
                var newObjScreenPos = camMain.WorldToScreenPoint(hit.collider.transform.position);
                camMain.orthographicSize = oldSize;
                var newObjWorldPos = camMain.ScreenToWorldPoint(newObjScreenPos);
                trCameraRoot.transform.Translate(-hit.collider.transform.position + newObjWorldPos);
                camMain.orthographicSize = zoomCurrent;
            }
            else
            {
                camMain.orthographicSize = zoomCurrent;
            }
        }
        void SetOrthographicMouseScrollZoom(float value)
	    {
	        SetOrthographicZoom(ray, value);
        }

        public void SetOrthographicTouchZoom(Ray ray_, float value)
        {
            SetOrthographicZoom(ray, value);
        }
        // Set Zoom value with 0.0 ~ 1.0 ratio of min to max value
        public void SetCameraZoomRatio(float fRatio) {
			float fRealValue = (zoomMax-zoomMin)*fRatio+zoomMin;
			SetCameraZoom(fRealValue);
		}

		void OnDrawGizmos(){
			Gizmos.color = Color.red;

			if(BorderUse) {
			    if (InXYPlane)
			    {
			        Gizmos.DrawLine(new Vector3(XMin, YMin, 0f), new Vector3(XMin, YMax, 0f));
			        Gizmos.DrawLine(new Vector3(XMin, YMin, 0f), new Vector3(XMax, YMin, 0f));
			        Gizmos.DrawLine(new Vector3(XMax, YMin, 0f), new Vector3(XMax, YMax, 0f));
			        Gizmos.DrawLine(new Vector3(XMin, YMax, 0f), new Vector3(XMax, YMax, 0f));
			    }
			    else
			    {
			        Gizmos.DrawLine(new Vector3(XMin, 0, ZMin), new Vector3(ZMax, 0, ZMin));
			        Gizmos.DrawLine(new Vector3(XMin, 0, ZMax), new Vector3(ZMax, 0, ZMax));
			        Gizmos.DrawLine(new Vector3(XMin, 0, ZMin), new Vector3(XMin, 0, ZMax));
			        Gizmos.DrawLine(new Vector3(ZMax, 0, ZMin), new Vector3(ZMax, 0, ZMax));
			    }
			}
		}
	}

}