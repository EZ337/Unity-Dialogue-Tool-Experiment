using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue Branch", menuName = "Dialogue System/Dialogue Root")]
public class DialogueRoot : ScriptableObject, IComparable<DialogueRoot>
{
    [SerializeField, Tooltip("The name of this dialogue tree")]
    private string topicName;

    [SerializeField, Tooltip("The first dialogue option that constitutes this dialogue tree")]
    private Dialogue[] startingTopics;

    [SerializeField, Tooltip("The type of dialogue we are leading to")]
    private DialogueType dlgType;

    [SerializeField, Tooltip("Sorting order for the dialogues")]
    private int priority;

    /// <summary>
    /// The starting topic for this main branch
    /// </summary>
    public Dialogue[] StartingTopics { get => startingTopics; set => startingTopics = value; }

    /// <summary>
    /// The type of dialogue. Preempt will block all other dialogues until its condition is false.
    /// </summary>
    public DialogueType DlgType { get => dlgType; set => dlgType = value; }

    /// <summary>
    /// Sorting order for dialogues
    /// </summary>
    public int Priority { get => priority; set => priority = value; }

    /// <summary>
    /// The "id" of this 
    /// </summary>
    public string TopicName { get => topicName; set => topicName = value; }


    /// <summary>
    /// Interface implementation to sort object
    /// </summary>
    /// <param name="other">The other DialogueRoot</param>
    /// <returns><0 if this preceeds other. 0 if equal in ordering, and >0 if after other</returns>
    public int CompareTo(DialogueRoot other)
    {
        return this.priority.CompareTo(other.priority);
    }
}

/// <summary>
/// Dialogue types marking the whole tree. Preempt will block all other dialogues until its conditions are false.
/// Greetings are randomly picked by default to greet the player and then present them with options
/// Normal lines are just dialogue choices that lead to conversation with the player
/// </summary>
public enum DialogueType
{
    Normal, // A regular dialogue script
    Greeting, // A greeting dialogue
    Preempt // A dialogue script that trumps normal dialogues as long as its condition is true
}