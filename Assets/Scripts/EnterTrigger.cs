using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    bool canPickUp;
    public float angle;
    public float line = 1.5f;
    public GameObject Bag;

    public Transform startPoint; // ��������� �����
    public Transform endPoint;   // �������� �����
    public float speed = 1.0f;

    [SerializeField] private Animator animator_l;

    private void Start()
    {
        // ������������� �� ������� ��������� NumOn
        ValueManager.Instance.OnNumOnChanged += OnNumOnChanged;
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��������� NumOn
        ValueManager.Instance.OnNumOnChanged -= OnNumOnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) PickUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();

        float step = speed * Time.deltaTime; // ���������, ����� ���������� ������ �� ���� ����

        int numOn = ValueManager.Instance.NumOn;

        if (numOn == 1)
        {
            Bag.transform.position = Vector3.MoveTowards(Bag.transform.position, endPoint.position, step);
        }
        else if (numOn == 2)
        {
            Bag.transform.position = Vector3.MoveTowards(Bag.transform.position, startPoint.position, step);
        }
    }

    void PickUp()
    {
        RaycastHit hit;
        Vector3 direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);

        Physics.Raycast(ray, out hit);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * line, Color.yellow);

        if (Physics.Raycast(transform.position, direction, line)) // ��� ���������
        {
            print(hit);
            if (hit.transform.tag == "Lever1")
            {
                animator_l.SetBool("OnOff", true);

                // ������������� �������� NumOn ����� ValueManager
                ValueManager.Instance.NumOn = 1;
            }
        }
    }

    void Drop()
    {
        // ���������� ����������� ���������
        Vector3 characterDirection = transform.forward;

        RaycastHit hit;
        Vector3 direction = Quaternion.AngleAxis(angle, transform.right) * characterDirection;

        Ray ray = new Ray(transform.position, direction);

        Physics.Raycast(ray, out hit);

        if (Physics.Raycast(transform.position, direction, line)) // ��� ���������
        {
            if (hit.transform.tag == "Lever1")
            {
                animator_l.SetBool("OnOff", false);

                // ������������� �������� NumOn ����� ValueManager
                ValueManager.Instance.NumOn = 2;
            }
        }
    }

    private void OnNumOnChanged(int newNumOn)
    {
        // �������� ��� ��������� �������� NumOn (���� ����������)
        // ��������, ������ ����������� ��������� �� ���������
    }
}
