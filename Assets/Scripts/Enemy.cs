using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int positionOfPatrol;
    public Transform point;
    bool movingRight;
    public float stoppingDistance;
    public int damage = 1;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    private bool chill = false;
    private bool angry = false;
    private bool goBack = false;

    public GameObject player;
    private HealthSystem playerHealth;

    void Start()
    {
        playerHealth = player.GetComponent<HealthSystem>();
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        float pointDistance = Vector3.Distance(transform.position, point.position);

        if (pointDistance < positionOfPatrol && !angry)
        {
            chill = true;
        }

        if (playerDistance < stoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false;
        }
        else if (playerDistance > stoppingDistance)
        {
            goBack = true;
            angry = false;
        }

        if (chill)
        {
            Chill();
        }
        else if (angry)
        {
            Angry();
        }
        else if (goBack)
        {
            GoBack();
        }
    }

    void Chill()
    {
        if (transform.position.x > point.position.x + positionOfPatrol)
        {
            movingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    void Angry()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (Time.time >= nextAttackTime && Vector3.Distance(transform.position, player.transform.position) < stoppingDistance)
        {
            playerHealth.TakeDamage(damage);
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void GoBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            playerHealth.TakeDamage(damage);
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
}
