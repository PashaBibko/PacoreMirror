using PashaBibko.Pacore.Editor.Caches;
using UnityEditor;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    public class MonoBehaviourDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawFunctionButtons(target);
        }
        
        public static void DrawFunctionButtons(Object target)
        {
            Type type = target.GetType();
            InspectorCallableCache.AttributeInfo[] buttons
                = InspectorCallableCache.GetAllAttributesOfType(type);
            
            if (buttons.Length == 0)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

            foreach (InspectorCallableCache.AttributeInfo button in buttons)
            {
                string name = button.Attribute.ButtonName;
                if (GUILayout.Button(name))
                {
                    button.AttachedMethod.Invoke(target, null);
                }
            }
        }
    }
}
