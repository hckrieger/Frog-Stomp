using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    public GameObject deathSplosion;
    public float bounceForce;
    private Rigidbody2D playerRigidbody;
    public GameObject deadFrog;
    private PatrolController frog;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {

            other.gameObject.SetActive(false);

            GameObject d = Instantiate(deadFrog, other.transform.position, other.transform.rotation);

            Destroy(d, 2f);

            Instantiate(deathSplosion, other.transform.position, other.transform.rotation);

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, bounceForce);
            
        }
    }
}
