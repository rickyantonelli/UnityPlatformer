using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformIndicator : MonoBehaviour
{
    public bool startMoving;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            startMoving = true;
        }    
    }
}
