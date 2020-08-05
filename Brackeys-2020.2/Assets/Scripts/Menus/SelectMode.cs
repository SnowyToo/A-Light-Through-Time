using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMode : MonoBehaviour
{
    public AudioClip beep;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            StartGame(3);
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            StartGame(1);
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            AudioManager.ins.PlayPersistentSoundEffect(beep);
        }
    }

    void StartGame(int h)
    {
        PlayerData.maxHealth = h;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        AudioManager.ins.PlayPersistentSoundEffect(beep);
    }
}
