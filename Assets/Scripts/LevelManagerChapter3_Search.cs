using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerChapter3_Search : MonoBehaviour
{

    public static int trueDocview = 0;
    public string sceneToLoad;
    public string nextlevelToload;
    public SceneFader sceneFader;
    public  static bool isDoc1Viewed = false;
    public static bool isDoc2Viewed = false;
    public static bool isDoc3Viewed = false;

    private void Awake()
    {
        if(trueDocview == 3)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        Debug.Log(trueDocview);
        if (trueDocview >= 3)
        {
            sceneFader.FadeTo(nextlevelToload);            
        }        
    }
     
    public static void IncreaseTrueViewed()
    {
        if (isDoc1Viewed == false)
            trueDocview++;
        if (isDoc1Viewed == true)
            return;
    }
    public static void IncreaseTrueViewed_2()
    {
        if (isDoc2Viewed == false)
            trueDocview++;
        if (isDoc2Viewed == true)
            return;
    }
    public static void IncreaseTrueViewed_3()
    {
        if (isDoc3Viewed == false)
            trueDocview++;
        if (isDoc3Viewed == true)
            return;
    }

}
