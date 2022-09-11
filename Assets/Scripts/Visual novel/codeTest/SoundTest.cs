using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip[] music;
    public float volum, pitch;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySFX(clips[Random.Range(0, clips.Length)],volum,pitch);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.instance.PlaySong(music[Random.Range(0, music.Length)]);
        }
    }
}
