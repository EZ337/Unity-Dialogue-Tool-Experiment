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
            Condition condition = Conditions[i];

            // If this condition is an OR condition, Enable orGroupLogic
            if (condition.OR)
            {
                evaluatingOrGroup = true;
                orGroupResult = orGroupResult || condition; // the total of the orGroup so far

                if (debug)
                {
                    Debug.Log($"OR Condition: {condition} evaluated to {condition}. Current OR Group Result: {orGroupResult}");
                }

                // If it's the last condition in our list, finalize the OR group evaluation
                if (i == Conditions.Count - 1)
                {
                    totalLogic = totalLogic && orGroupResult;
                }
            }
            else
            {
                // If this is an AND condition mixed preceding OR conditions
                if (evaluatingOrGroup)
                {
                    // Finalize the OR group evaluation
                    totalLogic = totalLogic && orGroupResult;
                    evaluatingOrGroup = false;
                    orGroupResult = false;

                    // Short circuit if the OR group result is false
                    if (!totalLogic)
                    {
                        break;
                    }
                }

                // Evaluate the current condition
                if (!condition)
                {
                    totalLogic = false;
                    if (debug)
                    {
                        Debug.Log($"{condition} is False. Breaking out of the loop.");
                    }
                    break;
                }

                if (debug)
                {
                    Debug.Log($"{condition} is True.");
                }
            }
        }

        // Final check for any remaining OR group at the end
        if (evaluatingOrGroup)
        {
            totalLogic = totalLogic && orGroupResult;
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
        foreach (Condition condition in Conditions)
        {
            condition.Reconstruct();
            // Worth defining a custom OnValidate for Condition
        }
    }
}
