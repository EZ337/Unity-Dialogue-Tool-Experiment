using System;
using System.Collections;
using System.Collections.Generic;
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