using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("Game");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Options");
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
