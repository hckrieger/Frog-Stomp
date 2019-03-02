using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLives : MonoBehaviour
{
    public int livesToGive;
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
            theGameManager.AddLives(livesToGive);
            gameObject.SetActive(false);
        }
    }
}
