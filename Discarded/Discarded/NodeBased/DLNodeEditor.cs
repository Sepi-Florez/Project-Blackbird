using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DLNodeEditor : EditorWindow {

    private List<DLNode> windows = new List<DLNode>();

    private Vector2 mousePos;

    private DLNode selectedNode;

    private bool makeTransitionMode = false;

    Vector2 offset;
    Vector2 drag;

    //Styles
    GUIStyle inPointStyle;
    GUIStyle outPointStyle;

    //Connections
    List<Connection> connections;
    ConnectionPoint selectedInPoint;
    ConnectionPoint selectedOutPoint;

    [MenuItem("Window/Node Editor")]
    static void ShowEditor() {
        DLNodeEditor editor = EditorWindow.GetWindow<DLNodeEditor>();
    }
    private void OnEnable() {
        Debug.Log("meow");
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
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
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
        DrawNodes();
        DrawConnections();

        Event e = Event.current;

        mousePos = e.mousePosition;
        //Right mouse click when not in transition
        if (e.button == 1 && !makeTransitionMode) {
            if (e.type == EventType.MouseDown) {
                bool clickedOnWindow = false;
                int selectedIndex = -1;

                if (CheckIfSelected() != -1) {
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
        foreach (DLNode n in windows) {
            n.DrawCurves();
        }
        if (GUI.changed) Repaint();

    }
    private void DrawConnections() {
        if (connections != null) {
            for (int i = 0; i < connections.Count; i++) {
                connections[i].Draw();
            }
        }
    }
    void DrawNodes() {
        BeginWindows();
        for (int i = 0; i < windows.Count; i++) {
            windows[i].rect = GUI.Window(i, windows[i].rect, DrawNodeWindow, windows[i].windowTitle);
            windows[i].DrawPoints();
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
            DLNode node = new DLNode(new Rect(mousePos.x, mousePos.y, 500, 500), inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint );
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
        else if (clb.Equals("deleteNode")) {
            bool clickedOnWindow = false;
            int selectedIndex = -1;

            if (CheckIfSelected() != -1) {
                clickedOnWindow = true;
                selectedIndex = CheckIfSelected();
            }

            if (clickedOnWindow) {
                DLNode node = windows[selectedIndex];
                windows.RemoveAt(selectedIndex);

                foreach (DLNode n in windows) {
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

        for (int i = 0; i < 3; i++) {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowC, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
    #region Connections
    private void OnClickOutPoint(ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            }
            else {
                ClearConnectionSelection();
            }
        }
    }
    private void OnClickInPoint(ConnectionPoint inPoint) {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            }
            else {
                ClearConnectionSelection();
            }
        }
    }
    private void CreateConnection() {
        if (connections == null) {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }
    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
    private void OnClickRemoveConnection(Connection connection) {
        connections.Remove(connection);
    }
    #endregion
    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

}

