using UnityEngine;

public class MainManager : MonoBehaviour
{

    public int playerHealth;
    public GameManager manager;
    public float time;
    public int gold;
    public int redo;
    //private GameObject player;


    // Static instance that can be accessed from anywhere
    public static MainManager Instance { get; private set; }

    private void Awake()
    {
        //player = GameObject.FindWithTag("Player");
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance
        Instance = this;

        // Ensure the object persists across scene loads
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerHealth(float healthValue)
    {
        playerHealth = Mathf.RoundToInt(healthValue);  // Convert slider value to integer
    }


    public void ProgressToNextScene(GameManager currentGameManager)
    {
        // Capture current game state
        time = currentGameManager.getRemainingTime();
        gold = currentGameManager.currentGold;
        redo = currentGameManager.getRedo();
        

        Debug.Log("Current Time: " + time);
        Debug.Log("Current Gold: " + gold);
        Debug.Log("Current Redo: " + redo);

        // Apply scene transition bonuses
        time += 30f;
        playerHealth += 2;

        // Ensure health doesn't exceed max
        playerHealth = Mathf.Clamp(playerHealth, 0, 6);

        // Transfer prepared data back to GameManager
        //PrepareGameManagerForNextScene(currentGameManager);
    }

    // Method to set up GameManager for the next scene
    //private void PrepareGameManagerForNextScene(GameManager gameManager)
    //{
    //    // Set all transferred values in GameManager
    //    gameManager.setRemainingTime(time);
    //    gameManager.currentGold = gold;
    //    gameManager.setRedo(redo);
    //}

}