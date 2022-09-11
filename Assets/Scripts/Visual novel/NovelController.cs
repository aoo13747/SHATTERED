using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    // Start is called before the first frame update
    List<string> data = new List<string>();
    int progress = 0;
    //  string ch = NovelLoad.Ch;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadChapterFile(NovelLoad.Ch);
        //UII = GetComponent<CanvasGroup>();
        //  Debug.Log(ch);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            HandleLine(data[progress]);
            progress++;
        }
    }
    public void LoadChapterFile(string fileName)
    {
        //data = FileManager.ReadTextAsset(Resources.Load<TextAsset>($"stroy/{fileName}"));
        //data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        data = FileManager.LoadFile(Application.streamingAssetsPath + "/Story/" + fileName);
        progress = 0;
        cachedLastSpeaker = "";
    }
    void HandleLine(string line)
    {
        string[] dialogAndAction = line.Split('"');

        if (dialogAndAction.Length == 3)
        {
            HandleDialog(dialogAndAction[0], dialogAndAction[1]);
            HandleEvent(dialogAndAction[2]);
        }
        else
        {
            HandleEvent(dialogAndAction[0]);
        }
    }
    string cachedLastSpeaker = "";
    void HandleDialog(string dialogDetails, string dialog)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogDetails.Contains("+");
        if (additive)
            dialogDetails = dialogDetails.Remove(dialogDetails.Length - 1);
        if (dialogDetails.Length > 0)
        {
            if (dialogDetails[dialogDetails.Length - 1] == ' ')
                dialogDetails = dialogDetails.Remove(dialogDetails.Length - 1);

            speaker = dialogDetails;
            cachedLastSpeaker = speaker;

        }
        if (speaker != "narrator")
        {
            Character character = CharacterManager.instance.GetCharacter(speaker);
            character.Say(dialog, additive);
        }
        else
        {
            DialogSystem.instance.Say(dialog, speaker, additive);
        }
    }
    void HandleEvent(string events)
    {
        //print("Handle event [" + event + "]");
        string[] actions = events.Split(' ');
 
        foreach (string action in actions)
        {
            HandleAction(action);
        }
    }
    void HandleAction(string action)
    {
        print("execute command - " + action);
        string[] data = action.Split('(', ')');

        switch (data[0])
        {
            case "enter":
                Command_Enter(data[1]);
                break;

            case "exit":
                Command_Exit(data[1]);
                break;
            case ("setBackground"):
                Command_SetLayerImger(data[1], Allbackground.instance.background);
                break;
            case ("setForeground"):
                Command_SetLayerImger(data[1], Allbackground.instance.foreground);
                break;
            case ("playSound"):
                Command_PlaySound(data[1]);
                break;
            case ("playMusic"):
                Command_PlayMusic(data[1]);
                break;
            case ("move"):
                Command_MoveCharacter(data[1]);
                break;

            case ("setPosition"):
                Command_SetPositions(data[1]);
                break;
            case ("changeExpression"):
                Command_ChangeExpression(data[1]);
                break;
            case ("outUI"):
                Command_fadeoutUI();
                break;
            case ("inUI"):
                Command_fadeinUI();
                break;
            case ("NextCh"):
                Command_Nextch();
                break;
            case ("CloseLog"):
                Command_CloseLog();
                break;
            case "Load":
                Command_Load(data[1]);
                break;
                /* case "transBackground":
                     Command_TransLayer(Allbackground.instance.background, data[1]);
                     break;
                 case "transForeground":
                     Command_TransLayer(Allbackground.instance.foreground, data[1]);
                     break;*/
        }
    }

    

    void Command_SetLayerImger(string data, Allbackground.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = texName == "null" ? null : Resources.Load("Background/" + texName) as Texture2D;
        float spd = 2f;
        bool smooth = false;
        if (data.Contains(","))
        {
            string[] paramemters = data.Split(',');
            foreach (string p in paramemters)
            {
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                {
                    spd = fVal; continue;
                }
                if (bool.TryParse(p, out bVal))
                {
                    smooth = bVal; continue;
                }
            }
        }
        layer.TransitionToTexture(tex, spd, smooth);
    }
    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if (clip != null)
        {
            AudioManager.instance.PlaySFX(clip);
        }
        else
            Debug.LogError("Clip does not exist - " + data);
    }
    void Command_PlayMusic(string data)
    {

        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        
            AudioManager.instance.PlaySong(clip);
        
    }
    void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);
        float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 1f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;
        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed);
    }
    void Command_SetPositions(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);
        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
        print("set " + c.characterName + " position to " + locationX + "," + locationY);
    }
    void Command_ChangeExpression(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string region = parameters[1];
        string expression = parameters[2];
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f;
        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);
        if (region.ToLower() == "body")
            c.TransitionBody(sprite,speed,false);
    }
    void Command_Exit(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }
    void Command_Enter(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s, true, false);
            if (!c.enabled)
            {
                c.renderers.bodyRenderer.color = new Color(1, 1, 1, 0);
                c.enabled = true;

                c.TransitionBody(c.renderers.bodyRenderer.sprite, speed, smooth);
            }
            else
                c.FadeIn(speed, smooth);
        }
    }
    public CanvasGroup UII;
    float fadeInDelay = 3;

    void Command_fadeoutUI()
    {
        UII.alpha = 0;
    }
    void Command_fadeinUI()
    {
        UII.alpha = 1;
    }
    public int n;
    void Command_Nextch()
    {
        SceneManager.LoadScene(n);
    }
    public GameObject Log;
    void Command_CloseLog()
    {
        Log.SetActive(false);
    }

    void Command_Load(string chapterName)
    {
        NovelController.instance.LoadChapterFile(chapterName);
    }
    
    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    Coroutine handlingChapterFile = null;
    /* void Command_TransLayer(Allbackground.LAYER layer, string data)
     {
         string[] parameters = data.Split(',');

         string texName = parameters[0];
         string transTexName = parameters[1];
         Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
         Texture2D transTex = Resources.Load("Images/TransitionEffects/" + transTexName) as Texture2D;

         float spd = 2f;
         bool smooth = false;

         for (int i = 2; i < parameters.Length; i++)
         {
             string p = parameters[i];
             float fVal = 0;
             bool bVal = false;
             if (float.TryParse(p, out fVal))
             { spd = fVal; continue; }
             if (bool.TryParse(p, out bVal))
             { smooth = bVal; continue; }
         }

         TransitionMaster.TransitionLayer(layer, tex, transTex, spd, smooth);
     }*/
}
