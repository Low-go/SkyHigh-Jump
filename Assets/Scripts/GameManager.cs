using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = startTime;
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
        while (remainingTime > 0) { 
            
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
        
        gameOverScreen.setup(currentGold, (int)remainingTime, redoAmount);
    }
}
