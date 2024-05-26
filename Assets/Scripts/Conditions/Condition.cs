using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField, HideInInspector]
    ConditionPredicate conditionPredicate;

    public static bool GetLevel(Actor actor, int level, ConditionComparator comparator)
    {
         return Compare(actor.Level.CompareTo(level), comparator);
    }

    public static bool GetDead(Actor actor)
    {
        return actor.IsDead;
    }
    
    public static void Evaluate(System.Object obj, MethodInfo function, ConditionComparator comparator, System.Object param2)
    {
        System.Object[] argsList = new System.Object[0];
        System.Object ret = function.Invoke(obj, argsList);
        if (ret is IComparable comparableRet && param2 is IComparable comparableParam2)
        {
            Debug.Log(comparableRet.CompareTo(comparableParam2));
        }
        
    }

    private static bool Compare(int comparison, ConditionComparator comparator)
    {
        switch (comparator)
        {
            case ConditionComparator.Equal:
                return comparison == 0;
            case ConditionComparator.NotEqual:
                return comparison != 0;
            case ConditionComparator.GreaterThan:
                return comparison > 0;
            case ConditionComparator.GreaterThanOrEqual:
                return comparison >= 0;
            case ConditionComparator.LessThan:
                return comparison < 0;
            case ConditionComparator.LessThanOrEqual:
                return comparison <= 0;
        }

        return false;
    }
}

public enum ConditionPredicate
{
    None,
    GetLevel,
    GetIsDead,
    GetGlobalVariable,
}

public enum ConditionComparator
{
    Equal,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    NotEqual
}