using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DialogueTrigger))]
public class Dialogue_Inspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        /*
        DialogueTrigger dt = (DialogueTrigger)target;
        Dialogue dialogue = dt.dialogue;

        SerializedProperty dl = serializedObject.FindProperty("dialogue");
        dl = dl.serializedObject.FindProperty("sentences");
        dialogue.name = EditorGUILayout.TextField("NPC Name", dialogue.name);
        dialogue.options = EditorGUILayout.Toggle("Ask Player?", dialogue.options);
        if (dialogue.options == true) {
            EditorGUILayout.LabelField("Now show answer textfields and answer Dialogue Classes");
            EditorGUILayout.PropertyField(dl, true);
        }
        dialogue._continue = EditorGUILayout.Toggle("Continue next dialogue?", dialogue._continue);
        if (dialogue._continue == true) {
            EditorGUILayout.LabelField("Now show next Dialogue Class");
        }   
        */
    }
}
