using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offSet;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivet;
    public float maxViewAngle;
    public float minViewAngle;
    public bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues)
        {
            offSet = target.position - transform.position;
        }

        pivet.transform.position = target.transform.position;
        //pivet.transform.parent = target.transform;
        pivet.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController.isGameOver)
        {
            return; // Stop camera movement during game over
        }

        pivet.transform.position = target.transform.position;

        // get the x position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivet.Rotate(0, horizontal, 0);

        // Get the Y position of the mouse and rotate the Pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        
        if (invertY)
        {
            pivet.Rotate(vertical, 0, 0);
        }
        else
        {
            pivet.Rotate(-vertical, 0, 0);
        }

        //limit the up/down camera rotation
        if (pivet.rotation.eulerAngles.x > maxViewAngle && pivet.rotation.eulerAngles.x < 180f)
        {
            pivet.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }
        if(pivet.rotation.eulerAngles.x > 180 && pivet.rotation.eulerAngles.x < 360f + minViewAngle)
        {
            pivet.rotation = Quaternion.Euler(36f + minViewAngle, 0, 0);
        }

        // move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = pivet.eulerAngles.y;
        float desiredXAngle = pivet.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offSet);

        
        //transform.position = target.position - offSet;

        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }
        transform.LookAt(target);
    }
}
