using UnityEditor;
using UnityEngine.Events;

public class QTNode : BaseDLInputNode {
    public string[] answers;
    public Dialogue[] answerDialogues;

    public override void DrawWindow() {
        base.DrawWindow();
    }
    public override void DrawCurves() {

    }
}