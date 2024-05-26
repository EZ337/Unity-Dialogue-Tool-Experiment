using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(MethodExposer))]
public class MethodExposerEditor : Editor
{
    private MethodInfo[] methods;
    private string[] methodNames;
    private int selectedMethodIndex = 0;

    private void OnEnable()
    {
        // Get all public methods from the target script
        methods = typeof(MethodExposer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        methodNames = methods.Select(m => m.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (methodNames.Length == 0)
        {
            EditorGUILayout.HelpBox("No methods available to select.", MessageType.Warning);
            return;
        }

        // Dropdown to select method
        selectedMethodIndex = EditorGUILayout.Popup("Methods", selectedMethodIndex, methodNames);

        if (GUILayout.Button("Invoke Method"))
        {
            InvokeSelectedMethod();
        }
    }

    private void InvokeSelectedMethod()
    {
        MethodInfo method = methods[selectedMethodIndex];

        // Create parameters array if the method has parameters
        ParameterInfo[] parameters = method.GetParameters();
        object[] parameterValues = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(string))
            {
                parameterValues[i] = EditorGUILayout.TextField("Parameter " + i, "");
            }
            else if (parameters[i].ParameterType == typeof(int))
            {
                parameterValues[i] = EditorGUILayout.IntField("Parameter " + i, 0);
            }
            else if (parameters[i].ParameterType == typeof(float))
            {
                parameterValues[i] = EditorGUILayout.FloatField("Parameter " + i, 0f);
            }
            else if (parameters[i].ParameterType == typeof(bool))
            {
                parameterValues[i] = EditorGUILayout.Toggle("Parameter " + i, false);
            }
            // Add more types as needed
        }

        method.Invoke(target, parameterValues);
    }
 
}
