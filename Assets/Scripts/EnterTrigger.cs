using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{

    public float angle;
    public float line = 1.5f;
    public GameObject block;
    int charOn = 0;

    [SerializeField] private Animator animator_l;

    public GameObject target1;
    public GameObject target2;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) PickUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
        if (charOn == 1)
        {
            block.transform.position = Vector3.Lerp(block.transform.position, target1.transform.position, Time.deltaTime);

        }
        else if (charOn == 2)
        {
            block.transform.position = Vector3.Lerp(block.transform.position, target2.transform.position, Time.deltaTime);

        }

    }
    void PickUp()
    {
        RaycastHit hit;
        Vector3 direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);

        Physics.Raycast(ray, out hit);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * line, Color.yellow);


        if (Physics.Raycast(transform.position, direction, line))
        {
            if (hit.transform.tag == "Lever1")
            {
                animator_l.SetBool("OnOff", true);
                charOn = 1;

            }
        }


    }

    void Drop()
    {
        // Определяем направление персонажа
        Vector3 characterDirection = transform.forward;

        RaycastHit hit;
        Vector3 direction = Quaternion.AngleAxis(angle, transform.right) * characterDirection;

        Ray ray = new Ray(transform.position, direction);

        Physics.Raycast(ray, out hit);



        if (Physics.Raycast(transform.position, direction, line))// для предметов
        {
            if (hit.transform.tag == "Lever1")
            {
                animator_l.SetBool("OnOff", false);
                charOn = 2;


            }
        }

    }
}


