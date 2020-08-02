using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<string, int> enemyScores;
    [HideInInspector]
    public static int score;

    void Start()
    {
        enemyScores = new Dictionary<string, int>();
        enemyScores.Add("SpikeEnemy", 10);
        enemyScores.Add("LaserEnemy", 20);
        enemyScores.Add("SnapEnemy", 30);

        score = 0;
    }

    public static void EnemyKill(string tag)
    {
        int scoreGained = GameManager.enemyScores[tag];
        // TODO: update score text UI
        GameManager.score += scoreGained;
    }
}
