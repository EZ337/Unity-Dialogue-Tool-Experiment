using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CustomEditor(typeof(CharacterDialogue))]
public class CharDialogueInspector : Editor
{

    public VisualTreeAsset VisualTree;
    private TextField dlgRootName;
    private Button createDlgRootBtn;
    private Button openDlgEditBtn;
    private EnumField dlgType;
    private IntegerField priority;
    private ListView dlgRoots;
    private CharacterDialogue owningCharacter;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        // Add a 
        VisualTree.CloneTree(root);

        dlgRootName = root.Q<TextField>("topic-name");
        dlgType = root.Q<EnumField>("dlg-type-enum");
        priority = root.Q<IntegerField>("dlg-priority");
        createDlgRootBtn = root.Q<Button>("create-dlg-btn");
        openDlgEditBtn = root.Q<Button>("open-dlg-btn");
        dlgRoots = root.Q<ListView>("dlg-roots");

        dlgRoots.SetEnabled(false);

        // Creates a new dialogue root and opens the editor window
        createDlgRootBtn.RegisterCallback<ClickEvent>(CreateNewDialogueRoot);

        // Open the Dialogue Editor window when we press the btn
        openDlgEditBtn.RegisterCallback<ClickEvent>(_ => DialogueEditorWindow.ShowWindow());

        owningCharacter = this.target.GetComponent<CharacterDialogue>();

        return root;
    }

    private void CreateNewDialogueRoot(ClickEvent evt)
    {
        if (dlgRootName.value == "")
        {
            Debug.LogWarning("Cannot create a Dialogue Root without a name");
            return;
        }

        string targetPath = $"Assets/Dialogues/{this.target.name}/";

        // Create directory if it doesn't exist
        if (!AssetDatabase.IsValidFolder(targetPath))
        {
            AssetDatabase.CreateFolder("Assets/Dialogues", this.target.name);
        }

        if (AssetDatabase.LoadAssetAtPath<DialogueRoot>($"{targetPath}{dlgRootName.value}.asset") != null)
        {
            Debug.LogWarning($"There's already a DialogueRoot named \"{dlgRootName.value}\" in {targetPath}");
            return;
        }

        DialogueRoot dialogueRoot = ScriptableObject.CreateInstance<DialogueRoot>();
        dialogueRoot.Iniitialise(dlgRootName.value, (DialogueType)dlgType.value, priority.value);

        AssetDatabase.CreateAsset(dialogueRoot, $"Assets/Dialogues/{this.target.name}/{dialogueRoot.TopicName}.asset");
        AssetDatabase.SaveAssets();
        dlgRootName.value = ""; // Clear the string

        // Update the list in CharacterDialogue
        owningCharacter.DialogueTopics.Add(dialogueRoot);

        Debug.Log($"Added {dialogueRoot.TopicName} To {AssetDatabase.GetAssetPath(dialogueRoot)}");
    }
}
