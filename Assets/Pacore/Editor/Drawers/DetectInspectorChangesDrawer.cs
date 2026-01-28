using PashaBibko.Pacore.Attributes;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(DetectInspectorChangesAttribute))]
    public sealed class DetectInspectorChangesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            /* Draws the property and checks for changes */
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            EditorGUI.PropertyField(position, property, label);
            if (EditorGUI.EndChangeCheck()) // Returns true if there were changes
            {
                property.serializedObject.ApplyModifiedProperties(); // Applies the changes
                if (attribute is DetectInspectorChangesAttribute inspectorChangesAttribute)
                {
                    const BindingFlags BINDING_FLAGS =
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                    string methodName = inspectorChangesAttribute.ActionName;

                    /* Fetches the method and the object to call it on */
                    Object target = property.serializedObject.targetObject;
                    MethodInfo method = target.GetType().GetMethod(methodName, BINDING_FLAGS);
                    if (method == null)
                    {
                        Debug.LogError($"Method not found [{methodName}]");
                    }

                    else
                    {
                        method.Invoke(target, null);
                    }
                }
            }
            
            EditorGUI.EndProperty();
        }
    }
}
