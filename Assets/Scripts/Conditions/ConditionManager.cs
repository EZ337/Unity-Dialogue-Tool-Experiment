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
        bool totalLogic = true;
        bool evaluatingOrGroup = false;
        bool orGroupResult = false;

        // Loop through all conditions and check their validity
        // If any condition returns false, this whole thing is false. Otherwise,
        // We return true
        // Conditions are evaluated with OR Superiority. Meaning A*B+C*D+E results in A*(B+C)*(D+E)
        for (int i = 0; i < Conditions.Count; i++)
        {
            // Short circuit if at any point, our running total is false
            if (!totalLogic) { break; }

            Condition condition = Conditions[i];
            bool currentResult = condition;

            // If this condition is an OR condition, Enable orGroupLogic
            if (condition.OR)
            {
                evaluatingOrGroup = true;
                // the total of the orGroup so far is true or whatever this condition's result is
                orGroupResult = orGroupResult || currentResult;

                if (debug)
                {
                    Debug.Log($"OR Condition: {condition} evaluated to {currentResult}. Current OR Group Result: {orGroupResult}");
                }

                // If it's the last condition in our list, finalize the OR group evaluation
                if (i == Conditions.Count - 1)
                {
                    totalLogic = totalLogic && orGroupResult;
                }
            }
            else
            {
                // If this is an AND condition mixed with preceding OR conditions
                if (evaluatingOrGroup)
                {
                    // Finalize the OR group evaluation
                    totalLogic = totalLogic && (orGroupResult || currentResult);
                    evaluatingOrGroup = false;
                    orGroupResult = false;
                }
                else if (!currentResult)
                {
                    totalLogic = currentResult;
                    if (debug)
                    {
                        Debug.Log($"{condition} is False.");
                    }
                }
            }
            
        }

        if (debug)
        {
            Debug.Log("Coalesced Conditions: " + totalLogic);
        }
        return totalLogic;
    }


    /// <summary>
    /// Important because of the nonSerializable information in Condition, we should invalidate
    /// the non-serializable field so that they are reconstructed as needed
    /// </summary>
    private void OnValidate()
    {
        List<Condition> ToRemove = new List<Condition>();
        foreach (Condition condition in Conditions)
        {
            
            if (!condition.IsValid)
            {
                ToRemove.Add(condition);
                continue;
            }
            else
            {
                // Worth defining a custom OnValidate for Condition
                condition.Reconstruct();
            }
        }

        foreach (Condition condition in ToRemove)
        {
            Conditions.Remove(condition);
            Debug.LogWarning("Invalid Condition Removed");
        }
    }
}
