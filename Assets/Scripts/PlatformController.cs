using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlatformEffector2D myPlatformEffector; 
    // Start is called before the first frame update
    void Start()
    {
        myPlatformEffector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
  void OnCollisionEnter2D(Collider2D other)
  {


          if (other.gameObject.tag == "Player")
          {
          Debug.Log("Player is touching effector");
             GetComponent<Collider2D>().myPlatformEffector.surfaceArc = 180;

          }



  }

      void OnCollisionExit2D(Collider2D other)
      {


          if (other.gameObject.tag == "Player")
          {

              GetComponent<Collider2D>().myPlatformEffector.surfaceArc = 179;

          }



      }  */




}
