using PashaBibko.Pacore.Editor.Caches;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
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
            DrawStaticFields(target);
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

        public static void DrawStaticFields(Object target)
        {
            Type type = target.GetType();
            StaticInspectorFieldCache.FieldData[] fields
                = StaticInspectorFieldCache.GetAllFieldsOfType(type);

            if (fields.Length == 0)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Static Fields (only applied at runtime)", EditorStyles.boldLabel);

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].Value = DrawStaticField(fields[i].Info, fields[i].Value);
            }
        }

        private static object DrawStaticField(FieldInfo field, object current)
        {
            Type type = field.FieldType;
            string label = field.Name;

            if (type == typeof(int))
                return EditorGUILayout.IntField(label, current as int? ?? 0);

            if (type == typeof(float))
                return EditorGUILayout.FloatField(label, current as float? ?? 0f);

            if (type == typeof(string))
                return EditorGUILayout.TextField(label, current as string ?? "");

            if (type == typeof(bool))
                return EditorGUILayout.Toggle(label, current as bool? ?? false);
            
            EditorGUILayout.LabelField(label, $"Unsupported type: {type.FullName}");
            return current;
        }
    }
}