using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        GameManager currentGameManager = FindObjectOfType<GameManager>();
        currentGameManager.StopCountdownTimer();
        MainManager.Instance.ProgressToNextScene(currentGameManager);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
