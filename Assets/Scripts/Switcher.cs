using UnityEngine;

public class Switcher : MonoBehaviour
{
    public GameObject char1, char2;
    private Renderer[] characterRenderers;
    private MonoBehaviour[] char1Scripts, char2Scripts;
    int charOn = 1;

    void Start()
    {
        char1.gameObject.SetActive(true);
       
        // Получаем все скрипты на персонажах
        char1Scripts = char1.GetComponentsInChildren<MonoBehaviour>();
        char2Scripts = char2.GetComponentsInChildren<MonoBehaviour>();
        // Отключаем скрипты на char2
        foreach (var script in char2Scripts)
        {
            script.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            SwitchCharacter();
    }

    public void SwitchCharacter()
    {
        switch (charOn)
        {
            case 1:
                charOn = 2;

                
                char2.gameObject.SetActive(true);

                // Отключаем скрипты на char1
                foreach (var script in char1Scripts)
                {
                    script.enabled = false;
                }
                // Включаем скрипты на char2
                foreach (var script in char2Scripts)
                {
                    script.enabled = true;
                }

                break;

            case 2:
                charOn = 1;

                char1.gameObject.SetActive(true);
                

                // Отключаем скрипты на char2
                foreach (var script in char2Scripts)
                {
                    script.enabled = false;
                }
                // Включаем скрипты на char1
                foreach (var script in char1Scripts)
                {
                    script.enabled = true;
                }
                break;
        }
    }
}
