using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTimedStart : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public Transform block;
    public bool startMoving;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    private int i;
    void Start()
    {
        boxCollider = block.GetComponent<BoxCollider2D>();
        spriteRenderer = block.GetComponent<SpriteRenderer>();
        block.transform.position = points[startingPoint].position;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            startMoving = true;
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
        }    
    }

    void Update()
    {
        if (Vector2.Distance(block.transform.position, points[i].position)<0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i=0;
            }
        }
        if(startMoving == true)
        {
            block.transform.position = Vector2.MoveTowards(block.transform.position, points[i].position, speed * Time.deltaTime);
        }
    }
}
