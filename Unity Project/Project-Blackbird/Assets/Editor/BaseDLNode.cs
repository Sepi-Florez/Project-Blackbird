using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseDLNode : ScriptableObject {
    public Rect windowRect;
    public bool hasInputs = false;
    public string windowTitle = " ";
    
    public virtual void DrawWindow() {
        
    }

    public abstract void DrawCurves();

    public virtual void SetInput(BaseDLInputNode input, Vector2 clickPos) {

    }

    public virtual void NodeDeleted(BaseDLNode node) {

    }
    public virtual BaseDLInputNode ClickedOnInput(Vector2 pos) {
        return null;
    }

   
}
