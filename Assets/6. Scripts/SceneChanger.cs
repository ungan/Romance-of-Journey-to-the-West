using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public Button start, exit;

    void Update()
    {
        if (start != null)
            start.onClick.AddListener(PressStart);

        if (exit != null)
            exit.onClick.AddListener(PressExit);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressExit();
        }
    }

    public void PressStart()
    {
        SceneManager.LoadScene(1);
    }

    public void PressExit()
    {
        Application.Quit();
    }
}
