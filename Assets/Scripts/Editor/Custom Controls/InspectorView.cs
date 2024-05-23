
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits> { }
    private Editor editor;
    public InspectorView() 
    {
    }

    /// <summary>
    /// Function called when we select a new node in the graphView
    /// </summary>
    /// <param name="nodeView">The node we selected</param>
    public void UpdateSelection(NodeView nodeView)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.dlg);
        IMGUIContainer iMGUIContainer = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(iMGUIContainer);

        EditorGUIUtility.PingObject(nodeView.dlg);
    }
}
