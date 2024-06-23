using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1; // ���� �� �����
    public float explosionDelay = 2.0f; // �������� ������ � ��������
    public float explosionRadius = 5.0f; // ������ ������
    public GameObject explosionEffect; // ������ ������� ������

    private bool isThrown = false;

    void Update()
    {
        if (isThrown && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    public void ThrowBomb()
    {
        isThrown = true;
        // ��������� ������ ������� �� ������
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        // ������� ������ ������ �� ������� �����
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration); // ���������� ������ ������ ����� ����������
        }

        // ��������� ���� �� ���� ����������� � ������� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("����� ������ �� �����. �������� �����: " + enemy.health);
                }
            }
        }

        // ���������� ������ �����
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // ������������� ����������� ����������� ����� ��� ������������
        if (!isThrown)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.bombCount++;
                    Debug.Log("����� �������� �����. ����� ����: " + playerController.bombCount);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("����� ����������� �: " + collision.gameObject.name);
            }
        }
    }
}
