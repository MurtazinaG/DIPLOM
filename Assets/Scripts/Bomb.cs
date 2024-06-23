using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1; // ���� �� �����
    public GameObject explosionEffect;
    public float explosionDelay = 3.0f; // �������� ����� �������

    private bool isActivated = false; // ���� ��� ��������� �����

    void Start()
    {
        // ����� ����� �������� ������ ��� ���������
    }

    public void ActivateBomb()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        // �������� ����� �������
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        // �������� ������� ������
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        // ��������� ����� ���� ������ � ������� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f); // ������ ������
        foreach (Collider nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Bomb hit enemy. Enemy health: " + enemy.health);
            }
        }
        // ����������� ������� ������ ����� �������� �����
        Destroy(explosion, 2f); // ������������ ������� ������
        // ����������� �����
        Destroy(gameObject);
    }
}
