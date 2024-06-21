using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform ballSpawn;
    [SerializeField] private float _velocity = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyUp(KeyCode.Space))
        {
            var ball = Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);
            ball.Init(_velocity);
        }*/
    }
}
