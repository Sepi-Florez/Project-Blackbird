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
    [TextArea(3,10)]
    public string[] sentences;

    //Answers
    [TextArea(3, 10)]
    public string[] answers;
    public Dialogue[] answerDialogues;

    public ConnectionPoint[] points;

    //Event
    public UnityEvent onEnd = new UnityEvent();

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    //Constructor
    public DLNode(Rect newRect, GUIStyle inPointStyle, GUIStyle outPointStyle, System.Action<ConnectionPoint> OnClickInPoint, System.Action<ConnectionPoint> OnClickOutPoint) {
        rect = newRect;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        //GUI.Box(rect, windowTitle);
    }
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
                if(sentences != null)
                    rect.height = 100 + (sentences.Length * 60);
                npcName = EditorGUILayout.TextField("NPC name", npcName);
                windowTitle = "Sentences : " + npcName ;
                serializedObject.Update();
                EditorGUILayout.PropertyField(st, true);
                serializedObject.ApplyModifiedProperties();
                break;
            case (NodeType)1:
                if(answers != null)
                    rect.height = 100 + (answers.Length * 60);
                windowTitle = "Answers";
                serializedObject.Update();
                EditorGUILayout.PropertyField(ans, true);
                serializedObject.ApplyModifiedProperties();

                break;
            case (NodeType)2:
                rect.height = 220;
                windowTitle = "Event";
                EditorGUILayout.PropertyField(eve, true);
                break;
        }
    }
    public void DrawPoints() {
        if (inPoint != null && outPoint != null) {
            switch (nodeType) {
                case (NodeType)0:
                    inPoint.Draw();
                    outPoint.Draw();
                    break;
                case (NodeType)1:
                    inPoint.Draw();
                    if (answers != null) {
                        foreach (string answer in answers) {
                            points = new ConnectionPoint[answers.Length];
                            for (int i = 0; i < points.Length; i++) {
                                points[i] = outPoint;
                            }
                        }
                        DrawMultiplePoints(points);
                    }
                    break;
                case (NodeType)2:
                    inPoint.Draw();
                    outPoint.Draw();
                    break;
            }
        }
    }
    public void DrawMultiplePoints(ConnectionPoint[] points) {
        if (inPoint != null) {
            float i = rect.y + 10;
            foreach (ConnectionPoint p in points) {
                p.Draw2((int)i);
                i += rect.height / points.Length;
            }
        }
    }

    public void DrawCurves() {

    }

    public virtual void NodeDeleted(DLNode node) {

    }
}
