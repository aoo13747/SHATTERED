using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDialog : MonoBehaviour
{
    DialogSystem dialog;

    public GameObject visualNovelPanel;
    public GameObject dialogManager;

    public GameObject player;

    bool isPlayerMove;

    // Start is called before the first frame update
    void Start()
    {
        dialog = DialogSystem.instance;
        isPlayerMove = true;
    }

    public string[] conversation = new string[]
    {
        "Dio:???",
        "Jotaro:Dio",
        "Yare yare daze"
    };

    int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!dialog.isSpeaking || dialog.isWaitingForUserInput)
            {
                if (index >= conversation.Length && isPlayerMove) // if dialog is over, what to do next?
                {
                    visualNovelPanel.gameObject.SetActive(false);
                    dialogManager.SetActive(false);
                    player.GetComponent<CharacterController>().enabled = true;
                }
                Say(conversation[index]);
                index++;
            }
        }
    }

    void Say(string conversation)
    {
        string[] parts = conversation.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        dialog.Say(speech, speaker);
    }
}
