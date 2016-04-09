using UnityEngine;
using UnityEditor;
using Green;

namespace Green.Test
{
    [CustomEditor(typeof(Dithered))]
    [System.Serializable]
    //在界面上显示：添加Behavior， 调节权重，实时更新
    public class DitheredInspector : Editor
    {
        Dithered dithered {  get { return target as Dithered; } }

        void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            OnGUI_CreateButton();
            foreach (var behavior in dithered.Behaviors)
            {
                OnGUI_DrawBehavior(behavior);
            }
        }

        void OnGUI_CreateButton()
        {
            using (new HorizontalBlock())
            {
                if (GUILayout.Button("+", GUILayout.Width(40f)))
                {
                    Undo.RecordObject(dithered, "Dithered");
                    dithered.AddEmptyBehavior();
                }

                if (GUILayout.Button("-", GUILayout.Width(40f)))
                {
                    Undo.RecordObject(dithered, "Dithered");
                    dithered.RemoveLastBehavior();
                }
            }
        }
        void OnGUI_DrawBehavior(Dithered.BehaviorWrapper behavior)
        {
            var type = (SteeringBehavior.Type_)EditorGUILayout.EnumPopup("Behavior Type: ", behavior.Type);
            if (type != behavior.Type)
            {
                behavior.UpdateBehaviorType(dithered.Target, type);
            }
            using (new HorizontalBlock())
            {
                behavior.Priority = EditorGUILayout.FloatField("优先级: ", behavior.Priority);
                behavior.Weight = EditorGUILayout.FloatField("权重", behavior.Weight);
            }
        }
    }
}
