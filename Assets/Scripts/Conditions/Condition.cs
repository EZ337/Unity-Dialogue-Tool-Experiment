using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class Condition
{

    public UnityEngine.Object obj;
    public MethodInfo function;
    public ConditionComparator comparator;
    public System.Object param2;
    public bool OR = false; // Whether this condition treated as AND or OR

    public string methodName;
    public string param2String = "";
    public UnityEngine.Object param2AsUnityObject;

    [HideInInspector]
    public bool param2Evaluation = false;

    public Condition(System.Object obj, MethodInfo function, ConditionComparator comparator, System.Object param2, bool OR, bool param2Evaluation)
    {

        if (function.ReturnType == typeof(void))
        {
            Debug.LogWarning($"{function} returns void. The condition will always be false");
        }


        this.obj = (UnityEngine.Object)obj;
        this.function = function;
        this.comparator = comparator;
        this.param2 = param2;
        this.OR = OR;
        this.param2Evaluation = param2Evaluation;

        methodName = function.Name;
        if (param2 != null )
        {
            param2String = param2.GetType().Name;
            param2AsUnityObject = param2 as UnityEngine.Object;
        }
    }

    public bool EvaluateCondition()
    {
        if (param2Evaluation)
        {
            return EvaluateParam2(obj, function, comparator, param2);
        }
        else
        {
            return Evaluate(obj, function, comparator, param2);
        }
    }

    /// <summary>
    /// Defines the implicit cconversion to a bool. Returns the evaluated condition
    /// </summary>
    /// <param name="condition">Condition to evaluate</param>
    public static implicit operator bool(Condition condition)
    {
        return condition.EvaluateCondition();
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
        // Function should take param2. Need to expand this later probably
        System.Object[] argsList = new System.Object[1] { param2 };
        System.Object ret = function.Invoke(obj, argsList); // Call the function
        
        if (ret is bool bRet)
        {
            // Just returns the result from the function. This means all the work is left to
            // the function itself. May need revisitation to uncomment below but for now, the work is done
            // by the function.
            return bRet;

            /*
            // returnedValue = 0 if true. 1 if false.
            int retVal = (bRet) ? 0 : 1;

            if (comparator == ConditionComparator.Equal)
            {
                // True means 0, false is otherwise... ik. Backwards
                // In casee I forget why: Remember in architecture, True means (A - B) = 0. Meaning they are the same
                // Compare() is a true boolean evaluation. So If checking equal, you want to check that there is no difference i.e.
                // result is 0.

                return Compare(retVal, comparator);
            }
            else if (comparator == ConditionComparator.NotEqual)
            {
                // Because we are checking NotEqual, that means we are checking that there IS a difference. In other words,
                // returnedValue should NOT be 0.

                return Compare(retVal, comparator);
            }
            else
            {
                Debug.LogWarning($"Only choose Equal or NotEqual for Bool returning functions. Returning false");
                return false;
            }

            */
        }

        Debug.LogWarning($"{function} Does not return a boolean. ConditionFunction returning false");
        return false;
    }

    private static bool Compare(int comparison, ConditionComparator comparator)
    {
        switch (comparator)
        {
            case ConditionComparator.EqualTo:
                return comparison == 0;
            case ConditionComparator.NotEqualTo:
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

public enum ConditionComparator
{
    EqualTo,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    NotEqualTo
}