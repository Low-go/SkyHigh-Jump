using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI redoText;
    public GameObject restartButton;
    public GameObject exitButton;

    void Start()
    {
        // Unlock the cursor and make it visible when the game over screen appears
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //variables passed in are those to be displayed on gameOver screen
    public void setup(int gold, int timeLeft, int redoAmount)
    {
        gameObject.SetActive(true);
        goldText.text = "Gold: " + gold;
        redoText.text = "Redo: " + redoAmount;

        //convert seconds to formatted string
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void RestartButton()
    {
        Debug.Log("Restart Button Pressed!");

        // Check if MainManager.Instance is null
        if (MainManager.Instance == null)
        {
            Debug.LogError("MainManager.Instance is null! Ensure it is initialized before calling RestartButton.");
            return;
        }
        MainManager.Instance.ResetToInitialHealth();

        // Check if PlayerController exists
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found! Ensure a PlayerController exists in the scene.");
            return;
        }
        playerController.isGameOver = false;

        // Log restartButton and exitButton status
        if (restartButton == null)
        {
            Debug.LogError("RestartButton is null! Make sure it is assigned in the inspector.");
            return;
        }
        if (exitButton == null)
        {
            Debug.LogError("ExitButton is null! Make sure it is assigned in the inspector.");
            return;
        }

        restartButton.SetActive(false);
        exitButton.SetActive(false);

        Debug.Log("Loading Scene 1...");
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Debug.Log("Menu Button Pressed!");
        SceneManager.LoadScene(0);
    }
}
