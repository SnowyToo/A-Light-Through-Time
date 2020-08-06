using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMode : MonoBehaviour
{
    public AudioClip beep;

    public bool reallySelect = true;

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
        if (!reallySelect)
            return;

        PlayerData.maxHealth = h;
        PlayerData.playedAnimation = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        AudioManager.ins.PlayPersistentSoundEffect(beep);
    }
}
