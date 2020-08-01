using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Transform player;
    public static Transform cam;
    public static TimeManager timeManager;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        cam = GameObject.FindWithTag("MainCamera").transform;
        timeManager = GetComponent<TimeManager>();
    }

    public static IEnumerator ReloadLevel()
    {
        // Some sort of fade out/death animation
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
