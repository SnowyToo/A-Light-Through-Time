using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    AudioClip beep;

    public TextMeshProUGUI hiScore;
    public TextMeshProUGUI hiScoreLabel;

    void Awake()
    {
        if(!PlayerData.playedAnimation)
        {
            PlayerData.playedAnimation = true;
            GetComponent<Animator>().SetTrigger("Intro");
        }

        

    }

    void Update()
    {
        hiScore.text = PlayerData.stats.hiScore.ToString();

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("SelectMode");
            AudioManager.ins.PlayPersistentSoundEffect(beep);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Options");
            AudioManager.ins.PlayPersistentSoundEffect(beep);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            AudioManager.ins.PlayPersistentSoundEffect(beep);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Controls");
            AudioManager.ins.PlayPersistentSoundEffect(beep);
        }
    }
}
