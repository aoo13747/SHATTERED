using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerChapter0 : MonoBehaviour
{
    public int allEnemy;
    public static int enemyAlive;
    public string sceneToLoad;
    public SceneFader sceneFader;

    bool completeLevel;

    private void Awake()
    {
        enemyAlive = allEnemy;
    }
    private void Start()
    {
        completeLevel = false;
    }
    private void FixedUpdate()
    {                         
        if (enemyAlive > 0)
            return;
        if (enemyAlive == 0 && completeLevel==false)
        {
            completeLevel = true;
            Debug.Log("LEVEL COMPLETE");
        }
        if(completeLevel == true)
        {
            sceneFader.FadeTo(sceneToLoad);
        }
    }    
}
