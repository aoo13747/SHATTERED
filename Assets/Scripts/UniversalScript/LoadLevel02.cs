using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel02 : MonoBehaviour
{
    public SceneFader sceneFader;
    string sceneToLoad = "Chapter2_Level";
    public void LoadLevel02_bar()
    {
        Time.timeScale = 1;
        sceneFader.FadeTo(sceneToLoad);
    }
}
