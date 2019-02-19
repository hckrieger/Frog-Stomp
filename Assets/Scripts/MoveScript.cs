using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform objectToMove;
    private Vector3 Target;
    

    // Start is called before the first frame update
    void Start()
    {
        Target = endPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, Target, step); 

        if (objectToMove.transform.position == endPoint.transform.position)
        {
            Target = startPoint.position; 

        } else if (objectToMove.transform.position == startPoint.transform.position)
        {
            Target = endPoint.position;
        }
    }
}
