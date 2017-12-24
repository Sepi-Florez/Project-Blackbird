using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class DLNode : ScriptableObject{

    //Base Node Variables
    public Rect rect;
    public bool hasInputs = false;
    public string windowTitle = " ";

    public enum NodeType { Sentences, Answers, Event}
    public NodeType nodeType = new NodeType();

    //General
    public string npcName;

    //Sentences
    public string[] sentences;

    //Answers
    public string[] answers;
    public Dialogue[] answerDialogues;

    //Event
    public UnityEvent onEnd = new UnityEvent();

    public List<ConnectionPoint> points = new List<ConnectionPoint>();

    public void DrawWindow() {
        //Serializing Objects/Properties
        DLNode thisNode = this;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(thisNode);
        SerializedProperty st = serializedObject.FindProperty("sentences");
        SerializedProperty ans = serializedObject.FindProperty("answers");
        SerializedProperty eve = serializedObject.FindProperty("onEnd");
        // End Serializing Objects/Properties


        nodeType = (NodeType)EditorGUILayout.EnumPopup("Node Type", nodeType);

        switch (nodeType) {
            case (NodeType)0:
                npcName = EditorGUILayout.TextField("NPC name", npcName);
                windowTitle = "Sentences : " + npcName ;
                serializedObject.Update();
                EditorGUILayout.PropertyField(st, true);
                serializedObject.ApplyModifiedProperties();
                break;
            case (NodeType)1:
                windowTitle = "Answers";
                serializedObject.Update();
                EditorGUILayout.PropertyField(ans, true);
                serializedObject.ApplyModifiedProperties();
                break;
            case (NodeType)2:
                windowTitle = "Event";
                EditorGUILayout.PropertyField(eve, true);
                break;
        }
        
    }
    public void DrawCurves() {

    }

    public virtual void NodeDeleted(DLNode node) {

    }
}
