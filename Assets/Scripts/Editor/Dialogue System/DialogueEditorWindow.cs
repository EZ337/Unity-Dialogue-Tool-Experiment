using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System.Collections.Generic;

public class DialogueEditorWindow : EditorWindow
{
    DialogueGraphView dlgGraphView;
    InspectorView dlgInspectorView;
    Label dlgRootLabel;

    [SerializeField]
    private VisualTreeAsset visualTree;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow()
    {
        DialogueEditorWindow wnd = GetWindow<DialogueEditorWindow>();
        wnd.titleContent = new GUIContent("DialogueEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        if (visualTree == null)
        {
            visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Dialogue System/DialogueEditorWindow.uxml");
        }
        visualTree.CloneTree(root);

        // Get references to the views in the window
        dlgGraphView = root.Q<DialogueGraphView>();
        dlgInspectorView = root.Q<InspectorView>();
        dlgRootLabel = root.Q<Label>("root-owner");

        dlgGraphView.NodeSelectAction = OnNodeSelectionChange;

        OnSelectionChange();
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instannceId, int line)
    {
        if (Selection.activeObject is Dialogue || Selection.activeObject is DialogueRoot)
        {
            ShowWindow();
            return true;
        }

        return false;
    }

    #region Needs Revision to be when we select a dlg Root
    private void OnSelectionChange()
    {
        // If we select a DialogueRoot Asset (In the project folder)
        DialogueRoot dlgRoot = Selection.activeObject as DialogueRoot;
        if (dlgRoot != null)
        {
            dlgRootLabel.text = dlgRoot.name + " > " + dlgRoot.TopicName;

            dlgGraphView.SetEnabled(true);
            dlgGraphView.PopulateView(dlgRoot);
            return;
        }
        
        // If we Select A dialogue Asset (A child of a DialogueRoot In the project folder)
        Dialogue dlg = Selection.activeObject as Dialogue;
        if (dlg != null && dlg.Root != null) 
        {
            dlgRootLabel.text = dlg.Root.name + " > " + dlg.Root.TopicName;
            dlgGraphView.SetEnabled(true);
            dlgGraphView.PopulateView(dlg.Root);
            return;
        }
        
        // If we select a gameObject that has the CharacterDialogue component on it (The most ideal way)
        if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent(out CharacterDialogue charDlg))
        {
            // Populate Inspector

            #region Revision. Modify the AssetDestroy so that it automatically handles this
            // Select the first DlgRoot from List
            if (charDlg.DialogueTopics.Count > 0)
            {

                // Make sure DialogueRoots are always valid in case someone deleted from the wrong place.
                if (charDlg.DialogueTopics[0] == null)
                {
                    Debug.LogWarning("For some reason, DlgRoot[0] is null");

                    List<DialogueRoot> newRoots = new List<DialogueRoot>();
                    charDlg.DialogueTopics.ForEach((dlg) =>
                    {
                        if (dlg != null)
                        {
                            newRoots.Add(dlg);
                        }
                    });

                    charDlg.DialogueTopics = newRoots;
                }

                // Now display the new 0th root if available
                if (charDlg.DialogueTopics.Count > 0)
                {
                    dlgRoot = charDlg.DialogueTopics[0];

                    dlgRootLabel.text = dlgRoot.name + " > " + dlgRoot.TopicName;
                    dlgGraphView.SetEnabled(true);
                    dlgGraphView.PopulateView(dlgRoot);
                    // Populate Second Inpsector View for the characterDlg
                    //dlgInspectorView.PopulateDialogueInspector(charDlg.DialogueTopics);
                    return;
                }

            }
            #endregion
        }

        dlgGraphView.ClearView();
        dlgGraphView.SetEnabled(false);
    }
    #endregion

    private void OnNodeSelectionChange(NodeView nodeView)
    {
        dlgInspectorView.UpdateSelection(nodeView);
    }
}
