using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DLNodeEditor : EditorWindow {

    private List<DLNode> windows = new List<DLNode>();

    private Vector2 mousePos;

    private DLNode selectedNode;

    private bool makeTransitionMode = false;

    [MenuItem("Window/Node Editor")]
    static void ShowEditor() {
        DLNodeEditor editor = EditorWindow.GetWindow<DLNodeEditor>();
    }


    public int CheckIfSelected() {
        for (int i = 0; i < windows.Count; i++) {
            if (windows[i].rect.Contains(mousePos)) {
                return i;
            }
        }
        return -1;
    }
    private void OnGUI() {
        Event e = Event.current;

        mousePos = e.mousePosition;
        //Right mouse click when not in transition
        if (e.button == 1 && !makeTransitionMode) {
            if (e.type == EventType.MouseDown){
                bool clickedOnWindow = false;
                int selectedIndex = -1;

                if(CheckIfSelected() != -1) {
                    clickedOnWindow = true;
                    selectedIndex = CheckIfSelected();
                }

                if (!clickedOnWindow) {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallback, "DLNode");
                    //menu.AddItem(new GUIContent("Add Event Node"), false, ContextCallback, "EventNode");

                    menu.ShowAsContext();
                    e.Use();
                }
                else {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        if (makeTransitionMode && selectedNode != null) {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
            DrawNodeCurve(selectedNode.rect, mouseRect);
            Repaint();
        }
        foreach(DLNode n in windows) {
            n.DrawCurves();
        }
        BeginWindows();

        for(int i = 0; i <  windows.Count; i++) {
            windows[i].rect = GUI.Window(i, windows[i].rect, DrawNodeWindow, windows[i].windowTitle);
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
            node.rect = new Rect(mousePos.x, mousePos.y, 500, 150);

            windows.Add(node);
        }
        /*else if (clb.Equals("EventNode")) {
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
        */
        else if(clb.Equals("deleteNode")){
            bool clickedOnWindow = false;
            int selectedIndex = -1;

            if (CheckIfSelected() != -1) {
                clickedOnWindow = true;
                selectedIndex = CheckIfSelected();
            }

            if (clickedOnWindow) {
                DLNode node = windows[selectedIndex];
                windows.RemoveAt(selectedIndex);

                foreach(DLNode n in windows) {
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
    public void OnClickOutPoint(ConnectionPoint outPoint) {
        Debug.Log("out");
    }
    public void OnClickInPoint(ConnectionPoint inPoint) {

    }
}
