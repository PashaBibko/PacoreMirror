using PashaBibko.Pacore.Attributes;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
    public sealed class InspectorReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is InspectorReadOnlyAttribute readOnlyAttribute)
            {
                GUI.enabled = false;

                label.text = readOnlyAttribute.Name ?? label.text; // Uses custom name if it exists
                EditorGUI.PropertyField(position, property, label);

                GUI.enabled = true;
            }
        }
    }
}