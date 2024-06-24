using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damage = 10; // ���������� �����, ���������� ��������

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TrapDamage: OnTriggerEnter with " + other.name);

        // ���������, ���� �� � ������� ��� "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("TrapDamage: Player detected");

            // �������� ��������� Player � �������
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("TrapDamage: Player script found");
                if (player.healthSystem != null)
                {
                    Debug.Log("TrapDamage: HealthSystem found, dealing damage");
                    // ������� ���� ����� HealthSystem
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
