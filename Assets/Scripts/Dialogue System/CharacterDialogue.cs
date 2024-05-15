using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The full dialogue view of this character. This is the dialogue holder of
/// all the dialogue this character will have
/// </summary>
public class CharacterDialogue : MonoBehaviour
{
    // May want to get a particular interface on the character

    //[SerializeField, Tooltip("List of all the top level dialogues this character has. Sorted by priority")]
    //private SortedList<DialogueRoot, string> dialogueTopics = new();


    //public SortedList<DialogueRoot, string> DialogueTopics { get => dialogueTopics; set => dialogueTopics = value; }

    [Tooltip("Needs to change to a SortedList")]
    public List<DialogueRoot> DialogueTopics = new List<DialogueRoot>();
}
