using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CreateAssetMenu(fileName ="New Dialogue Branch", menuName = "Dialogue System/Dialogue Root")]
public class DialogueRoot : ScriptableObject, IComparable<DialogueRoot>
{
    [SerializeField, Tooltip("The name of this dialogue tree")]
    private string topicName;

    [SerializeField, Tooltip("The first dialogue option that constitutes this dialogue tree")]
    private List<Dialogue >startingTopics = new();

    [SerializeField, Tooltip("The type of dialogue we are leading to")]
    private DialogueType dlgType;

    [SerializeField, Tooltip("Sorting order for the dialogues")]
    private int priority;


    #region Properties

    /// <summary>
    /// The starting topic for this main branch
    /// </summary>
    public List<Dialogue> StartingTopics { get => startingTopics; set => startingTopics = value; }

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

    #endregion

    public void Initialise(string  topicName, DialogueType dlgType, int priority)
    {
        this.topicName = topicName;
        this.dlgType = dlgType;
        this.priority = priority;
    }

    #region GraphView Stuff
    int index = 0;
    public Dialogue CreateTopic(DialogueRoot root, bool isStartingTopic = false)
    {
        Dialogue dlg = ScriptableObject.CreateInstance<Dialogue>();
        dlg.name = $"New Dialogue Entry {index++}";
        dlg.Root = root ;
        dlg.guid = GUID.Generate().ToString();

        if (isStartingTopic )
        {
            startingTopics.Add(dlg);
            dlg.IsStartingTopic = true;
        }

        AssetDatabase.AddObjectToAsset(dlg, this);
        AssetDatabase.SaveAssets();
        return dlg;
    }

    public void DeleteTopic(Dialogue dlg)
    {
        // Delete from starting topic list if it's a starting topic
        if (dlg.IsStartingTopic)
        {
            if (!StartingTopics.Remove(dlg))
            {
                Debug.LogWarning($"Failed to remove Dialogue: {dlg.name} from starting topics");
            }
        }

        AssetDatabase.RemoveObjectFromAsset(dlg);
        AssetDatabase.SaveAssets();
    }

    public void ConnectDialogue(Dialogue parent, Dialogue next)
    {
        parent.NextDialogueOptions.Add(next);
    }

    public void RemoveConnection(Dialogue parent, Dialogue next)
    {
        if (!parent.NextDialogueOptions.Remove(next))
        {
            Debug.LogWarning("Failed to remove NextDialogueConnection");
        }
    }

    #endregion



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