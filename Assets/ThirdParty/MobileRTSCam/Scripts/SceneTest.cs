using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;  

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          SceneTest
///   Description:    test scene for MobileRTSCamera
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2016-03-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class SceneTest : MonoBehaviour, MobileRTSCamListner {
		
		private	Transform	trSelected=null;
		private Transform	trPreClicked = null;
		private bool		LongPressed = false;

		void Start () {
			// Set this as an Listner of MobileRTSCam
			MobileRTSCam.instance.Listner = this;
			MobileRTSCam.instance.SetCameraZoomRatio(0.7f);
		}
		
		void Update () {
			
		}

		// Select object
		public void SelectObject(Transform trNew) {

			// if previous selected object exist
			if((trNew != trSelected) && (trSelected != null)) {
				// set scale to 2.0f
				trSelected.localScale = new Vector3(2.0f,2.0f,2.0f);
				trSelected = null;
			}
				
			trSelected = trNew;

			// if newly selected object exist
			if(trSelected != null) {
				// set scale to 2.4f
				trSelected.localScale = new Vector3(2.4f,2.4f,2.4f);
			}
		}

		//MobileRTSCam implement
		// when touch(mouse down) started
		public void OnTouchDown(Ray ray) {

			LongPressed = false;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				// if raycasted object was founded, keep it to thr trPreClicked
				trPreClicked = hit.collider.transform;
			}
			else {
				trPreClicked = null;
			}
		}
		
		public void OnTouchUp(Ray ray) {
		}

		// when touch down , town up occured
		public void OnTouch(Ray ray) {

			RaycastHit hit;
			if(trSelected == null) {
				if (Physics.Raycast(ray, out hit) && !LongPressed) {
					// if raucasted object founded & this is not a longPressed state
					// select object
					SelectObject(hit.collider.transform);
				}
			}
			else {
				
				if(trPreClicked != trSelected)
					SelectObject(null);
				
				if (Physics.Raycast(ray, out hit)) {
					SelectObject(hit.collider.transform);
				}
			}
		}
		
		public void OnDragStart(Ray ray) {

			// if selected object exist
			if((trSelected != null) && (trPreClicked == trSelected)) {
				// disable camara panning & ineritia
				MobileRTSCam.instance.camPanningUse = false;
				MobileRTSCam.instance.InertiaUse = false;
				
				if(trPreClicked == trSelected) {
				}
			}
		}
		
		public void OnDrag(Ray ray) {
			
			if((trSelected != null) && (trPreClicked == trSelected)) {

				// change position of selected object
				float enter=0.0f;
				MobileRTSCam.instance.xzPlane.Raycast(ray, out enter);
				trSelected.localPosition = ray.GetPoint(enter);
			}
		}
		
		public void OnDragEnd(Ray ray) {
			
			// if selected object exist
			if(trSelected != null) {
				// enable camara panning & ineritia
				MobileRTSCam.instance.camPanningUse = true;
				MobileRTSCam.instance.InertiaUse = true;

				// unselect object
				if((trPreClicked != null) && (trPreClicked == trSelected)) {
					SelectObject(null);
				}
			}
			
			trPreClicked = null;
		}
		
		public void OnLongPress(Ray ray) {

			// if user touch over object long period
			LongPressed = true;
			RaycastHit hit;
			if(trSelected == null) {
				if (Physics.Raycast(ray, out hit)) {
					//select object
					SelectObject(hit.collider.transform);
				}
			}
		}

		public void OnMouseWheel(float fValue) {
		}

        public void OnTouchZoom(float value)
        {
            throw new NotImplementedException();
        }
    }
}