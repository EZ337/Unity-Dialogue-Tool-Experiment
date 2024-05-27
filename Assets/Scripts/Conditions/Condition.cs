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
    

    /// <summary>
    /// Evaluates obj against param2 based off of function. Returns the comparison. Returns false if unable to compare
    /// </summary>
    /// <param name="obj">Object owning the function call. Must be thhe declaring type for function.
    /// It's also the subject of the condition</param>
    /// <param name="function">Condition predicate. The question being asked. Method with the [Condition] attribute</param>
    /// <param name="comparator">The comparison operator to check foro</param>
    /// <param name="param2">The other object to compare against</param>
    /// <returns>A valid comparison of true or false. False if unable to compare the two objects</returns>
    public static bool Evaluate(System.Object obj, MethodInfo function, ConditionComparator comparator, System.Object param2)
    {
        // All functions should not take args
        System.Object[] argsList = new System.Object[0];
        System.Object ret = function.Invoke(obj, argsList); // Call the function

        // Can we compare the two objects against each other? (It's a loose definition here)
        if (ret is IComparable comparableRet && param2 is IComparable comparableParam2)
        {
            try
            {
                return Compare(comparableRet.CompareTo(comparableParam2), comparator);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Condition - {function}: Cannot compare these two types with each other. Error is handled though. Let EZ know");
                Debug.LogError($"Condition Function Returning false.\n" + ex);
                return false;
            }
        }


        Debug.LogWarning($"Condition - {function}: Param1 and Param2 cannot be compared to each other. False");
        return false;
    }


    public static bool EvaluateParam2(System.Object obj, MethodInfo function, ConditionComparator comparator, System.Object param2)
    {
        // All functions should not take args
        System.Object[] argsList = new System.Object[1] { param2 };
        System.Object ret = function.Invoke(obj, argsList); // Call the function
        
        if (ret is bool bRet)
        {
            int retVal = (bRet) ? 1 : 0;
            return Compare(retVal, comparator);
        }

        Debug.LogWarning($"{function} Does not return a boolean. ConditionFunction returning false");
        return false;
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