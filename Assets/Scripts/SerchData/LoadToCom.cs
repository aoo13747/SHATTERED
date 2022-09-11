using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadToCom : MonoBehaviour
{
    public string sceneToLoad;
    public SceneFader sceneFader;
    public void LoadTo()
    {
        LevelManagerChapter3_Search.IncreaseTrueViewed();
        sceneFader.FadeTo(sceneToLoad);
        LevelManagerChapter3_Search.isDoc1Viewed = true;
    }
    public void LoadTo_2()
    {
        LevelManagerChapter3_Search.IncreaseTrueViewed_2();
        sceneFader.FadeTo(sceneToLoad);
        LevelManagerChapter3_Search.isDoc2Viewed = true;
    }
    public void LoadTo_3()
    {
        LevelManagerChapter3_Search.IncreaseTrueViewed_3();
        sceneFader.FadeTo(sceneToLoad);
        LevelManagerChapter3_Search.isDoc3Viewed = true;
    }
}
