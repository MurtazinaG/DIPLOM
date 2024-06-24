using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthSystem healthSystem;

    void Start()
    {
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem is not assigned to the player");
        }
    }
}
