using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel01 : MonoBehaviour
{
    public SceneFader sceneFader;
    string sceneToLoad = "Chapter0_Level";
    public void LoadLevel00_sim()
    {
        Time.timeScale = 1;
        sceneFader.FadeTo(sceneToLoad);
    }
}
