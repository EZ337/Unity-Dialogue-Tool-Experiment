using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : ScriptableObject
{
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 EditorPosition;

    [field : SerializeField]
    public DialogueRoot Root { get; set; }

    [field : SerializeField] 
    public bool IsStartingTopic { get; set; } = false;

    /// <summary>
    /// The dialogue piece the player will say
    /// </summary>
    [field: SerializeField, Tooltip("The text the player will say")]
    public string PlayerPrompt { get; private set; }

    /// <summary>
    /// List of responses the NPC will say when player picks this option. Says all of them sequentially according to conditions
    /// </summary>
    [field: SerializeField, Tooltip("Sequence of responses to this prompt")]
    public DialogueResponse[] Responses { get; private set; }

    // Flags

    /// <summary>
    /// The next dialogue options the player will have to select from
    /// </summary>

    [field : SerializeField, Tooltip("The next dialogue options the player will have to select from")]
    public List<Dialogue> NextDialogueOptions { get; private set; } = new List<Dialogue>();

    // Conditions

    [SerializeField, Tooltip("Event Called when this topic is first interacted with")]
    private UnityEvent dialogueBeginEvent;

    [SerializeField, Tooltip("Event called when we finish this dialogue")]
    private UnityEvent dialogueEndEvent;

}
