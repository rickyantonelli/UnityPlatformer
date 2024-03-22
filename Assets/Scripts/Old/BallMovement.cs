using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform ballContainer, ball;
    public GameObject player;

    public float pickUpRange;
 
    public bool equipped;
    public bool slotFull;
    

    void Start() 
    {
        equipped = false;
    }

    void Update()
    {
        //Vector3 distanceToPlayer = player.position - transform.position;
        //if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull && player.gameObject.tag == "Team1Player") PickUp();

    }

    private void PickUp()
    {
        equipped = true;

        transform.SetParent(ballContainer);
        transform.localPosition = Vector3.zero;
    }
}

