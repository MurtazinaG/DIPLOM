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
       
        // �������� ��� ������� �� ����������
        char1Scripts = char1.GetComponentsInChildren<MonoBehaviour>();
        char2Scripts = char2.GetComponentsInChildren<MonoBehaviour>();
        // ��������� ������� �� char2
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

                // ��������� ������� �� char1
                foreach (var script in char1Scripts)
                {
                    script.enabled = false;
                }
                // �������� ������� �� char2
                foreach (var script in char2Scripts)
                {
                    script.enabled = true;
                }

                break;

            case 2:
                charOn = 1;

                char1.gameObject.SetActive(true);
                

                // ��������� ������� �� char2
                foreach (var script in char2Scripts)
                {
                    script.enabled = false;
                }
                // �������� ������� �� char1
                foreach (var script in char1Scripts)
                {
                    script.enabled = true;
                }
                break;
        }
    }
}
