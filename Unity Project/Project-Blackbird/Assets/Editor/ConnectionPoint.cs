using System;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint {
    public Rect rect;

    public ConnectionPointType type;

    public DLNode node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(DLNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint) {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw() {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + -7f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - -6f;
                break;
        }

        if (GUI.Button(rect, "", style)) {
            if (OnClickConnectionPoint != null) {
                OnClickConnectionPoint(this);
            }
        }
    }
    public void Draw2(int i) {
        rect.y = i;

        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + -7f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - -10f;
                break;
        }

        if (GUI.Button(rect, "", style)) {
            if (OnClickConnectionPoint != null) {
                OnClickConnectionPoint(this);
            }
        }
    }
}