using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    [Condition(typeof(Actor), typeof(int))]
    [field: SerializeField]
    public int Level {  get; private set; }

    [Condition(typeof(Actor), typeof(bool))]
    [field: SerializeField]
    public bool IsDead { get; private set; }


    public UnityEvent evt;

    [Condition(typeof(Actor))]
    public void TestMethod()
    {
        Debug.Log(name + " Test Method {Actor} called");
    }

}
