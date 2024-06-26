using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button level2B;
    public Button level3B;
    public Button level4B;
    int levelComplete;
    void Start()
    {
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        level2B.interactable = false;
        level3B.interactable = false; 
        level4B.interactable = false;

        switch (levelComplete)
        {
            case 1:
                level2B.interactable = true;
                break;
                case 2:
                level2B.interactable = true;
                level3B.interactable= true;
                break;
                case 3:
                level2B.interactable = true;
                level3B.interactable = true;
                level4B.interactable = true; 
                break;

        }
    }

    public void LoadTo(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Reset()
    {
        level2B.interactable = false;
        level3B.interactable = false;
        level4B.interactable = false;
        PlayerPrefs.SetInt("LevelComplete", 0);
        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
