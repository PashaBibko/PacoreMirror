using System;
using System.Collections.Generic;
using System.Reflection;
using PashaBibko.Pacore.Shared.Attributes;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Drawers
{
    public static class InspectorCallableAttributeCache
    {
        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        private static Dictionary<Type, InspectorCallableAttribute[]> Cache { get; } = new();

        public static InspectorCallableAttribute[] GetAllAttributes(Type type)
        {
            /* Checks the cache for the type */
            if (Cache.TryGetValue(type, out InspectorCallableAttribute[] attributes))
            {
                return attributes;
            }
            
            /* Finds all the functions with the attribute */
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
            
            /* Adds it to the cache before returning */
            InspectorCallableAttribute[] array = buttons.ToArray();
            Cache.Add(type, array);
            return array;
        }
    }

    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
    public class MonoBehaviourDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Type type = target.GetType();
            DrawFunctionButtons(type);
        }
        
        public static void DrawFunctionButtons(Type type)
        {
            InspectorCallableAttribute[] buttons = InspectorCallableAttributeCache.GetAllAttributes(type);
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