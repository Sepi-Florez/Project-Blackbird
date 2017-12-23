using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class DLNode : BaseDLInputNode {
    public string name;
    public string[] sentences;
    public bool _continue;
    public Dialogue nextDialogue;



    public override void DrawWindow() {
        base.DrawWindow();
        base.windowTitle = "Dialogue";
        //Serializing Objects/Properties
        DLNode thisNode = this;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(thisNode);
        SerializedProperty dl = serializedObject.FindProperty("sentences");
        SerializedProperty nd = serializedObject.FindProperty("nextDialogue");
        // End Serializing Objects/Properties
        name = EditorGUILayout.TextField("NPC name", name);
        EditorGUILayout.PropertyField(dl, true);
        _continue = EditorGUILayout.Toggle("Continue", _continue);
        if (_continue) {
            EditorGUILayout.PropertyField(nd);
        }
    }
    public override void DrawCurves() {
        Debug.Log("Hary");
    }
}
