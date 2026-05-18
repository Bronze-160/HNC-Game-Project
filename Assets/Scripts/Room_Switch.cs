using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
public class Room_Switch : MonoBehaviour
{
    //Finlay Macmillan
    //Level 3 Cameras
    static List<CinemachineCamera> cameras = new List<CinemachineCamera>();

    public static CinemachineCamera activeCamera = null;

    [Header(" Level 3 Cameras ")]
    [SerializeField] CinemachineCamera camera_1;
    [SerializeField] CinemachineCamera camera_2;

    void OnEnable()
    {
        Room_Switch.Register(camera_1);
        Room_Switch.Register(camera_2);
    }

    void OnDisable()
    {
        Room_Switch.UnRegister(camera_1);
        Room_Switch.UnRegister(camera_2);
    }

    public static bool isActiveCamera(CinemachineCamera camera)
    {
        return camera == activeCamera;
    }

    public static void SwitchCamera(CinemachineCamera camera)
    {
        camera.Priority = 10;
        activeCamera = camera;

        foreach (CinemachineCamera c in cameras)
        {
            if(c != camera && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineCamera camera)
    {
        cameras.Add(camera);
    }

    public static void UnRegister(CinemachineCamera camera)
    {
        cameras.Remove(camera);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        //switch between cameras
        if (other.gameObject.CompareTag("Player"))
        {
            if (isActiveCamera(camera_1))
            {
                Debug.Log("cam 1");
                SwitchCamera(camera_2);
            }
            else if (isActiveCamera(camera_2))
            {
                Debug.Log("cam 2");
                SwitchCamera(camera_1);
            }
        }
        
    }
}
