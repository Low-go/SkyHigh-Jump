using UnityEngine;

public class MainManager : MonoBehaviour
{

    public int playerHealth;
    public GameManager manager;
    public float time;
    public int gold;
    public int redo;
    public int initialPlayerHealth;
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
        initialPlayerHealth = playerHealth; // store it
    }


    public void ProgressToNextScene(GameManager currentGameManager)
    {
        // Capture current game state
        time = currentGameManager.getRemainingTime();
        gold = currentGameManager.currentGold;
        redo = currentGameManager.getRedo();

        HealthManager healthManager = FindObjectOfType<HealthManager>();
        if (healthManager != null)
        {
            healthManager.SaveHealthToMainManager();
        }


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

    public void ResetToInitialHealth()
    {
        playerHealth = initialPlayerHealth; // Reset to initial value
    }


}