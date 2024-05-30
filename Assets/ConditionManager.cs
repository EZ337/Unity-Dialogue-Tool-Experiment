using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    /// <summary>
    /// List of conditions
    /// </summary>

    public List<Condition> Conditions = new List<Condition>();

    public bool EvaluateConditions(bool debug = false)
    {
        // Loop through all conditions and check their validity
        // If any condition returns false, this whole thing is false. Otherwise,
        // We return true
        foreach (Condition condition in Conditions)
        {
            if (!condition)
            {
                if (debug)
                {
                    Debug.Log($"{condition.Obj.name}.{condition.MethodName} " +
                        $"{condition.Comparator} {condition.Param2} evaluated to false. Returning false...");
                }
                return false;
            }

            Debug.Log($"{condition.Obj.name}.{condition.MethodName} {condition.Comparator} " +
                $"{condition.Param2} evaluated to true. Next...");
        }

        return true;
    }
}
