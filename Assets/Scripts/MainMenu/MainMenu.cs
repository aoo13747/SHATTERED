using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneToLoad;
    public SceneFader sceneFader;
    public void LoadTo()
    {
        sceneFader.FadeTo(SceneToLoad);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
