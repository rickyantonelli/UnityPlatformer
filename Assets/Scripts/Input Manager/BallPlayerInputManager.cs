using UnityEngine;
using UnityEngine.InputSystem;

public class BallPlayerInputManager : PlayerInputManager
{
    public GameObject Ball;
    public GameObject ballSpawn;
    public GameObject ballPrefab;

    void Start()
        {
            GameObject Ball = Instantiate(ballPrefab.transform, ballSpawn.transform.position, ballPrefab.transform.rotation).gameObject;
            Debug.Log(playerCount);
        }          
}
