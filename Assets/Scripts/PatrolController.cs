using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    public Transform originPoint;
    public Transform originPoint2;
    private Vector2 dir = new Vector2(-1, 0);
    public float speed;
    public float range;
    public float range2;
    public float jumpSpeed;
    public float jumpOnPlayerSpeed;
    BoxCollider2D myCollider2D;
    Rigidbody2D myRigidbody;
    bool canMove = false;
    bool grounded;
    float startSpeed;
    private Vector2 startDir;
    float startMoveInAirSpeed;
    public float moveInAirSpeed;
    private PlayerController player;
    private Animator myAnimator;
    private GameManager theGameManager;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerController>();
        startSpeed = speed;
        startDir = dir;
        startMoveInAirSpeed = moveInAirSpeed;
        theGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawRay(originPoint.position, dir * range);
        Debug.DrawRay(originPoint2.position, dir * range2);

        //Code that gets the enemies to jump over ground obstructions
        RaycastHit2D hit = Physics2D.Raycast(originPoint.position, dir, range);
        RaycastHit2D hit2 = Physics2D.Raycast(originPoint2.position, dir, range2);

        grounded = myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (hit == true)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                 myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
    
            }

            if (hit.collider.CompareTag("Stop Guard"))
            {
                myRigidbody.velocity = new Vector2(0f, 0f);

            }
        }

        if (hit2 == true)
        { 
            if (hit2.collider.CompareTag("Player"))
            {
                if (grounded)
                {
                    myRigidbody.velocity = new Vector2(10f, jumpOnPlayerSpeed);

                }
            }

        /*    if (hit.collider.CompareTag("turn around"))
            {
                startSpeed *= -1;
                moveInAirSpeed *= -1;
                dir *= -1;
            } */

            
        }

        FlipSprite();
        AnimateIfHasAnimator();
    }

    void FixedUpdate()
    {
        
        //Get the enemies to chase the player no matter which direction he's going. 
        if (canMove && player.gameObject.activeInHierarchy)
        {
          
            if (transform.position.x < player.transform.position.x) {
                if (player.grounded && grounded)
                {
                    myRigidbody.velocity = new Vector2(speed, myRigidbody.velocity.y);

                    moveInAirSpeed = -startMoveInAirSpeed;
                    dir = -startDir;

                }

            } else if (transform.position.x > player.transform.position.x)
            {

                if (player.grounded && grounded)
                {
                    myRigidbody.velocity = new Vector2(-speed, myRigidbody.velocity.y);
                    

                    moveInAirSpeed = startMoveInAirSpeed;
                    dir = startDir;

                }
            } 

            if (grounded)
            {
                speed = startSpeed;
            }
            
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void OnBecameVisible()
    {
        canMove = true;
    }

    private void AnimateIfHasAnimator()
    {
        if (gameObject.GetComponent<Animator>() != null)
        {
            if (grounded)
            {
                myAnimator.SetBool("Grounded", true);
            }
            else
            {

                myAnimator.SetBool("Grounded", false);
            }

        }
    }

    void OnEnable()
    {
        canMove = false;
    }
}
