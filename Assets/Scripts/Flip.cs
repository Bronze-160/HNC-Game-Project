using UnityEngine;
using UnityEngine.InputSystem;

public class Flip : MonoBehaviour
{
    int direction = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(0.8f, 2, direction);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(0.8f, 2, direction);
        }
    }
}
