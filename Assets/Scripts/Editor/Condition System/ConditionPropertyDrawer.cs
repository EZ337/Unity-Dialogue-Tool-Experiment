using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Condition))]
public class ConditionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty obj = property.FindPropertyRelative("obj");
        SerializedProperty function = property.FindPropertyRelative("methodName");
        SerializedProperty comparator = property.FindPropertyRelative("comparator");
        SerializedProperty param2 = property.FindPropertyRelative("param2String");
        SerializedProperty param2AsObject = property.FindPropertyRelative("param2AsUnityObject");
        SerializedProperty OR = property.FindPropertyRelative("OR");

        int nativeIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        string orText = (OR.boolValue) ? "OR" : "AND";

        // A super simple version just telling you what conditions you've made. I will make it editable and pretty later
        if (param2AsObject.objectReferenceValue == null)
        {
            EditorGUI.LabelField(position, $"{obj.objectReferenceValue.name}.{function.stringValue} Is" +
                $" {comparator.enumDisplayNames[comparator.enumValueIndex]} {param2.stringValue} {orText}");
        }
        else
        {
            EditorGUI.LabelField(position,$"{obj.objectReferenceValue.name}.{function.stringValue} Is " +
                $"{comparator.enumDisplayNames[comparator.enumValueIndex]} " +
                $"{param2AsObject.objectReferenceValue.name}.{param2.stringValue} {orText}");
        }

        EditorGUI.indentLevel = nativeIndent;
        EditorGUI.EndProperty();
    }
}
