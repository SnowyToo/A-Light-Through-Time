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
    private AudioClip webGLnormMusic;
    [SerializeField]
    private AudioClip webGLreverseMusic;

    [SerializeField]
    private AudioSource normSource;

    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private AudioSource soundEffect;
    [SerializeField]
    private AudioLowPassFilter lowPass;

    private const float NORMAL_LOW_PASS = 7500;
    private const float TIME_WARP_LOW_PASS = 2500;

    private bool hasReversed;
    private bool hasUnreversed;

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

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            bgm.clip = webGLnormMusic;

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

        if(Application.platform != RuntimePlatform.WebGLPlayer)
        {
            lowPass.enabled = false;
        }

        hasUnreversed = true;
        hasReversed = false;

        StartCoroutine(MusicManager());
    }

    public void Update()
    {
        bgm.volume = 0.8f * PlayerData.options.masterVolume;
    }

    public void SetTimeWarpMusic()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (!hasReversed)
            {
                float curTrackTime = bgm.time;
                float reverseTrackTime = webGLreverseMusic.length - curTrackTime;
                bgm.clip = webGLreverseMusic;
                bgm.Play();
                bgm.time = reverseTrackTime;
                hasReversed = true;
                hasUnreversed = false;
            }

        }
        else
        {
            bgm.pitch = -1;
            lowPass.cutoffFrequency = TIME_WARP_LOW_PASS;
        }
        
    }

    public void SetNormalMusic()
    {        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (!hasUnreversed)
            {
                float curTrackTime = bgm.time;
                float reverseTrackTime = webGLnormMusic.length - curTrackTime;
                bgm.clip = webGLnormMusic;
                bgm.Play();
                bgm.time = reverseTrackTime;
                hasReversed = false;
                hasUnreversed = true;
            }
        }
        else
        {
            bgm.pitch = 1;
        }
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
