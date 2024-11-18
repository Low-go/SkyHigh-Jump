using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offSet;
    public bool useOffsetValues;

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
        transform.LookAt(target);
        transform.position = target.position - offSet;
    }
}
