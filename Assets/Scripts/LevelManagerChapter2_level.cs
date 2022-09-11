using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerChapter2_level : MonoBehaviour
{
    public SceneFader sceneFader;
    public string SceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sceneFader.FadeTo(SceneToLoad);
        }
    }
}
