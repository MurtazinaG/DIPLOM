using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayByAngle : MonoBehaviour
{

    public float angle;
    public float distance = 1f;

    // Update is called once per frame
    void Update()
    {
        ShootRay();
    }

    void ShootRay()
    {
        var direction = Quaternion.AngleAxis(angle, transform.right) * transform.forward;

        Ray ray = new Ray(transform.position, direction);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);

        if (Physics.Raycast(ray, distance))
        {
            Debug.Log("Something is there");
        }

    }
}