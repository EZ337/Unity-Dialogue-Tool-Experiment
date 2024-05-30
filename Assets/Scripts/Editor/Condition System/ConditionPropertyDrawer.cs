using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Condition))]
public class ConditionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            EditorGUI.BeginProperty(position, label, property);
        
            SerializedProperty obj = property.FindPropertyRelative("obj");
            SerializedProperty function = property.FindPropertyRelative("methodName");
            SerializedProperty comparator = property.FindPropertyRelative("comparator");
            SerializedProperty param2 = property.FindPropertyRelative("param2Value");
            SerializedProperty param2AsObject = property.FindPropertyRelative("param2AsUnityObject");
            SerializedProperty OR = property.FindPropertyRelative("or");

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

            if (property.boxedValue is Condition condition)
            {
                if (condition.Function.ReturnType == typeof(void))
                {
                    Debug.LogWarning($"{property.serializedObject.targetObject} " +
                        $"has function {condition.MethodName} which returns void." +
                        " Condition will evaluate to false");
                }
            }

            EditorGUI.indentLevel = nativeIndent;
            EditorGUI.EndProperty();

        }
        catch (System.Exception)
        {
            Debug.LogError("Failed to build Custom Conditions Property. Building Default Look");
            base.OnGUI(position, property, label);
        }
    }

}
