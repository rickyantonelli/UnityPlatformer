using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
private GameObject checkPoint;
[SerializeField] private GameObject startPoint;
private bool checkpointReached = false;
Rigidbody2D rb;

private void Start() 
{
    rb = GetComponent<Rigidbody2D>();    
}
private void OnTriggerEnter2D(Collider2D other) //when player collides with ball - no physics
    {
        if(other.gameObject.tag == "Death")
        { 
            returnToSpawn();
        }
        if(other.gameObject.tag == "CheckPoint")
        { 
            checkPoint = other.gameObject;
            checkpointReached = true;
        }



    }

    private void returnToSpawn()
    {
        if(checkpointReached == false)
        {
        transform.position = startPoint.transform.position;
        rb.velocity = new Vector2(0,0);
        }    
        else
        {
        transform.position = checkPoint.transform.position;
        rb.velocity = new Vector2(0,0);  
        }
    }
}
