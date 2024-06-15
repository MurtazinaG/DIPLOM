using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int numberOfLives;

    public Image[] lives;

    public Sprite fullLive;
    public Sprite empthyLive;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health>numberOfLives)
        {
            health = numberOfLives;
        }
        for(int i = 0; i < lives.Length; i++)
        {
            if(i < health)
            {
                lives[i].sprite = fullLive;
            }
            else
            {
                lives[i].sprite = empthyLive;
            }
            if(i<numberOfLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }
}
