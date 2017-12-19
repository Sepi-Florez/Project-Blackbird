using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class Dialogue {
    public string name;
    [TextArea(3,10)]
    public string[] sentences;
    public bool options;
    public string[] answers;
    public Dialogue[] answerDialogues;
    public bool _continue;
    public Dialogue nextDialogue;
    public UnityEvent onEnd = new UnityEvent();
}
