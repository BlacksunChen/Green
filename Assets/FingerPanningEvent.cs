using UnityEngine;
using System.Collections;
using Sov.AVGPart;

namespace Green
{
    public class FingerPanningEvent : MonoBehaviour
    {
        public void OnScriptContinue()
        {
            Debug.Log("Finish Animation!");
            ScriptEngine.Instance.Status.EnableNextCommand = true;
            ScriptEngine.Instance.NextCommand();
        }
    

    // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}