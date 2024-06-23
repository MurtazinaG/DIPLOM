using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1; // Урон от бомбы
    public GameObject explosionEffect;
    public float explosionDelay = 3.0f; // Задержка перед взрывом

    private bool isActivated = false; // Флаг для активации бомбы

    void Start()
    {
        // Бомба будет взорвана только при активации
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
        // Задержка перед взрывом
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        // Создание эффекта взрыва
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        // Нанесение урона всем врагам в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f); // Радиус взрыва
        foreach (Collider nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Bomb hit enemy. Enemy health: " + enemy.health);
            }
        }
        // Уничтожение эффекта взрыва через короткое время
        Destroy(explosion, 2f); // Длительность эффекта взрыва
        // Уничтожение бомбы
        Destroy(gameObject);
    }
}
