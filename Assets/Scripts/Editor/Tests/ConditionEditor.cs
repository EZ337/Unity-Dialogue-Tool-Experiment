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
    public EnumField predicateField;
    public EnumField comparatorField;
    public ObjectField obj;
    public IntegerField intCompare;
    public Button compareBtn;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);

        // Find the EnumField and bind it to the serialized property
        predicateField = root.Q<EnumField>("predicate");
        comparatorField = root.Q<EnumField>("comparator");
        compareBtn = root.Q<Button>("evaluateCondition");
        obj = root.Q<ObjectField>("param1");
        intCompare = root.Q<IntegerField>("int-compare");
        compareBtn.RegisterCallback<ClickEvent>(EvaluateCondition);

        // Picking the predicate changes the objectType of param1
        // This event handles that.
        predicateField.RegisterValueChangedCallback(OnPredicateChange);
        HideAllOptions();

        return root;
    }

    private void OnPredicateChange(ChangeEvent<Enum> evt)
    {
        switch ((ConditionPredicate) evt.newValue)
        {
            // GlobalVariable Param1
            case ConditionPredicate.GetGlobalVariable:
                Debug.LogWarning("GlobalVariables are not implemented yet. Comeback later");
                goto default;

            // Actor Param1 

            case ConditionPredicate.GetLevel:
                SetActorParameter();
                ShowAllOptions();
                break;
            case ConditionPredicate.GetIsDead:
                SetActorParameter();
                HideAllOptions();
                ShowElement(obj);
                break;



            default:
                // Disable the ObjectReference Field
                HideAllOptions();
                break;

        }
    }

    private void EvaluateCondition(ClickEvent evt)
    {
        ConditionPredicate predicate = (ConditionPredicate) predicateField.value;
        bool result = false;

        switch (predicate)
        {
            case ConditionPredicate.GetLevel:
                result = Condition.GetLevel((Actor) obj.value, intCompare.value, (ConditionComparator) comparatorField.value);
                break;

            case ConditionPredicate.GetIsDead:
                result = Condition.GetDead((Actor)obj.value);
                break;
        }

        Debug.Log($"result is: {result}");
    }

    private void SetActorParameter()
    {
        obj.objectType = typeof(Actor);
        obj.label = "Actor";
    }

    private void ShowElement(VisualElement elm)
    {
        elm.style.display = DisplayStyle.Flex;
    }

    private void HideElement(VisualElement elm)
    {
        elm.style.display = DisplayStyle.None;
    }

    private void ShowAllOptions()
    {
        obj.style.display = DisplayStyle.Flex;
        intCompare.style.display = DisplayStyle.Flex;
        comparatorField.style.display = DisplayStyle.Flex;
    }

    private void HideAllOptions()
    {
        obj.style.display = DisplayStyle.None;
        intCompare.style.display = DisplayStyle.None;
        comparatorField.style.display = DisplayStyle.None;
    }
}
