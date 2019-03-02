using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float waitToRespawn;
    private PlayerController thePlayer;
    public GameObject deadPlayer;
    public int coinCount;
    public Text coinText;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    private bool respawning;

    public bool invincible;

    public ResetOnRespawn[] objectsToReset;

    public Text livesText;
    public int startingLives;
    public int currentLives;

    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        coinText.text = "X " + coinCount;

        healthCount = maxHealth;

        objectsToReset = FindObjectsOfType<ResetOnRespawn>();

        currentLives = startingLives;
        livesText.text = "X " + currentLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthCount <= 0 && !respawning)
        {
            Respawn();
            respawning = true;
        }
    }

    public void Respawn()
    {
        currentLives -= 1;
        livesText.text = "X " + currentLives;

        if (currentLives > 0)
        {
            StartCoroutine("RespawnCo");
        } else
        {
            thePlayer.gameObject.SetActive(false);
            gameOverScreen.SetActive(true);
        }
        
    }

    public IEnumerator RespawnCo()
    {
        thePlayer.gameObject.SetActive(false);

        GameObject inst = Instantiate(deadPlayer, thePlayer.transform.position, thePlayer.transform.rotation);
        Destroy(inst, 2f);

        yield return new WaitForSeconds(waitToRespawn);

        healthCount = maxHealth;
        respawning = false;
        thePlayer.GetComponent<Rigidbody2D>().gravityScale = thePlayer.gravityScaleAtStart;
        UpdateHeartMeter();

        thePlayer.transform.position = thePlayer.respawnPosition;
        thePlayer.gameObject.SetActive(true);

        for (int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].ResetObject();
            objectsToReset[i].gameObject.SetActive(true);
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
        coinText.text = "X " + coinCount;

        if (coinCount == 25)
        {
            currentLives += 1;
            coinCount = 0;
            coinText.text = "X " + coinCount;
            livesText.text = "X " + currentLives;
        }
    }

    public void HurtPlayer(int damageToTake)
    {

        if (!invincible)
        {
            healthCount -= damageToTake;
            if (healthCount > 0)
            {
                thePlayer.Knockback();
            }
        }
    }

    public void GiveHealth(int healthToGive)
    {
        healthCount += healthToGive;

        if(healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }

        UpdateHeartMeter();
    }

    public void UpdateHeartMeter()
    {
        switch(healthCount)
        {
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;

            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;

            case 1:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

                default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;

        }

    }

    public void AddLives(int livesToAdd)
    {
        currentLives += livesToAdd;
        livesText.text = "X " + currentLives;
    }


}
