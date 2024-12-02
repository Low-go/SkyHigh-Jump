using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public HealthManager theHealhMan;
    public Renderer theRend;

    public Material cpOff;
    public Material cpOn;

    // Start is called before the first frame update
    void Start()
    {
        theHealhMan = FindObjectOfType<HealthManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPointOn()
    {
        CheckPoint[] checkpoints = FindObjectsOfType<CheckPoint>();
        foreach (CheckPoint cp in checkpoints)
        {
            cp.CheckPointOff();
        }
        theRend.material = cpOn;
    }

    public void CheckPointOff()
    {
        theRend.material = cpOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            theHealhMan.SetSpawnPoint(spawnPoint);
            CheckPointOn();
        }
    }
}
