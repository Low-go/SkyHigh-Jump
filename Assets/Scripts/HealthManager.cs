using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public PlayerController thePlayer;
   
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurtPlayer(int damage, Vector3 direction)
    {
        currentHealth -= damage;

        thePlayer.knockBack(direction);
    }

    public void healPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
