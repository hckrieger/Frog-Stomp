using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpController : MonoBehaviour
{
 
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float timeToTurn;
    [SerializeField] float timeOnGround;
    [SerializeField] float moveInAirSpeed;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myCollider2D;
    float startCounter;
    float startTimeOnGround;
    float startMoveSpeed;
    bool grounded;
    float startJumpSpeed;
    bool canMove = false;
    float gravityAtStart;


    // Start is called before the first frame update
    void Start()
    {

        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<BoxCollider2D>();
        startCounter = timeToTurn;
        startMoveSpeed = moveSpeed;
        startTimeOnGround = timeOnGround;
        startJumpSpeed = jumpSpeed;
        gravityAtStart = myRigidbody.gravityScale;

        if (gameObject.GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove) //If the gameObject is visible within the camera
        {
            // Set the running speed along the X axis
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

            //Check if the player is touching the grounded
            grounded = myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

            //Countdown in seconds when the game object should switch directions
            /* Declare variable outside of the 'grounded' if-statement so 
               it counts down even when the player is not on the ground */
            timeToTurn -= Time.deltaTime;

            if (grounded) //If the gameObject is on the ground then...
            {
                // set the move speed to what it was when the scene started
                moveSpeed = startMoveSpeed;

                // Cound down in seconds that the game Object is on the ground until it jumps
                timeOnGround -= Time.deltaTime;

                // If the declared amount of time counts down to zero the..
                if (timeOnGround <= 0)
                {
                    // Have the game Object jump along the Y Axis
                    myRigidbody.velocity += new Vector2(myRigidbody.velocity.x, jumpSpeed);

                    // Reset the countdown time
                    timeOnGround = startTimeOnGround;

                }


                if (timeToTurn <= 0) // If the declared time in which the gameObject switches directions counts down to zero
                {
                    // Multiply both move speeds by negative one so the game object goes in the opposite direction
                    startMoveSpeed *= -1;
                    moveInAirSpeed *= -1;

                    // Reset the countdown time in which the gameObject switches directions
                    timeToTurn = startCounter;

                }


            }
            else // If the game object is in the air
            {
                // Then have the game Object go a certain speed only when it's in the air
                moveSpeed = moveInAirSpeed;
            }


            



            FlipSprite();
            AnimateIfHasAnimator();


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

    private void AnimateIfHasAnimator()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (gameObject.GetComponent<Animator>() != null)
        {
            if (grounded && playerHasHorizontalSpeed)
            {
                myAnimator.SetBool("Grounded", true);
            }
            else
            {

                myAnimator.SetBool("Grounded", false);
            }

        }
    }

    void OnBecameVisible()
    {
        canMove = true;
    }




}

  


