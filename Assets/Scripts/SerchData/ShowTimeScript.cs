using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShowTimeScript : MonoBehaviour
{
    public TMP_Text clockTimeText;
    private void FixedUpdate()
    {
        clockTimeText.text = DateTime.Now.ToString("HH:mm\ndd/MM/yyyy"); 
    }
}
