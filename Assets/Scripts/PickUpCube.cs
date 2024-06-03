using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCube : MonoBehaviour
{
    
    GameObject TOTcube;
    bool canPickUp;
    public float angle;
    public float line = 1.5f;
    public GameObject Bag;
  



    public GameObject LeverWork;
    public GameObject DialoguePanel;
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;

   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) PickUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
       
    }

    

    void PickUp()
    {
        RaycastHit hit;
        Vector3 direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);
        
        Physics.Raycast(ray, out hit);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * line, Color.yellow);

       

        if (Physics.Raycast(transform.position, direction, line))//чтобы подобрать куб
        { 
            if (hit.transform.tag == "TOTcube")
            {
                TOTcube = hit.transform.gameObject;
                TOTcube.SetActive(false);
                Bag.SetActive(true);
                TOTcube.transform.parent = transform;
                TOTcube.transform.localPosition = Vector3.zero;
                canPickUp = true;
            }
        }
        if (Physics.Raycast(transform.position, direction, line))// для предметов
        { 
            if (hit.transform.tag == "Lever")
            {
                animator1.SetBool("OnOff", true);
                LeverWork.SetActive(false);
                animator2.SetBool("Stop", true);
            }
        }
        if (Physics.Raycast(transform.position, direction, line))// для диалогового окна
        {
            if (hit.transform.tag == "DialoguePanel")
            {
                DialoguePanel.SetActive(true);
                
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


        DialoguePanel.SetActive(false);

        if (!Physics.Raycast(transform.position, direction, line))
        {
            // Добавляем смещение в зависимости от направления персонажа
            Vector3 offset = Vector3.zero;
            if (Mathf.Abs(characterDirection.x) > Mathf.Abs(characterDirection.z))
            {
                // Персонаж смотрит вбок (по оси X)
                offset.x = characterDirection.x > 0 ? 1f : -1f;
            }
            else
            {
                // Персонаж смотрит вперед (по оси Z)
                offset.z = characterDirection.z > 0 ? 1f : -1f;
            }

            Vector3 newPosition = TOTcube.transform.position + new Vector3(0f, -1f, 0f) + offset;
            TOTcube.transform.position = newPosition;
            TOTcube.transform.parent = null;
            TOTcube.SetActive(true);
            Bag.SetActive(false);
            canPickUp = false;
            TOTcube = null;
        }

        if (Physics.Raycast(transform.position, direction, line))// для предметов
        {
            if (hit.transform.tag == "Lever")
            {
                animator1.SetBool("OnOff", false);
                LeverWork.SetActive(true);
                animator2.SetBool("Stop", false);


            }
        }

    }
}
