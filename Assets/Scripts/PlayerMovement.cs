using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    Vector3 targetPos;
    float laneOffset = 1.0f;
    float laneChangeSpeed = 15;

    public float maxDistance = 1f; // ћаксимальное рассто€ние дл€ Raycast
    public LayerMask obstacleLayer; // —лои, которые будут рассматриватьс€ как преп€тстви€
    public LayerMask roadLayer; // —лои, которые будут рассматриватьс€ как дорога

    void Start()
    {
        animator = GetComponent<Animator>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayDirection = Quaternion.Euler(45, 0, 0) * Vector3.left;
        Debug.DrawRay(transform.position, rayDirection * maxDistance, Color.green);
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left);
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.back);
        }
    }

    void Move(Vector3 direction)
    {
        RaycastHit hit;

        Vector3 rayDirection = Quaternion.Euler(45, 0, 0) * direction;
        Debug.DrawRay(transform.position, rayDirection * maxDistance, Color.green);
        // ¬ыполн€ем Raycast в направлении движени€, чтобы проверить наличие дороги
        if (!Physics.Raycast(transform.position, rayDirection, out hit, maxDistance, roadLayer))
        {
            Debug.Log("Hit smth");
            // ≈сли нет дороги, то ничего не делаем
            return;
        }

        // ѕроверка на наличие преп€тствий перед перемещением
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance, obstacleLayer))
        {

            // ≈сли Raycast не сталкиваетс€ с преп€тствием, то можно перемещать объект
            Debug.Log("wtf");
            if (direction == Vector3.forward || direction == Vector3.back)
            {
                targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + laneOffset * Mathf.Sign(direction.z));
            }
            else if (direction == Vector3.left || direction == Vector3.right)
            {
                targetPos = new Vector3(transform.position.x + laneOffset * Mathf.Sign(direction.x), transform.position.y, transform.position.z);
            }
        }

        // ѕроизводим перемещение к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, targetPos, laneChangeSpeed * Time.deltaTime);
    }
}
