using Unity.Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    //Finlay Macmillan

    public CinemachineBrain CinemachineBrain;

    public CinemachineCamera[] cameras;
    public int currentCameraNumber;// checks what current camera the list is on

    [Header("Level 3 Cameras")]
    public CinemachineCamera cameraStart;
    public CinemachineCamera camera_1;
    public CinemachineCamera camera_2;
    public CinemachineCamera camera_3;

    public CinemachineCamera startCamera;
    public CinemachineCamera currentCamera;

    [Header("Level 3 Rooms")]
    public Collider2D room1;
    public Collider2D room2;
    public Collider2D room3;

    void Start()
    {
        currentCamera = cameraStart;
        cameraStart.Priority = 20;

        for(int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCamera)
            {
                cameras[i].Priority = 20;
                currentCameraNumber = i;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }

    public void SwitchCamera(CinemachineCamera newCamera)
    {
            currentCamera = newCamera;
            currentCamera.Priority = 20;
        

        for (int i = 0;  i < cameras.Length;i++)
        {
            if(cameras[i] != currentCamera)
            {
                cameras[i].Priority = 10;
            }
            else
            {
                currentCameraNumber = i;
            }
        }
    }
}
