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
    public float smoothSpeed = 2.0f; // �������� �������� �����������

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



        //�������� ���������
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
            // ���� �������� �� ��������, ���������� ��� � ����� ���������� �������
            MoveToCenterOfCurrentObject();
        }
    }

    IEnumerator SwitchWithDelay()  //��� �����
    {
        yield return new WaitForSeconds(3f);

    }
        void MoveToCenterOfCurrentObject()
    {
        RaycastHit hit;
        // ���������� ��� ���� �� ������� ������� ���������
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // �������� ��������� �������, �� ������� ����� ��������
            Collider collider = hit.collider;

            // ��������� ������� ������� ��� ����� ���������� ���� �������� ��� ������� �� ���������
            Vector3 center = collider.bounds.center;

            targetPosition = center + new Vector3(0f, 0.5f, 0f) + Vector3.up * collider.bounds.extents.y;

            // ��������� �������� ��� �������� �����������
            StartCoroutine(MoveToTarget());
            
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            // ������ ���������� ��������� � ������� �������
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
            // ���� ������� ���� ������, ���������� false
           
            if (hit.transform.tag == "Player")
            {
                animator.SetBool("Dance", true);
                WinMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
            }
            
            return false;
        }

        // ��������� ����������� ���� ��� ����� 45 ��������
        direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);

        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * lined, Color.red);

        if (Physics.Raycast(transform.position, direction, lined))
        {
            // ���� �� ��������� ���� ������, ���������� true
            
            return true;

        }
        
        return false;

    }
}