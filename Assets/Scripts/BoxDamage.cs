using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damage = 10; // Количество урона, наносимого ловушкой

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TrapDamage: OnTriggerEnter with " + other.name);

        // Проверяем, есть ли у объекта тег "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("TrapDamage: Player detected");

            // Получаем компонент Player у объекта
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("TrapDamage: Player script found");
                if (player.healthSystem != null)
                {
                    Debug.Log("TrapDamage: HealthSystem found, dealing damage");
                    // Наносим урон через HealthSystem
                    player.healthSystem.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("TrapDamage: HealthSystem is null on player " + other.name);
                }
            }
            else
            {
                Debug.LogError("TrapDamage: Player script not found on " + other.name);
            }
        }
        else
        {
            Debug.Log("TrapDamage: Object is not tagged as Player, it's tagged as " + other.tag);
        }
    }
}
