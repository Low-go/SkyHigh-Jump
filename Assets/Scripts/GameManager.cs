using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentGold;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI redoText;
    public int startTime;
    private float remainingTime;
    private int redoAmount = 0;
    public GameOver gameOverScreen;
    public bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            Debug.Log("In GameManager");
            Debug.Log("Gold: " + MainManager.Instance.gold);
            Debug.Log("Redo: " + MainManager.Instance.redo);
            Debug.Log("Current Time: " + MainManager.Instance.time);

            remainingTime = MainManager.Instance.time;
            currentGold = MainManager.Instance.gold;
            redoAmount = MainManager.Instance.redo;
        }

        if (remainingTime == 0)
        {
            remainingTime = startTime;
        }

        // Update the UI right after setting the values
        goldText.text = "Gold: " + currentGold;
        redoText.text = "Redo: " + redoAmount;

        StartCoroutine(CountdownTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int goldToAdd)
    {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold + "!"; // update gold score UI
    }

    IEnumerator CountdownTimer()
    {
        while (remainingTime > 0 && gameOver == false) { 
            
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        // time ran out

        timeText.text = "00:00";

        //gameover function should go here
        Debug.Log("Game is over");
        GameOver();
    }

    public void addRedo()
    {
        redoAmount += 1;
        redoText.text = "Redo: " + redoAmount;
    }

    public void GameOver()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.isGameOver = true;
        gameOverScreen.setup(currentGold, (int)remainingTime, redoAmount);
    }

    //getter and setter methods for remaining time

    public float getRemainingTime()
    {
        return remainingTime;
    }

    public void setRemainingTime(float value)
    {
        remainingTime = value;
    }

    //getter and setter methods for redoAmount

    public int getRedo()
    {
        return redoAmount;
    }

    public void setRedo(int value)
    {
        redoAmount = value;
    }

    // for transitioning scenes or anything else
    public void StopCountdownTimer()
    {
        // Stop the coroutine if it's running
        StopAllCoroutines();
    }


}
