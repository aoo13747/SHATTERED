using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public SceneFader sceneFader;
    string sceneToLoad = "MainMenu";
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        sceneFader.FadeTo(sceneToLoad);
    }
}
