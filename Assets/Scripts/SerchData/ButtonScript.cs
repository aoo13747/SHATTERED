using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject itemToOpen;

    public void Open()
    {
        itemToOpen.SetActive(true);
    }

   public void Close()
   {
        gameObject.SetActive(false);
   }
}
