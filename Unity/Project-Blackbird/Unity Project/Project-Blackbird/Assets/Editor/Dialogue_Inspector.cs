using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DialogueTrigger))]
public class Dialogue_Inspector : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
  
        DialogueTrigger dt = (DialogueTrigger)target;
        Dialogue dialogue = dt.dialogue;

        dialogue.name = EditorGUILayout.TextField("NPC Name", dialogue.name);
        dialogue.options = EditorGUILayout.Toggle("Ask Player?", dialogue.options);
        if (dialogue.options == true) {
            
        }
        dialogue._continue = EditorGUILayout.Toggle("Continue next dialogue?", dialogue._continue);
        if (dialogue._continue == true) {
            EditorGUILayout.LabelField(" meow");
        }   
    }
}
