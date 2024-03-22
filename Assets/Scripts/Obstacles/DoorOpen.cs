using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform door;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        boxCollider = door.GetComponent<BoxCollider2D>();
        spriteRenderer = door.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.tag == "Ball")
        {
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
            Debug.Log("collided");
        }
    }
}
