using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class layerTest : MonoBehaviour
{
    Allbackground controller;
    public Texture tex;
    public float speed;
    public bool smooth;
    void Start()
    {
       controller = Allbackground.instance;
    }
     void Update()
    {
        Allbackground.LAYER Layer = null;
        if (Input.GetKey(KeyCode.Q))
            Layer = controller.background;
        if (Input.GetKey(KeyCode.W))
            Layer = controller.foreground;
        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.A))
            {
                Layer.TransitionToTexture(tex, speed, smooth);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.B))
                    Layer.SetTexture(tex);
         
        }
    }
}
