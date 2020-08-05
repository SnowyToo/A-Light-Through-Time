using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Sprite filledImage;
    public Sprite emptyImage;

    public List<Image> cameraShake;
    private int cameraIndex;

    public List<Image> volume;
    private int volumeIndex;

    public Image fullScreen;
    private bool screenBool;

    void Start()
    {
        screenBool = Screen.fullScreen;

        Debug.Log(PlayerData.options.masterVolume);

        volumeIndex = Mathf.RoundToInt(PlayerData.options.masterVolume * 7f) - 1;
        cameraIndex = Mathf.RoundToInt(PlayerData.options.cameraShake * 4f) - 1;

        SetSlider(cameraIndex, cameraShake);
        SetSlider(volumeIndex, volume);
        TickBox(screenBool, fullScreen);

    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CameraShake();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            FullScreen();
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            Volume();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void CameraShake()
    {
        cameraIndex++;
        if (cameraIndex >= cameraShake.Count)
            cameraIndex = -1;
        SetSlider(cameraIndex, cameraShake);

        PlayerData.options.cameraShake = (cameraIndex + 1f) / 4f;
        PlayerData.options.Save();

    }

    private void FullScreen()
    {
        screenBool = !screenBool;
        Screen.fullScreen = screenBool;
        PlayerData.options.useFullscreen = screenBool;
        PlayerData.options.Save();
    }

    private void TickBox(bool b, Image im)
    {
        if (b)
            im.sprite = filledImage;
        else
            im.sprite = emptyImage;
    }

    private void SetSlider(int index, List<Image> sliderImages)
    {
        for (int i = 0; i < sliderImages.Count; i++)
        {
            if (i <= index)
                sliderImages[i].sprite = filledImage;
            else
                sliderImages[i].sprite = emptyImage;
        }
    }

    private void Volume()
    {
        volumeIndex++;
        if (volumeIndex >= volume.Count)
            volumeIndex = -1;

        SetSlider(volumeIndex, volume);
        PlayerData.options.masterVolume = (volumeIndex + 1f) / 7f;
        PlayerData.options.Save();
    }
}
