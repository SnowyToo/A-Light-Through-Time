using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int deaths;

    public int totalScore;
    public int hiScore;

    public int snappersKilled;
    public int lasersKilled;
    public int spikesKilled;
    public int shieldDestroyed;

    public int timesReflects;

    public static Stats zero = new Stats(0, 0, 0, 0, 0, 0, 0, 0);

    public Stats(int d, int tot, int hi, int snaps, int lasers, int spikes, int shield, int reflect)
    {
        deaths = d;
        totalScore = tot;
        hiScore = hi;
        snappersKilled = snaps;
        lasersKilled = lasers;
        spikesKilled = spikes;
        shieldDestroyed = shield;
        timesReflects = reflect;
    }

    public string Print()
    {
        return $"{deaths} deaths, {totalScore} score, {hiScore} highScore, {snappersKilled} snappers, {lasersKilled} lasers, {spikesKilled} spikes, {shieldDestroyed} shileds, {timesReflects} reflect";
    }
}
