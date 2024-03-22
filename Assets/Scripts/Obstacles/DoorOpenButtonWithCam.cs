using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenButtonWithCam : MonoBehaviour
{
    private List<Transform> hitButton = new List<Transform>();
    public Transform roomDoor, oldRoomDoor;
    BoxCollider2D bcRoomDoor, bcOldRoomDoor;
    SpriteRenderer srRoomDoor, srOldRoomDoor;
    public GameObject virtualCam;
    void Start()
    {
        bcRoomDoor = roomDoor.GetComponent<BoxCollider2D>();
        srRoomDoor = roomDoor.GetComponent<SpriteRenderer>();
        bcOldRoomDoor = oldRoomDoor.GetComponent<BoxCollider2D>();
        srOldRoomDoor = oldRoomDoor.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if(hitButton.Count == 2)
        {
            bcRoomDoor.enabled = false;
            srRoomDoor.enabled = false;
            bcOldRoomDoor.enabled = true;
            srOldRoomDoor.enabled = true;
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //when player collides with ball - no physics
    {
        if(other.gameObject.tag == "Player")
        {
            if(!hitButton.Contains(other.transform))
            {
                hitButton.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) //when player collides with ball - no physics
    {
        if(other.gameObject.tag == "Player")
        {
            if(hitButton.Contains(other.transform))
            {
                hitButton.Remove(other.transform);
            }
        }
    }   
}
