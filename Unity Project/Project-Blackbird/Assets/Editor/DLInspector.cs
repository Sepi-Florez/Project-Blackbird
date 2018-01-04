using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Dialogue))]
public class DLInspector : Editor {
    public override void OnInspectorGUI() {
        Dialogue dl = (Dialogue)target;
        SerializedProperty type = serializedObject.FindProperty("type");
        SerializedProperty sentences = serializedObject.FindProperty("sentences");
        SerializedProperty answers = serializedObject.FindProperty("answers");
        SerializedProperty answersDls = serializedObject.FindProperty("answerDialogues");
        SerializedProperty leEvent = serializedObject.FindProperty("onEnd");
        SerializedProperty nextDialogue = serializedObject.FindProperty("nextDialogue");

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("NPC Name");
        dl.name = EditorGUILayout.TextField(dl.name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(type, true);
        switch (dl.type) {
            case (Dialogue.TypeDL)0:
                EditorGUILayout.PropertyField(sentences, true);
                EditorGUILayout.PropertyField(nextDialogue, true);
                break;
            case (Dialogue.TypeDL)1:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Question");
                dl.question = EditorGUILayout.TextField(dl.question);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(answers, true);
                EditorGUILayout.PropertyField(answersDls, true);
                break;
            case (Dialogue.TypeDL)2:
                EditorGUILayout.PropertyField(leEvent, true);
                EditorGUILayout.PropertyField(nextDialogue, true);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
