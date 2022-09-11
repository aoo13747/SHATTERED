using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadToDoc : MonoBehaviour
{
    public string SceneToLoad;
    public SceneFader sceneFader;
    public void LoadTo()
    {
        sceneFader.FadeTo(SceneToLoad);
    }
}
