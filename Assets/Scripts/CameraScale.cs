using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zoom the camera according to screen resolution
public class CameraScale : MonoBehaviour
{
    private Camera camera;
    private float zoom;
    [SerializeField] private SpriteRenderer size;
    void Start()
    {
        camera = GetComponent<Camera>();
        UpdateCameraSize(Screen.width, Screen.height);
    }
    

    public void UpdateCameraSize(int screenWidth, int screenHeight)
    {
        float targetAspectRatio = size.bounds.size.x/size.bounds.size.y;
        float currAspectRatio = (float)screenWidth / screenHeight;

        if (currAspectRatio >= targetAspectRatio)
        {
            float differenceInSize = (float)targetAspectRatio / currAspectRatio;
            camera.orthographicSize = size.bounds.size.y / 2; //fit the desired height
           
            camera.transform.position = size.transform.position + new Vector3((float)differenceInSize*(float)differenceInSize, 0, 0);
        }
        else if(currAspectRatio < targetAspectRatio)
        {
            float differenceInSize = targetAspectRatio / currAspectRatio;
           
            camera.orthographicSize = size.bounds.size.y / 2*differenceInSize; //fit the desired width
            camera.transform.position = size.transform.position;
        }
    }
}
