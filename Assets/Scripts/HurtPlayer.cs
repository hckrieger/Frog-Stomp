using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
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
            GameObject inst = Instantiate(theGameManager.deadPlayer, other.transform.position, other.transform.rotation);

            Destroy(inst, 2f);

            theGameManager.Respawn();
        }
    }
}
