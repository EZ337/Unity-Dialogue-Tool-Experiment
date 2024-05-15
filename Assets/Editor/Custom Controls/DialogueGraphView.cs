using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

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

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // Get all the Dialgues under our root scriptableobject
        var dlgs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(currentDlgRoot))
            .Where(asset => asset.GetType() == typeof(Dialogue));

        #region Revision
        // TODO: Needs Revision as I am just putting to screen things that are only linked
        foreach (Dialogue topic in dlgs)
        {
            CreateNodeView(topic);
        }
        #endregion
        //currentDlgRoot.StartingTopics.ForEach(dlg => CreateNodeView(dlg));


    }

    /// <summary>
    /// Event received when the graph view is interacted with
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        graphViewChange.elementsToRemove?.ForEach(elem =>
            {
                NodeView nodeview = elem as NodeView;
                if (nodeview != null)
                {
                    currentDlgRoot.DeleteTopic(nodeview.dlg);
                }
            });

        return graphViewChange;
    }

    /// <summary>
    /// Creates a node. Returns true if success. False otherwise
    /// </summary>
    /// <param name="dlg">Dialogue that the nodeView will hold</param>
    /// <returns>True if succesful</returns>
    private bool CreateNodeView(Dialogue dlg)
    {
        if (dlg != null)
        {
            NodeView nodeView = new NodeView(dlg);
            AddElement(nodeView);
            return true;
        }
        return false;

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
