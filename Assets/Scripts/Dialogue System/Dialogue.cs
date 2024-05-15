using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 EditorPosition;


    private bool isStartingTopic = false;

    [SerializeField, Tooltip("The text the player will say")]
    private string playerPrompt;

    [SerializeField, Tooltip("Sequence of responses to this prompt")]
    private DialogueResponse[] responses;

    // Flags

    [SerializeField, Tooltip("The next dialogue options the player will have to select from")]
    private Dialogue[] nextDialogueOptions;

    // Conditions

    [SerializeField, Tooltip("Event Called when this topic is first interacted with")]
    private UnityEvent dialogueBeginEvent;
    [SerializeField, Tooltip("Event called when we finish this dialogue")]
    private UnityEvent dialogueEndEvent;

    /// <summary>
    /// The dialogue piece the player will say
    /// </summary>
    public string PlayerPrompt { get => playerPrompt; set => playerPrompt = value; }

    /// <summary>
    /// List of responses the NPC will say when player picks this option. Says all of them sequentially according to conditions
    /// </summary>
    public DialogueResponse[] Responses { get => responses; set => responses = value; }

    /// <summary>
    /// The next dialogue options the player will have to select from
    /// </summary>
    public Dialogue[] NextDialogueOptions { get => nextDialogueOptions; set => nextDialogueOptions = value; }
    public bool IsStartingTopic { get => isStartingTopic; set => isStartingTopic = value; }
}
