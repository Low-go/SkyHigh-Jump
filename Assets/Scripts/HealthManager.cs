using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Image[] hearts;

    public PlayerController thePlayer;
    public float invincibilityLength;
    private float invincibilityCounter;

    public Renderer playerRenderer;
    public float flashCounter;
    public float flashLength = 0.1f;

    //respawn values, take not for future
    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnLength;

    // death variables
    public GameObject deathEffect;
    public Image blackScreen;
    private bool isFadeToBlack;
    private bool isFadeFromBlack;
    public float fadeSpeed;
    public float waitForFade;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //thePlayer = FindObjectOfType<PlayerController>();
        
        respawnPoint = thePlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // time period player does not take damage
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if(flashCounter <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashLength;
            }

            // make sure its on when damage is done
            if(invincibilityCounter <= 0)
            {
                playerRenderer.enabled = true;
            }
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        
        // responsible for fading black screen during respawns
        if (isFadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f)
            {
                isFadeToBlack = false;
            }
        }

        if (isFadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                isFadeFromBlack = false;
            }
        }


    }


    public void hurtPlayer(int damage, Vector3 direction)
    {
        // only take damage if player is not within invincibility time
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damage;

            // do i really want this? respawn
            if(currentHealth <= 0)
            {
                FindObjectOfType<GameManager>().addRedo();
                Respawn();
            }
            else
            {
                thePlayer.knockBack(direction);

                invincibilityCounter = invincibilityLength;

                playerRenderer.enabled = false;
                flashCounter = flashLength;
            }


        }
    }

    public void Respawn()
    {

        if (!isRespawning)
        {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo()
    {
        isRespawning = true;
        thePlayer.gameObject.SetActive(false);
        Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);

        yield return new WaitForSeconds(respawnLength);

        isFadeToBlack = true;

        yield return new WaitForSeconds(waitForFade);

        isFadeToBlack = false;
        isFadeFromBlack = true;

        isRespawning = false;

        thePlayer.gameObject.SetActive(true);
        CharacterController charController = thePlayer.GetComponent<CharacterController>();

        // Disable controller, move player, then re-enable
        charController.enabled = false;
        thePlayer.transform.position = respawnPoint;
        charController.enabled = true;

        // Reset health
        currentHealth = maxHealth;


        invincibilityCounter = invincibilityLength;

        playerRenderer.enabled = false;
        flashCounter = flashLength;



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
