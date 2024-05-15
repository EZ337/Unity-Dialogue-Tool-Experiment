using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class DialogueGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueGraphView, GraphView.UxmlTraits> { }

    /// <summary>
    /// The current Dialogue Root being edited
    /// </summary>
    private DialogueRoot currentDlgRoot;

    public DialogueGraphView() 
    {
        // Grid Background
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // Stylesheet
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueEditorWindow.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(DialogueRoot dlgRoot)
    {
        currentDlgRoot = dlgRoot;

        DeleteElements(graphElements);

        foreach (var startingTopic in currentDlgRoot.StartingTopics)
        {
            CreateNodeView(startingTopic);
            foreach( var dlg in startingTopic.NextDialogueOptions)
            {
                CreateNodeView(dlg); // Needs revision later
            }
        }
        //currentDlgRoot.StartingTopics.ForEach(dlg => CreateNodeView(dlg));


    }

    private void CreateNodeView(Dialogue dlg)
    {
        NodeView nodeView = new NodeView(dlg);
        AddElement(nodeView);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction("Create New Starting Dialogue", _ => CreateNewDialogue(true)); // Create new Starting dialogue
        evt.menu.AppendAction("Create New Dialogue", _ => CreateNewDialogue(false)); // Create regular dialogue
    }

    private void CreateNewDialogue(bool isStartingTopic)
    {
        //Debug.Log("Pressed Create New starting topic");
        Dialogue dlg = currentDlgRoot.CreateTopic(isStartingTopic);
        CreateNodeView(dlg);
    }
}
