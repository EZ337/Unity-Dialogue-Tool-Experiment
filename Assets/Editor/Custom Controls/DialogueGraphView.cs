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
    public Action<NodeView> NodeSelectAction;

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

        ClearView();

        // Get all the Dialgues under our root scriptableobject
        var dlgs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(currentDlgRoot))
            .Where(asset => asset.GetType() == typeof(Dialogue));

        #region Revision
        // TODO: Needs Revision as I am just putting to screen things that are only linked
        foreach (Dialogue topic in dlgs)
        {
            CreateNodeView(topic);
        }

        foreach (Dialogue topic in dlgs)
        {
            if (topic.NextDialogueOptions.Count > 0)
            {
                NodeView currTopicNode = GetNodeByGuid(topic.guid) as NodeView;

                foreach (Dialogue connection in topic.NextDialogueOptions)
                {
                    NodeView connectionNode = GetNodeByGuid(connection.guid) as NodeView;

                    Edge connectedEdge = currTopicNode.oPort.ConnectTo(connectionNode.iPort);
                    AddElement(connectedEdge);
                }
            }

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
            if (elem is NodeView nodeview)
            {
                // Delete a topic
                currentDlgRoot.DeleteTopic(nodeview.dlg);
            }

            if (elem is Edge edge)
            {
                NodeView start = edge.output.node as NodeView;
                NodeView end = edge.input.node as NodeView;

                currentDlgRoot.RemoveConnection(start.dlg, end.dlg);
            }
        });

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView iNode = edge.input.node as NodeView;
                NodeView oNode = edge.output.node as NodeView;

                currentDlgRoot.ConnectDialogue(((NodeView)edge.output.node).dlg, ((NodeView)edge.input.node).dlg);
            });
        }

        EditorUtility.SetDirty(currentDlgRoot);

        return graphViewChange;
    }

    /// <summary>
    /// Subtly clears the view. Does not remove/destroy anything
    /// </summary>
    public void ClearView()
    {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
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
            nodeView.NodeSelectAction = NodeSelectAction;
            AddElement(nodeView);
            return true;
        }
        return false;

    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {

        evt.menu.AppendAction("Create New Starting Dialogue", (evt) => CreateNewDialogue(true)); // Create new Starting dialogue
        evt.menu.AppendAction("Create New Dialogue", (evt) => CreateNewDialogue(false)); // Create regular dialogue
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
            (endPort.direction != startPort.direction) && (endPort.node != startPort.node)
        ).ToList();
    }

    private void CreateNewDialogue(bool isStartingTopic)
    {
        //Debug.Log("Pressed Create New starting topic");
        Dialogue dlg = currentDlgRoot.CreateTopic(currentDlgRoot, isStartingTopic);
        CreateNodeView(dlg);
    }

    
}
