using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueTrigger : MonoBehaviour {
    [SerializeField]
    public Dialogue dialogue;
    
    public void TriggerDialogue() {
        DialogueManager.thisManager.ReadDialogue(dialogue);
    }
}
