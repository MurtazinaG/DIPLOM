using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    public int positionOfPatrol;
    public Transform point;
    bool moveingRight;
    public float stoppingDistance;
    bool chill = false;
    bool angry = false;
    bool goBack = false;

    
    //Transform player;
    public GameObject player;

    void Start()
    {
        

        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, point.position) < positionOfPatrol && angry == false);
        {
            chill = true;
        }
        if(Vector3.Distance(transform.position, player.transform.position) < stoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false ;
        }
        if (Vector3.Distance(transform.position,player.transform.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
        }
        if (chill == true)
        {
            Chill();
        }
        else if (angry == true)
        {
            Angry();
        }
        else if (goBack == true) 
        { 
            GoBack();
        }
    }

    void Chill()
    {
        if(transform.position.x > point.position.x + positionOfPatrol)
        {
            moveingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            moveingRight=true;
        }

        if(moveingRight)
        {
            transform.position = new Vector3(transform.position.x + speed*Time.deltaTime, 0.5029546f, 4.014f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - speed*Time.deltaTime, 0.5029546f, 4.014f);
        }
    }

    void Angry()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position,speed*Time.deltaTime);
    }

    void GoBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }
}
