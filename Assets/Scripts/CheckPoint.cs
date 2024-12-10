using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public HealthManager theHealhMan;
    public Renderer theRend;

    public Material cpOff;
    public Material cpOn;

    private bool clipOnce = false;
    private bool isActive = false; // Track if the checkpoint is active
    public AudioClip checkSound;

    void Start()
    {
        theHealhMan = FindObjectOfType<HealthManager>();
    }

    public void CheckPointOn()
    {
        CheckPoint[] checkpoints = FindObjectsOfType<CheckPoint>();
        foreach (CheckPoint cp in checkpoints)
        {
            cp.CheckPointOff();
        }
        theRend.material = cpOn;
        isActive = true; // Mark this checkpoint as active
    }

    public void CheckPointOff()
    {
        theRend.material = cpOff;
        isActive = false; // Mark this checkpoint as inactive
        clipOnce = false; // Reset sound trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!isActive) // Only activate if not already active
            {
                Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
                theHealhMan.SetSpawnPoint(spawnPoint);
                CheckPointOn();

                if (!clipOnce)
                {
                    AudioSource.PlayClipAtPoint(checkSound, transform.position);
                    clipOnce = true;
                }
            }
        }
    }
}
