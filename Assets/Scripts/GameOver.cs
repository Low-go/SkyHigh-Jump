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
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1); // I think an index is ok
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }
}