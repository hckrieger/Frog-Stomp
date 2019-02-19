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
    

    //[SerializeField] GameObject accessPlatform;
    private PlatformController changeEffector;

    BoxCollider2D myFeet;
    CapsuleCollider2D myBodyCollider;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    float gravityScaleAtStart;
    Vector3 SpawnPosition;
    bool touchingLadder;
    MovingPlatform movingPlatform;
    public Vector3 respawnPosition;
    private GameManager theGameManager;



    // Start is called before the first frame update
    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        theGameManager = FindObjectOfType<GameManager>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        respawnPosition = transform.position;
        changeEffector = FindObjectOfType<PlatformController>();
        movingPlatform = FindObjectOfType<MovingPlatform>();

    }

    // Update is called once per frame
    void Update()
    {
        Run();
        ClimbLadder();
        Jump();
        FlipSprite();
        StompBox();
 

    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        touchingLadder = myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        

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
        grounded = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        

        if (!grounded) {
            myAnimator.SetBool("Grounded", false);

            if (CrossPlatformInputManager.GetButtonUp("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y / jumpReducer);
                myRigidBody.velocity = jumpVelocityToAdd;
            }
            return;
        } else if (grounded)
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
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }






    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        {
            theGameManager.Respawn();
        }

        if (other.tag == "coin")
        {
            other.gameObject.SetActive(false);
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
        } else
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

        /*while (gameObject.GetComponent<Collider2D>().GetType() == typeof(BoxCollider2D))
        {
            if (other.gameObject.tag == "main ground")
            {
                changeEffector.myPlatformEffector.surfaceArc = 179;
            } else if (other.gameObject.tag == "effector ground")
            {
                changeEffector.myPlatformEffector.surfaceArc = 180;
            } 
        }*/
   
    }

 

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }

}
