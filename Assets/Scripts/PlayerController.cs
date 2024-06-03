using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    [SerializeField]private Animator animator;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    Vector3 targetPos;
    float laneOffset = 1.0f;
    public float laneChangeSpeed = 6.0f;
    public float angle;
    public float lined = 1.5f;

    public static bool GameIsPaused = false;
    public GameObject WinMenuUI;

    private Vector3 targetPosition;
    public float smoothSpeed = 2.0f; // Скорость плавного перемещения

    void Start()
    {
        
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        startGameRotation = transform.rotation;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;



        //Анимация персонажа
        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isMoving", true);
           

        }
        else
        {
            animator.SetBool("isMoving", false);
            
        }

        if (direction.magnitude >= 0.1f)
        {
            
            if (horizontal < 0)
            {
                if (CanMoveInDirection(Vector3.left))
                {
                    //StartCoroutine(SwitchWithDelay());
                    controller.Move(Vector3.left * laneChangeSpeed * Time.deltaTime);
                }
            }
            else if (horizontal > 0)
            {
                if (CanMoveInDirection(Vector3.right))
                {
                    //StartCoroutine(SwitchWithDelay());
                    controller.Move(Vector3.right * laneChangeSpeed * Time.deltaTime);
                }
            }

            if (vertical > 0)
            {
                if (CanMoveInDirection(Vector3.forward))
                {
                    //StartCoroutine(SwitchWithDelay());
                    controller.Move(Vector3.forward * laneChangeSpeed * Time.deltaTime);
                }
            }
            else if (vertical < 0)
            {
                if (CanMoveInDirection(Vector3.back))
                {
                    //StartCoroutine(SwitchWithDelay());
                    controller.Move(Vector3.back * laneChangeSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            // Если персонаж не движется, переместим его в центр ближайшего объекта
            MoveToCenterOfCurrentObject();
        }
    }

    IEnumerator SwitchWithDelay()  //для паузы
    {
        yield return new WaitForSeconds(3f);

    }
        void MoveToCenterOfCurrentObject()
    {
        RaycastHit hit;
        // Отправляем луч вниз из текущей позиции персонажа
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Получаем коллайдер объекта, на котором стоит персонаж
            Collider collider = hit.collider;

            // Вычисляем целевую позицию как центр коллайдера плюс половина его размера по вертикали
            Vector3 center = collider.bounds.center;

            targetPosition = center + new Vector3(0f, 0.5f, 0f) + Vector3.up * collider.bounds.extents.y;

            // Запускаем корутину для плавного перемещения
            StartCoroutine(MoveToTarget());
            
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            // Плавно перемещаем персонажа к целевой позиции
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            yield return null;
        }
    }


    bool CanMoveInDirection(Vector3 direction)
    {

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1000);


        }
        SwitchWithDelay();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, lined-0.9f))
        {
            // Если впереди есть объект, возвращаем false
           
            if (hit.transform.tag == "Player")
            {
                animator.SetBool("Dance", true);
                WinMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
            }
            
            return false;
        }

        // Проверяем доступность пути под углом 45 градусов
        direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);

        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * lined, Color.red);

        if (Physics.Raycast(transform.position, direction, lined))
        {
            // Если по диагонали есть объект, возвращаем true
            
            return true;

        }
        
        return false;

    }
}