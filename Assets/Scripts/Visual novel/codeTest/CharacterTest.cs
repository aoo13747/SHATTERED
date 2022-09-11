using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTest : MonoBehaviour
{
    public Character Ubi;
    // Start is called before the first frame update
    void Start()
    {
        Ubi = CharacterManager.instance.GetCharacter("Ubi", enbleCreatedCharacterOnStart: true);
        
    }
    public string[] speech;
    int i = 0;
    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;
    public int bodyIndex = 0;

    public float speed = 5f;
    public bool smoothtranstion = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (i < speech.Length)
                Ubi.Say(speech[i]);
            else
                DialogSystem.instance.Close();
            i++;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Ubi.MoveTo(moveTarget, moveSpeed, smooth);
           
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Ubi.StopMoving(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.R))
                Ubi.TransitionBody(Ubi.GetSprite(bodyIndex), speed, smoothtranstion);
            else
                Ubi.SetBody(bodyIndex);
            
        }
    }
}
