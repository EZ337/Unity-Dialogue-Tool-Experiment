using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    private List<DialogueRoot> charDialogues = new();
    private ListView dlgRootsView;
    public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits> { }
    public InspectorView() 
    {
        dlgRootsView = this.Q<ListView>("dlg-roots-view");
    }

    public void PopulateDialogueInspector(List<DialogueRoot> dialogueRoots)
    {

        if (dlgRootsView == null)
        {
            Debug.LogWarning("Failed to get the dlgRootsView. Fixing...");
            dlgRootsView = this.Q<ListView>("dlg-roots-view");
            if (dlgRootsView == null)
            {
                Debug.LogError("Failed to acquire The list view in the Dialogue Editor in the Inspector");
                return;
            }
        }


    }
}
