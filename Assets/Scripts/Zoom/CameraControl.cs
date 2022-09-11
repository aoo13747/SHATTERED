using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] GameObject StartCam, ZoomCam;
    // Start is called before the first frame update
    void Start()
    {
        StartCam.SetActive(true);
        ZoomCam.SetActive(false);
    }

    // Update is called once per frame
        private void Update()
        {
            SwitchingCamera();
        }
        void SwitchingCamera()
        {
            if(Input.GetKeyDown(KeyCode.Space))            
            {
                CameraChangeCounter();
            }
        }
        void CameraChangeCounter()
        {
            int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
            cameraPositionCounter++;
            CameraPositionChange(cameraPositionCounter);
        }
        void CameraPositionChange(int camPosition)
        {
            if (camPosition > 1)
            {
                camPosition = 0;
            }

            PlayerPrefs.SetInt("CameraPosition", camPosition);
            if (camPosition == 0)
            {
                StartCam.SetActive(true);
                ZoomCam.SetActive(false);
            }
            if (camPosition == 1)
            {
                StartCam.SetActive(false);
                ZoomCam.SetActive(true);
            }
        }
    
}
