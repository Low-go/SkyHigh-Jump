using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{

    public GameManager gameManager;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            gameManager.gameOver = true;
            gameManager.GameOver();

        }
    }
}
