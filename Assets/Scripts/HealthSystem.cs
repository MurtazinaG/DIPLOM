using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int numberOfLives;
    public Image[] lives;
    public Sprite fullLive;
    public Sprite emptyLive;
    public GameObject loseMenuUI; // —сылка на окно проигрыша

    void Start()
    {
        UpdateLivesUI();
    }

    void Update()
    {
        if (health > numberOfLives)
        {
            health = numberOfLives;
        }
        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < health)
            {
                lives[i].sprite = fullLive;
            }
            else
            {
                lives[i].sprite = emptyLive;
            }

            lives[i].enabled = i < numberOfLives;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player takes damage: " + damage);
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        UpdateLivesUI();
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (loseMenuUI != null)
        {
            loseMenuUI.SetActive(true);
        }
        Time.timeScale = 0f; // ќстанавливаем игру
    }
}
