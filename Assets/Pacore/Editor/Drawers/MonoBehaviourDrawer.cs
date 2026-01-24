using System;
using System.Collections.Generic;
using System.Reflection;
using PashaBibko.Pacore.Shared.Attributes;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    public class MonoBehaviourDrawer : UnityEditor.Editor
    {
        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        private InspectorCallableAttribute[] GetTypeButtons()
        {
            Type type = target.GetType();
            MethodInfo[] methods = type.GetMethods(BINDING_FLAGS);
            List<InspectorCallableAttribute> buttons = new();

            foreach (MethodInfo method in methods)
            {
                InspectorCallableAttribute attribute = method.GetCustomAttribute<InspectorCallableAttribute>();
                if (attribute != null)
                {
                    buttons.Add(attribute);
                }
            }
            
            return buttons.ToArray();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            InspectorCallableAttribute[] buttons = GetTypeButtons();
            if (buttons.Length == 0)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

            foreach (InspectorCallableAttribute button in buttons)
            {
                GUILayout.Button(button.ButtonName);
            }
        }
    }
}