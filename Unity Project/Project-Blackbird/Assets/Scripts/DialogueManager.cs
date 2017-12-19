using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour {
    //Main Variables
    public static DialogueManager thisManager;

    private Dialogue currentDialogue;

    private Queue<string> sentences = new Queue<string>();

    public GameObject dialogueObject;
    public Text nameText;
    public Text dialogueText;

    public float textSpeed;

    bool loaded = true;
    bool asking = false;

    string currentSentence;

    List<UnityEvent> events = new List<UnityEvent>();

    //Anwers
    public Transform answerPanel;
    List<CanvasGroup> answerObjects = new List<CanvasGroup>();


    #region General
    public void Awake() {
        thisManager = this;
        foreach(Transform child in answerPanel) {
            answerObjects.Add(child.GetComponent<CanvasGroup>());
        }
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.K)){
            Click();
        }
    }
    #endregion
    // Registers if players pressed the desired button or chat window to procceed with the dialogue. If the dialogue is at its end, this will call the EndDialogue function
    public void Click() {
        NextDialogue();
    }
    // Start a new conversation with the given dialogue object
    public void StartDialogue(Dialogue dialogue) {
        currentDialogue = dialogue;
        print("Starting Conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        events.Add(currentDialogue.onEnd);
        NextDialogue();
    }
    //Changes Dialogue to the next sentence in queue or if the previous sentence isn't done loading yet it will complete it.
    //This will also check if there are no sentences left and then call upon the EndDialogue function
    public void NextDialogue() {
        if (!loaded) {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            loaded = true;
            print("Loading next sentence");
            return;
        }
        if(sentences.Count == 0 && currentDialogue.options && !asking) {
            print("Loading Answers");
            ActivateAnswers(currentDialogue.answers.Length);
            return;
        }
        else if(sentences.Count == 0 && !asking) {
            EndDialogue();
            return;
        }
        loaded = false;
        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }
    public void ActivateAnswers(int count) {
        asking = true;
        for (int i = 0; i < count; i++) {
            answerObjects[answerObjects.Count - 1 - i].alpha = 1;
            answerObjects[answerObjects.Count - 1 - i].interactable = true;
            answerObjects[answerObjects.Count - 1 - i].transform.GetChild(0).GetComponent<Text>().text = currentDialogue.answers[i];
        }
    }
    public void DisableAnswers() {
        foreach(CanvasGroup answer in answerObjects) {
            answer.alpha = 0;
            answer.interactable = false;
        }
        asking = false;
    }
    public void Answer(int answer) {
        DisableAnswers();
        StartDialogue(currentDialogue.answerDialogues[answer]);
    }
    //Destroys dialogue object
    public void EndDialogue() {
        foreach(UnityEvent ev in events){
            ev.Invoke();
        }
        Destroy(dialogueObject);
    }
    public void Test() {
        print("Event is activated!");
    }

    //Fills the Dialogue box with the next dialogue char for char
    IEnumerator TypeSentence(string dialogue) {
        dialogueText.text = "";
        foreach(char lt in dialogue.ToCharArray()) {
            dialogueText.text += lt;
            yield return new WaitForSeconds(textSpeed);
        }
        loaded = true;
    }

}

