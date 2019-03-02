using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private GameManager theGameManager;
    public int damageToGive;
    
    // Start is called before the first frame update
    void Start()
    {
        theGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theGameManager.HurtPlayer(damageToGive);
            theGameManager.UpdateHeartMeter();
            //GameObject inst = Instantiate(theGameManager.deadPlayer, other.transform.position, other.transform.rotation);

            //Destroy(inst, 2f);

            //theGameManager.Respawn();
        }
    }
}
