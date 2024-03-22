using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorOpenTile : MonoBehaviour
{
    private List<Transform> hitButton = new List<Transform>();
    public Transform roomDoor;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        boxCollider = roomDoor.GetComponent<BoxCollider2D>();
        spriteRenderer = roomDoor.GetComponent<SpriteRenderer>();       
    }

    void Update()
    {
        if(hitButton.Count == 2)
        {
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
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
}
