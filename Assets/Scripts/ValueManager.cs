using UnityEngine;
using System;

public class ValueManager : MonoBehaviour
{
    public static ValueManager Instance;

    private int numOn;
    public event Action<int> OnNumOnChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int NumOn
    {
        get { return numOn; }
        set
        {
            if (numOn != value)
            {
                numOn = value;
                OnNumOnChanged?.Invoke(numOn);
            }
        }
    }
}
