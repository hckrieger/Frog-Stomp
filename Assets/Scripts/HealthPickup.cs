using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthToGive;
    private GameManager theGameManager;

    // Start is called before the first frame update
    void Start()
    {
        theGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theGameManager.GiveHealth(healthToGive);
            gameObject.SetActive(false);
        }
    }
}
