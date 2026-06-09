using Unity.Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    /// <summary>
    /// Camera Change Script 
    /// By Finlay Macmillan
    /// </summary>

    public CinemachineBrain CinemachineBrain;//  Stores the CinemachineBrain component 

    public CinemachineCamera[] cameras; // Stores all the cameras
    public int currentCameraNumber;// checks what current camera the list is on

    [Header("Level 3 Cameras")] // All the cameras in Level 3
    public CinemachineCamera cameraStart;
    public CinemachineCamera camera_1;
    public CinemachineCamera camera_2;
    public CinemachineCamera camera_3;
    public CinemachineCamera camera_4;
    public CinemachineCamera camera_5;

    public CinemachineCamera startCamera; // Stores what camera to start with
    public CinemachineCamera currentCamera; // Stores what camera is currently on

    [Header("Level 3 Rooms")] // All the Rooms in Level 3
    public Collider2D room1;
    public Collider2D room2;
    public Collider2D room3;
    public Collider2D room4;
    public Collider2D room5;

    void Start()
    {
        currentCamera = cameraStart; // Makes sure that the start camera starts first
        cameraStart.Priority = 20; // Priority means the higher it is thats what camera you see from
        camera_1.Priority = 10;

        for (int i = 0; i < cameras.Length; i++) // Goes through every camera, making the current camera the priority
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

        Debug.Log(currentCamera);
    }

    public void SwitchCamera(CinemachineCamera newCamera) // This Switches to the a new camera in the list when function is called
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
