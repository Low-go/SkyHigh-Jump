using UnityEngine;

public class MainManager : MonoBehaviour
{

    public int playerHealth = 4;
    // Static instance that can be accessed from anywhere
    public static MainManager Instance { get; private set; }

    private void Awake()
    {
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
}