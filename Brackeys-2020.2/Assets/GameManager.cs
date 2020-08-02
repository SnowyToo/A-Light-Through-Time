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

    public static void KillEnemy(GameObject enemy)
    {
        int scoreGained = GameManager.enemyScores[enemy.tag];
        // TODO: death particles maybe?
        Destroy(enemy);
        // TODO: update score text
        GameManager.score += scoreGained;
    }
}
