using PashaBibko.Pacore.Shared.Attributes;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
    public class InspectorReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is not InspectorReadOnlyAttribute roAttribute)
            {
                return;
            }

            using (new DisabledGUIBlock())
            {
                label.text = roAttribute.Name ?? label.text; // Uses custom name if it exists
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}