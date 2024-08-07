using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean) {
            if (conditionProperty.boolValue) {
                EditorGUI.PropertyField(position, property, label, true);
            }
        } else {
            EditorGUI.LabelField(position, label.text, "Error: Invalid condition field");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean) {
            return conditionProperty.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : -EditorGUIUtility.standardVerticalSpacing;
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}