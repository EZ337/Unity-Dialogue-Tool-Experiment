using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Unity.VisualScripting;
using System;

[CustomEditor(typeof(Condition))]
public class ConditionEditor : Editor
{

    public VisualTreeAsset VisualTree;
    public EnumField enumField;
    public ObjectField obj;
    public IntegerField intCompare;
    public Button compareBtn;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);

        // Find the EnumField and bind it to the serialized property
        enumField = root.Q<EnumField>("predicate");
        compareBtn = root.Q<Button>("evaluateCondition");
        obj = root.Q<ObjectField>("param1");
        intCompare = root.Q<IntegerField>("int-compare");
        compareBtn.RegisterCallback<ClickEvent>(EvaluateCondition);

        return root;
    }

    private void EvaluateCondition(ClickEvent evt)
    {
        ConditionPredicate predicate = (ConditionPredicate) enumField.value;
        bool result = false;

        switch (predicate)
        {
            case ConditionPredicate.GetLevel:
                result = Condition.GetLevel((Actor) obj.value, intCompare.value, ConditionComparator.LessThan);
                break;

            case ConditionPredicate.GetIsDead:
                result = Condition.GetDead((Actor)obj.value);
                break;
        }

        Debug.Log($"result is: {result}");
    }
}
