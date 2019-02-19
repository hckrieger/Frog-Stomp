using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;
    public Transform leftGuard;
    public Transform rightGuard;
    public float startMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {

        startMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector2(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y);

      
       
    }


}



