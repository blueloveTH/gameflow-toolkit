using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MaskIntAttribute))]
public class MaskIntDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            position = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));

            var e = EditorGUI.EnumFlagsField(position,
                 (Enum)Enum.ToObject((attribute as MaskIntAttribute).enumType, property.intValue));
            property.intValue = Convert.ToInt32(e);
        }
        else
            EditorGUI.LabelField(position, label.text, "Use [] for integers.");
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
