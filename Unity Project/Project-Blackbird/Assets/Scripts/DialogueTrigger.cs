using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    public Dialogue dialogue;
    public string[] h;
    
    public void TriggerDialogue() {
        Dialogue_UI.thisManager.StartDialogue(dialogue);
    }
}
