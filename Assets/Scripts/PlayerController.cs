using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    public bool grounded;
    public GameObject stompBox;
    [SerializeField] float climbSpeed;
    [SerializeField] float jumpReducer;

    public bool facingRight = true;
    private bool ignoreEnemy = false;

    private float knockbackCounter;
    public float knockbackXForce;
    public float knockbackYForce;
    public float knockbackLength;

    public float invincibilityLength;
    private float invincibilityCounter;
 
    private float move;
    public GameObject groundCheck;
    CapsuleCollider2D myBodyCollider;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    public float gravityScaleAtStart;
    Vector3 SpawnPosition;
    bool touchingLadder;
    MovingPlatform movingPlatform;
    public Vector3 respawnPosition;
    private GameManager theGameManager;





    // Start is called before the first frame update
    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        theGameManager = FindObjectOfType<GameManager>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        respawnPosition = transform.position;
        movingPlatform = FindObjectOfType<MovingPlatform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackCounter <= 0f)
        {
            
            Run();
        }

        if (knockbackCounter > 0f)
        {
            
            knockbackCounter -= Time.deltaTime;

            if(transform.localScale.x > 0) { 
                myRigidBody.velocity = new Vector2(-knockbackXForce, knockbackYForce);
            } else
            {
                myRigidBody.velocity = new Vector2(knockbackXForce, knockbackYForce);
            }  
        }

        if (invincibilityCounter > 0)
        {
            stompBox.SetActive(false);
            invincibilityCounter -= Time.deltaTime;
        }

        if (invincibilityCounter <= 0)
        {
            stompBox.SetActive(true);
            theGameManager.invincible = false;
        }
        
        if (move > 0 && !facingRight)
        {
            FlipSprite();
        }
        else if (move < 0 && facingRight)
        {
            FlipSprite();
        }

        Jump();
        ClimbLadder();
        //FlipSprite();
        StompBox();
    }

    private void Run()
    {
        move = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(move * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        touchingLadder = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        

        if (!touchingLadder) { 
            myAnimator.SetBool("Climbing", false);
            myAnimator.SetBool("Idle on Ladder", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

      
        
            myAnimator.SetBool("Idle on Ladder", true);
            myRigidBody.gravityScale = 0f;
      

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;



        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("Climbing", playerHasHorizontalSpeed);
        

    }

    private void Jump()
    {
        grounded = groundCheck.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
        

        if (!grounded) {
            myAnimator.SetBool("Grounded", false);
            if (CrossPlatformInputManager.GetButtonUp("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y / jumpReducer);
                myRigidBody.velocity = jumpVelocityToAdd;
            }
            return;
        } else
        {
            myAnimator.SetBool("Grounded", true);
        }


        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded)
        {
            
            Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;

            if (touchingLadder)
            {
                myRigidBody.gravityScale = gravityScaleAtStart;
            }
        

        }
        
    }

    private void FlipSprite()
    {
        /*  bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
          if (playerHasHorizontalSpeed)
          {
              transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
          } */

        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;
        theGameManager.invincible = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        {
            theGameManager.Respawn();
        }

        if (other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
            
            Debug.Log("Player reached checkpoint!");
        }
    }

    private void StompBox()
    {
        if (myRigidBody.velocity.y < 0)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }
    }




    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
        }

        if (other.gameObject.tag == "slope")
        {
            myRigidBody.gravityScale = 0f;
        }
   
    }

 

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }

        if (other.gameObject.tag == "slope")
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
        }
    }

}
