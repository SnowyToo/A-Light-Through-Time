using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Options
{
    public float cameraShake;

    public bool useFullscreen;

    public float masterVolume;
    public float musicVolume;
    public float effectVolume;

    public static Options def = new Options(1, false, 1 ,1, 1);

    public Options(float camShake, bool fullScreen, float master, float volume, float fxVolume)
    {
        cameraShake = camShake;
        useFullscreen = fullScreen;
        musicVolume = volume;
        masterVolume = master;
        effectVolume = fxVolume;
    }

    public bool Equals(Options o)
    {
        return (o.cameraShake == cameraShake && o.useFullscreen == useFullscreen && o.musicVolume == musicVolume && o.masterVolume == masterVolume && o.effectVolume == effectVolume);
    }

    public string Print()
    {
        return $"{cameraShake} camShake, {useFullscreen} fullscreen, {masterVolume} masterVolume, {musicVolume} musicVolume, {effectVolume} effectVolume";
    }

    public void Save()
    {
        SaveManager.Save(this);
    }

}
