using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Condition))]
public class ConditionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Display ReadOnly Info
        try
        {
            EditorGUI.BeginProperty(position, label, property);
            int nativeIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            /* No longer needed since Condition has a ToString Method
            SerializedProperty obj = property.FindPropertyRelative("obj");
            SerializedProperty function = property.FindPropertyRelative("methodName");
            SerializedProperty comparator = property.FindPropertyRelative("comparator");
            SerializedProperty param2 = property.FindPropertyRelative("param2Value");
            SerializedProperty param2AsObject = property.FindPropertyRelative("param2AsUnityObject");
            SerializedProperty OR = property.FindPropertyRelative("or");

            string orText = (OR.boolValue) ? "OR" : "AND";
            string param2String = (string.IsNullOrEmpty(param2.stringValue)) ? "Null" : param2.stringValue; 

            // A super simple version just telling you what conditions you've made. I will make it editable and pretty later
            if (param2AsObject.objectReferenceValue == null)
            {
                EditorGUI.LabelField(position, $"{obj.objectReferenceValue.name}.{function.stringValue} Is" +
                    $" {comparator.enumDisplayNames[comparator.enumValueIndex]} {param2String} {orText}");
            }
            else
            {
                EditorGUI.LabelField(position,$"{obj.objectReferenceValue.name}.{function.stringValue} Is " +
                    $"{comparator.enumDisplayNames[comparator.enumValueIndex]} " +
                    $"{param2AsObject.objectReferenceValue} {orText}");
            }
            */

            if (property.boxedValue is Condition condition)
            {
                EditorGUI.LabelField(position, $"{condition}");

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
