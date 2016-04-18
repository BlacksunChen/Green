using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Green
{
    [CustomEditor(typeof(Star))]
    [System.Serializable]
    public class StarEditor : Editor
    {
        Star Target { get { return (Star)target; } }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Target.FsmState == null)
            {
                var e = EditorGUILayout.EnumPopup("Current State:", Star.e_State.Player);
            }
            else
            {
                EditorGUILayout.EnumPopup("Current State:", Target.State);
            }
        }
    }
}