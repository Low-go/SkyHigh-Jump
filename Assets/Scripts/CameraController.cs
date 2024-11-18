using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offSet;
    public bool useOffsetValues;
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues)
        {
            offSet = target.position - transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get the x position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        target.Rotate(vertical, 0, 0);

        // move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = target.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offSet);

        
        //transform.position = target.position - offSet;
        transform.LookAt(target);
    }
}
