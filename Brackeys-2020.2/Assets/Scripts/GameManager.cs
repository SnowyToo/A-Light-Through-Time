using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Score
    [SerializeField]
    private List<EnemyScorePair> _enemyScores;

    private static Dictionary<string, int> enemyScores;
    [HideInInspector]
    public static int score;

    private Volume cameraProfile;

    // Gameplay
    [HideInInspector]
    public static bool isRewinding;

    [HideInInspector]
    public static GameObject playerObject;
    [HideInInspector]
    public static GameObject photonObject;
    [HideInInspector]
    public static Player player;
    [HideInInspector]
    public static Photon photon;
    [SerializeField]
    private Collider2D mirrorCollider;

    [HideInInspector]
    public static bool gameIsOver;

    //Enemies
    [HideInInspector]
    public static EnemySpawner enemySpawner;

    // Visuals & Audio
    [SerializeField]
    private VolumeProfile normalPostProcess;
    [SerializeField]
    private VolumeProfile warpPostProcess;
    public static CameraShake camShake;

    private AudioSource bgm;
    private AudioLowPassFilter lowPass;
    private const float NORMAL_LOW_PASS = 7500;
    private const float TIME_WARP_LOW_PASS = 2500;

    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
        photonObject = GameObject.FindWithTag("Photon");
        player = playerObject.GetComponent<Player>();
        photon = photonObject.GetComponent<Photon>();
        enemySpawner = GetComponent<EnemySpawner>();

        camShake = Camera.main.GetComponent<CameraShake>();

        bgm = GetComponent<AudioSource>();
        lowPass = GetComponent<AudioLowPassFilter>();
        gameIsOver = false;
    }

    void Start()
    {
        enemyScores = new Dictionary<string, int>();
        foreach (EnemyScorePair esp in _enemyScores)
        {
            enemyScores.Add(esp.name, esp.value);
        }

        score = 0;

        cameraProfile = Camera.main.transform.GetComponent<Volume>();
    }

    void Update()
    {
        if (!gameIsOver)
        {
            if (Input.GetButton("Rewind") && !photon.captured)
            {
                isRewinding = true;
                cameraProfile.profile = warpPostProcess;
                mirrorCollider.enabled = false;

                bgm.pitch = -1;
                lowPass.cutoffFrequency = TIME_WARP_LOW_PASS;
            }
            else
            {
                isRewinding = false;
                cameraProfile.profile = normalPostProcess;
                mirrorCollider.enabled = true;

                bgm.pitch = 1;
                lowPass.cutoffFrequency = NORMAL_LOW_PASS;
            }
        }

        // For testing purposes
        if (Input.GetKeyDown("r")) SceneManager.LoadScene("Game");
    }

    public static void EnemyKill(string tag)
    {
        int scoreGained = GameManager.enemyScores[tag];
        // TODO: update score text UI
        score += scoreGained;
    }

    public static void CameraShake(float dur, float mag)
    {
        camShake.Shake(dur, mag);
    }

    public static void GameOver()
    {
        photon.Die();
        enemySpawner.enabled = false;
        gameIsOver = true;
        Debug.Log("GAME OVER");
    }

    public static Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static void PlaySound(AudioClip clip, GameObject source, float volume = 0.8f, float pitch = 1f)
    {
        AudioSource sound = new GameObject().AddComponent<AudioSource>();
        sound.volume = volume;
        sound.name = source.name + " Sound";
        sound.clip = clip;

        sound.pitch = pitch;

        AudioLowPassFilter lowPass = sound.gameObject.AddComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = (isRewinding ? TIME_WARP_LOW_PASS : NORMAL_LOW_PASS);

        sound.Play();
        Destroy(sound.gameObject, 4f);
    }

    public static void PlaySound(AudioClip[] clips, GameObject source)
    {
        PlaySound(clips[Random.Range(0, clips.Length)], source);
    }

    public static void SpawnParticles(GameObject particles, GameObject source)
    {
        GameObject part = Instantiate(particles, source.transform.position, Quaternion.identity);
        Destroy(part.gameObject, 4f);
    }
}

[System.Serializable]
public struct EnemyScorePair
{
    public string name;
    public int value;
}
