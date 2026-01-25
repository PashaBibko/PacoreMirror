using PashaBibko.Pacore.Editor.Caches;
using PashaBibko.Pacore.Attributes;
using UnityEditor;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(MonoScriptAttribute))]
    public class MonoScriptDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            /* I'm not sure if this will ever happen */
            if (attribute is not MonoScriptAttribute attr)
            {
                return; // Stops NullReferenceExceptions
            }
            
            /* Makes sure the type is a string */
            if (property.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError($"Attribute [{nameof(MonoScriptAttribute)}] must be attached to a string");
                return;
            }

            /* Draws the label of the script variable */
            Rect rect = EditorGUI.PrefixLabel(position, label);

            /* Fetches the script if there is a value assigned to the property */
            string propValue = property.stringValue;
            MonoScript script = null;
            if (!string.IsNullOrEmpty(propValue))
            {
                script = MonoScriptCache.Get(propValue);
            }

            /* Draws the selected script and checks for changes */
            script = EditorGUI.ObjectField(rect, script, typeof(MonoScript), allowSceneObjects: false) as MonoScript;
            if (!GUI.changed)
            {
                return; // No changes to check
            }

            /* Makes sure the script is valid */
            if (script is null)
            {
                property.stringValue = string.Empty; // No type means empty string
                return;
            }

            /* Fetches the type of the attached script, and checks if it is valid */
            Type type = script.GetClass();
            if (type is null || (attr.MonoType is not null && !attr.MonoType.IsAssignableFrom(type)))
            {
                Debug.LogError($"The script file [{script.name}] doesn't contain an assignable MonoBehaviour class");
                return;
            }
            
            /* If a forced inheritance has been set, checks its validity */
            if (attr.InheritedFrom is not null && !attr.InheritedFrom.IsAssignableFrom(type))
            {
                property.stringValue = type.FullName; // Still applies the changes to make it appear like it is functioning
                
                string attachedObject = property.serializedObject.targetObject.name;
                string inherited = attr.InheritedFrom.FullName;

                Debug.LogError
                (
                    $"Field [{property.name}] as part of [{attachedObject}] is invalid.\n" +
                    $"The type must inherit from [{inherited}]. Currently it is [{type.FullName}]"
                );
                return;
            }

            /* Assigns the name of the type to the property so it can be created */
            property.stringValue = type.FullName;
        }
    }
}