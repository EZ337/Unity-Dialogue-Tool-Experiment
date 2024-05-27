using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{

    [Condition(typeof(int))]
    [field: SerializeField]
    public int Level {  get; private set; }

    [Condition(typeof(bool))]
    [field: SerializeField]
    public bool IsDead { get; private set; }


    public UnityEvent evt;

    [Condition]
    public void TestMethod()
    {
        Debug.Log(name + " Test Method {Actor} called");
    }

    [Condition(typeof(Dialogue))]
    public bool CheckDialogue(Dialogue dialogue)
    {
        Debug.Log("Check Dialogue called. Dialogue guid: " + dialogue.guid);
        return true;
    }

    [Condition(typeof(Collider))]
    public bool ActorHasCollider(Collider collider)
    {
        return TryGetComponent<Collider>(out Collider sth);
    }
}
