using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1; // Урон от бомбы
    public float explosionDelay = 2.0f; // Задержка взрыва в секундах
    public float explosionRadius = 5.0f; // Радиус взрыва
    public GameObject explosionEffect; // Префаб эффекта взрыва

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
        // Запускаем отсчет времени до взрыва
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        // Создаем эффект взрыва на позиции бомбы
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration); // Уничтожаем эффект взрыва после завершения
        }

        // Применяем урон ко всем коллайдерам в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Бомба попала во врага. Здоровье врага: " + enemy.health);
                }
            }
        }

        // Уничтожаем объект бомбы
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Предотвращаем немедленное уничтожение бомбы при столкновении
        if (!isThrown)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.bombCount++;
                    Debug.Log("Игрок подобрал бомбу. Всего бомб: " + playerController.bombCount);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Бомба столкнулась с: " + collision.gameObject.name);
            }
        }
    }
}
