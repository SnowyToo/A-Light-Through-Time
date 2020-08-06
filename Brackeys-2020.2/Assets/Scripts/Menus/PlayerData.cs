using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static Stats stats;
    public static Options options;

    public static int maxHealth;

    public static bool playedAnimation;

    private void Start()
    {
        stats = SaveManager.Load<Stats>();

        if (SaveManager.Load<Options>().Equals(new Options()))
        {
            options = Options.def;
        }
        else
        {
            options = SaveManager.Load<Options>();
        }

        SaveManager.Save(stats);
        SaveManager.Save(options);
    }
}
