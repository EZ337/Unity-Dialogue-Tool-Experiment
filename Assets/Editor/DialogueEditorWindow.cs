using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class DialogueEditorWindow : EditorWindow
{
    DialogueGraphView dlgGraphView;
    InspectorView dlgInspectorView;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

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
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DialogueEditorWindow.uxml");
        visualTree.CloneTree(root);

        // Get references to the views in the window
        dlgGraphView = root.Q<DialogueGraphView>();
        dlgInspectorView = root.Q<InspectorView>();

        OnSelectionChange();
    }

    //[OnOpenAsset]
    //public static bool OnOpenAsset(int instannceId, int line)
    //{
    //    if ()
    //    {
    //        ShowWindow();
    //        return true;
    //    }

    //    return false;
    //}

    #region Needs Revision to be when we select a dlg Root
    private void OnSelectionChange()
    {
        DialogueRoot dlgRoot = Selection.activeObject as DialogueRoot;
        if (dlgRoot != null)
        {
            Label title = dlgGraphView.Q<Label>("root-owner");
            title.text = dlgRoot.name + " > " + dlgRoot.TopicName;

            dlgGraphView.SetEnabled(true);
            dlgGraphView.PopulateView(dlgRoot);
        }
        //else
        //    dlgGraphView.SetEnabled(false);
    }
    #endregion
}
