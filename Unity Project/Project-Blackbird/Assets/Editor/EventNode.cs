using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
public class EventNode : BaseDLInputNode {
    public UnityEvent onEnd = new UnityEvent();
    public Rect parentRect;

    public override void DrawWindow() {
        base.DrawWindow();
        windowTitle = "Event Node";
        // Serializing Objects/Properties
        EventNode eventNode = this;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(eventNode);
        SerializedProperty endEvent = serializedObject.FindProperty("onEnd");
        // End

        EditorGUILayout.PropertyField(endEvent);
    }
    public override void DrawCurves() {
        if (parentRect != null) {
            Rect rect = windowRect;
            rect.x += parentRect.x;
            rect.y += parentRect.y + parentRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            DLNodeEditor.DrawNodeCurve(parentRect, rect);
        }
    }
    public override void NodeDeleted(BaseDLNode node) {
        if (node.Equals(parentRect)) {
        }
    }
}
