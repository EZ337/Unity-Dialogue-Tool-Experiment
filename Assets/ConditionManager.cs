using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    /// <summary>
    /// List of conditions
    /// </summary>

    public List<Condition> Conditions = new List<Condition>();

    public bool EvaluateConditions()
    {
        // Loop through all conditions and check their validity
        // If any condition returns false, this whole thing is false. Otherwise,
        // We return true
        foreach (Condition condition in Conditions)
        {
            if (!condition)
            {
                return false;
            }
        }

        return true;
    }
}
