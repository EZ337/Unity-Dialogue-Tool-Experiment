using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Condition(typeof(int))]
    [field: SerializeField]
    public int Level {  get; private set; }

    [Condition(typeof(bool))]
    [field: SerializeField]
    public bool IsDead { get; private set; }

    [Condition(typeof(Actor))]
    [field: SerializeField]
    public Actor Me { get; private set; }

}
