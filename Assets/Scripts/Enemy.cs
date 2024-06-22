using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float positionOfPatrol;
    public Transform point;
    public float stoppingDistance;
    public int damage = 1;
    public float attackRate = 1f;
    public int health = 3; // Количество здоровья врага

    private float nextAttackTime = 0f;
    private bool movingRight;
    private bool chill;
    private bool angry;
    private bool goBack;

    public GameObject[] players; // Массив игроков
    private HealthSystem sharedHealthSystem;

    void Start()
    {
        if (players.Length > 0)
        {
            sharedHealthSystem = players[0].GetComponent<Player>().healthSystem;
        }
    }

    void Update()
    {
        GameObject closestPlayer = GetClosestPlayer();
        float playerDistance = Vector3.Distance(transform.position, closestPlayer.transform.position);
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
            Angry(closestPlayer);
        }
        else if (goBack)
        {
            GoBack();
        }
    }

    void Chill()
    {
        Vector3 moveDirection;

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
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }

        transform.position += moveDirection * speed * Time.deltaTime;
        RotateTowards(moveDirection);
    }

    void Angry(GameObject targetPlayer)
    {
        Vector3 direction = targetPlayer.transform.position - transform.position;
        direction.y = 0; // Игнорируем ось Y для 2D движения
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);
        RotateTowards(direction);

        if (Time.time >= nextAttackTime && Vector3.Distance(transform.position, targetPlayer.transform.position) < stoppingDistance)
        {
            AttackPlayer();
        }
    }

    void GoBack()
    {
        Vector3 direction = point.position - transform.position;
        direction.y = 0; // Игнорируем ось Y для 2D движения
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
        RotateTowards(direction);
    }

    void AttackPlayer()
    {
        if (sharedHealthSystem != null)
        {
            Debug.Log("Shared health system found, applying damage");
            sharedHealthSystem.TakeDamage(damage);
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
    }

    GameObject GetClosestPlayer()
    {
        GameObject closestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // Поворачиваем только по осям X и Z, с поворотом на 90 градусов
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 90) * 90; // Принудительный поворот на 90 градусов
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage, current health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed");
        }
    }
}
