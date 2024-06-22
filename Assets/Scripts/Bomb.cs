using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1; // ���� �� �����

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����� ��������� PlayerController � ��������� ���������� ����
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.bombCount++;
                Debug.Log("Player picked up a bomb. Total bombs: " + playerController.bombCount);
            }
            // ���������� �����
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // ����� ��������� Enemy � ������� ����
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Bomb hit enemy. Enemy health: " + enemy.health);
            }
            // ���������� �����
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Bomb collided with: " + collision.gameObject.name);
        }
    }
}
