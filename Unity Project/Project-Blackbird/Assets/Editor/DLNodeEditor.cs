using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DLNodeEditor : EditorWindow {

    private List<BaseDLNode> windows = new List<BaseDLNode>();

    private Vector2 mousePos;

    private BaseDLNode selectedNode;

    private bool makeTransitionMode = false;

    [MenuItem("Window/Node Editor")]
    static void ShowEditor() {
        DLNodeEditor editor = EditorWindow.GetWindow<DLNodeEditor>();
    }

    private void OnGUI() {
        Event e = Event.current;

        mousePos = e.mousePosition;

        if (e.button == 1 && !makeTransitionMode) {
            if (e.type == EventType.MouseDown){
                bool clickedOnWindow = false;
                int selectedIndex = -1;

                for (int i = 0; i < windows.Count; i++) {
                    if (windows[i].windowRect.Contains(mousePos)) {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }
                if (!clickedOnWindow) {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallback, "DLNode");
                    menu.AddItem(new GUIContent("Add Event Node"), false, ContextCallback, "EventNode");

                    menu.ShowAsContext();
                    e.Use();
                }
                else {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Make transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator(" ");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        else if(e.button == 0 && e.type == EventType.mouseDown && makeTransitionMode) {
            bool clickedOnWindow = false;
            int selectedIndex = -1;

            for (int i = 0; i < windows.Count; i++) {
                if (windows[i].windowRect.Contains(mousePos)) {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if(clickedOnWindow && !windows[selectedIndex].Equals(selectedNode)){
                windows[selectedIndex].SetInput((BaseDLInputNode)selectedNode, mousePos);
                makeTransitionMode = false;
                selectedNode = null;
            }
            if (!clickedOnWindow) {
                makeTransitionMode = false;
                selectedNode = null;
            }
            e.Use();
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode) {
            bool clickedOnWindow = false;
            int selectedIndex = -1;

            for (int i = 0; i < windows.Count; i++) {
                if (windows[i].windowRect.Contains(mousePos)) {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if (clickedOnWindow) {
                BaseDLInputNode nodeTochange = windows[selectedIndex].ClickedOnInput(mousePos);
                if(nodeTochange != null) {
                    selectedNode = nodeTochange;
                    makeTransitionMode = true;
                }
            }
        }
        if (makeTransitionMode && selectedNode != null) {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
            DrawNodeCurve(selectedNode.windowRect, mouseRect);
            Repaint();
        }
        foreach(BaseDLNode n in windows) {
            n.DrawCurves();
        }
        BeginWindows();

        for(int i = 0; i <  windows.Count; i++) {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }
        EndWindows();
    }

    void DrawNodeWindow(int id) {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }
    void ContextCallback(object obj) {
        string clb = obj.ToString();

        if (clb.Equals("DLNode")) { 
            DLNode node = new DLNode();
            node.windowRect = new Rect(mousePos.x, mousePos.y, 500, 150);

            windows.Add(node);
        }
        else if (clb.Equals("EventNode")) {
            EventNode node = new EventNode();
            node.windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);

            windows.Add(node);
        }
        else if(clb.Equals("makeTransition")){
            bool clickedOnWindow = false;
            int selectedIndex = -1;
            for (int i = 0; i < windows.Count; i++) {
                if (windows[i].windowRect.Contains(mousePos)) {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if (clickedOnWindow) {
                selectedNode = windows[selectedIndex];
                makeTransitionMode = true;
            }
        }
        else if(clb.Equals("deleteNode")){
            bool clickedOnWindow = false;
            int selectedIndex = -1;
            for (int i = 0; i < windows.Count; i++) {
                if (windows[i].windowRect.Contains(mousePos)) {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if (clickedOnWindow) {
                BaseDLNode node = windows[selectedIndex];
                windows.RemoveAt(selectedIndex);

                foreach(BaseDLNode n in windows) {
                    n.NodeDeleted(node);
                }
            }
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowC = new Color(0, 0, 0, .06f);

        for(int i = 0; i < 3; i++) {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowC, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
