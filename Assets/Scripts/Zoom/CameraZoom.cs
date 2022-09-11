using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;
    
    CinemachineVirtualCamera vCam;
    [SerializeField] int maxVcamOrthographicSize = 4, minVcamOrthographicSize = 2;
    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        MoveCamera();
        Zoom();
    }
    void MoveCamera()
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector2.up * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector2.down * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector2.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector2.right * panSpeed * Time.deltaTime, Space.World);
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        pos.z = -2;

        transform.position = pos;

        #region May Be Use In Future But Not Today
        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //Vector3 pos = transform.position;
        //pos.z -= scroll * 1000 * scrollSpeed * Time.deltaTime;




        #endregion
    }
    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0 && (scroll + vCam.m_Lens.OrthographicSize) >= minVcamOrthographicSize)
        {
            for (int scrollSpeed = 1; scrollSpeed > 0; scrollSpeed--)vCam.m_Lens.OrthographicSize--;                  
        }
        if (scroll < 0 && (scroll + vCam.m_Lens.OrthographicSize) <= maxVcamOrthographicSize)
        {
            for (int scrollSpeed = 1; scrollSpeed > 0; scrollSpeed--) vCam.m_Lens.OrthographicSize++;
        }
    }
}
   

