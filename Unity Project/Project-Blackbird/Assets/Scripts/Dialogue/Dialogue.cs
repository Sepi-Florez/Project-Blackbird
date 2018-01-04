using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject {
    public enum TypeDL { Sentences, Answers, Event };
    public TypeDL type;

    //Sentences
    public new string name;
    [TextArea(3,10)]
    public string[] sentences;

    //Answers
    public string question;
    public string[] answers;
    public Dialogue[] answerDialogues;

    //Event
    public UnityEvent onEnd = new UnityEvent();

    //Multiple
    public Dialogue nextDialogue;
}
