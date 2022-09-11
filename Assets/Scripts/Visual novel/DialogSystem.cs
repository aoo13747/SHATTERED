using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;
    public ELEMENTS elements;

    void Awake()
    {
        instance = this;
    }

    public void Say(string dialog, string speaker = "",bool additive = false)
    {
        StopSpeaking();
        if (additive)
        dialogText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(dialog, additive,  speaker));
    }
   
    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    
    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput = false;
    public string targetSpeech = "";
    Coroutine speaking = null;
    TextArchitect textArchitect = null;
    IEnumerator Speaking(string targetDialog, bool additive, string speaker = "")
    {
        dialogPanel.SetActive(true);
        dialogText.text = "";
        speakerNameText.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = false;

        while (dialogText.text != targetDialog)
        {
            dialogText.text += targetDialog[dialogText.text.Length];
            yield return new WaitForEndOfFrame();
        }

        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
        {
            yield return new WaitForEndOfFrame();
        }

        StopSpeaking();
    }


    string DetermineSpeaker(string speakerName)
    {
        string retVal = speakerNameText.text;
        if (speakerName != speakerNameText.text && speakerName != "")
        {
            retVal = (speakerName.ToLower().Contains("narrator")) ? "" : speakerName;
        }

        return retVal;
    }

    public void Close()
    {
        StopSpeaking();
        dialogPanel.SetActive(false);
    }
    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject dialogPanel;
        public Text speakerNameText;
        public Text dialogText;
    }

    public GameObject dialogPanel { get { return elements.dialogPanel; } }
    public Text speakerNameText { get { return elements.speakerNameText; } }
    public Text dialogText { get { return elements.dialogText; } }
}

