using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private Animator animator;
    private Vector3 startGamePosition;
    private Quaternion startGameRotation;
    private Vector3 targetPos;
    private float laneOffset = 1.0f;
    public float laneChangeSpeed = 6.0f;
    public float angle;
    public float lined = 1.5f;

    public static bool GameIsPaused = false;
    public GameObject WinMenuUI;

    private Vector3 targetPosition;
    public float smoothSpeed = 2.0f;

    public int bombCount = 0; // Количество бомб у игрока
    public GameObject bombPrefab; // Префаб бомбы
    public Transform bombSpawnPoint; // Точка, откуда бомба будет выброшена
    public float throwForce = 10f; // Сила броска
    public float throwAngle = 45f; // Угол броска

    void Start()
    {
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
    }

    void Update()
    {
        startGameRotation = transform.rotation;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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
                    controller.Move(Vector3.left * laneChangeSpeed * Time.deltaTime);
                }
            }
            else if (horizontal > 0)
            {
                if (CanMoveInDirection(Vector3.right))
                {
                    controller.Move(Vector3.right * laneChangeSpeed * Time.deltaTime);
                }
            }

            if (vertical > 0)
            {
                if (CanMoveInDirection(Vector3.forward))
                {
                    controller.Move(Vector3.forward * laneChangeSpeed * Time.deltaTime);
                }
            }
            else if (vertical < 0)
            {
                if (CanMoveInDirection(Vector3.back))
                {
                    controller.Move(Vector3.back * laneChangeSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            MoveToCenterOfCurrentObject();
        }

        if (Input.GetKeyDown(KeyCode.Space) && bombCount > 0)
        {
            ThrowBomb();
        }
    }

    IEnumerator SwitchWithDelay()
    {
        yield return new WaitForSeconds(3f);
    }

    void MoveToCenterOfCurrentObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Collider collider = hit.collider;
            Vector3 center = collider.bounds.center;
            targetPosition = center + new Vector3(0f, 0.5f, 0f) + Vector3.up * collider.bounds.extents.y;
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
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
        if (Physics.Raycast(transform.position, direction, out hit, lined - 0.9f))
        {
            if (hit.transform.tag == "Player")
            {
                animator.SetBool("Dance", true);
                WinMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
            }
            return false;
        }

        direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(transform.position, direction, lined))
        {
            return true;
        }

        return false;
    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Рассчитываем направление броска, учитывая ориентацию персонажа
            Vector3 throwDirection = CalculateThrowDirection();
            rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
        }
        bombCount--;

        // Запускаем логику взрыва бомбы
        Bomb bombComponent = bomb.GetComponent<Bomb>();
        if (bombComponent != null)
        {
            bombComponent.ThrowBomb();
        }
    }

    Vector3 CalculateThrowDirection()
    {
        // Переводим угол в радианы
        float angleInRadians = throwAngle * Mathf.Deg2Rad;
        // Вычисляем компоненты направления
        float y = Mathf.Sin(angleInRadians);
        float z = Mathf.Cos(angleInRadians);
        // Получаем направление, куда смотрит персонаж
        Vector3 forward = transform.forward;
        // Создаем вектор направления броска, учитывая угол
        Vector3 throwDirection = forward * z + Vector3.up * y;
        return throwDirection.normalized;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BombPickup"))
        {
            bombCount++;
            Debug.Log("Подобрана бомба. Текущее количество бомб: " + bombCount);
            Destroy(other.gameObject);
        }
    }
}
