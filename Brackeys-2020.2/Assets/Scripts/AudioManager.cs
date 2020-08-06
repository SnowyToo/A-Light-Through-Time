using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager ins;

    [SerializeField]
    private AudioClip normMusic;
    [SerializeField]
    private AudioClip reverseMusic;

    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private AudioSource soundEffect;
    [SerializeField]
    private AudioLowPassFilter lowPass;

    private const float NORMAL_LOW_PASS = 7500;
    private const float TIME_WARP_LOW_PASS = 2500;

    private IEnumerator MusicManager()
    {
        bgm.clip = reverseMusic;
        bgm.pitch = 0.85f;
        bgm.Play();
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
        {
            yield return null;
        }
        bgm.Stop();
        bgm.clip = normMusic;
        bgm.pitch = 1f;
        yield return new WaitForSeconds(4f);
        bgm.Play();
        while(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            yield return null;
        }
        StartCoroutine(MusicManager());
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (ins == null)
            ins = this;
        else if (ins != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        lowPass = GetComponent<AudioLowPassFilter>();

        StartCoroutine(MusicManager());
    }

    public void Update()
    {
        bgm.volume = 0.8f * PlayerData.options.masterVolume;
    }

    public void SetTimeWarpMusic()
    {
        bgm.pitch = -1;
        lowPass.cutoffFrequency = TIME_WARP_LOW_PASS;
    }

    public void SetNormalMusic()
    {
        bgm.pitch = 1;
        lowPass.cutoffFrequency = NORMAL_LOW_PASS;
    }

    public void PlaySound(AudioClip clip, GameObject source, float volume = 0.8f, float pitch = 1f)
    {
        AudioSource sound = new GameObject().AddComponent<AudioSource>();
        sound.volume = volume * PlayerData.options.masterVolume;
        sound.name = source.name + " Sound";
        sound.clip = clip;

        sound.pitch = pitch;

        AudioLowPassFilter lowPass = sound.gameObject.AddComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = (GameManager.isRewinding ? TIME_WARP_LOW_PASS : NORMAL_LOW_PASS);

        sound.Play();
        Destroy(sound.gameObject, 4f);
    }

    public void PlayPersistentSoundEffect(AudioClip sound)
    {
        soundEffect.clip = sound;
        soundEffect.Play();
    }
}
