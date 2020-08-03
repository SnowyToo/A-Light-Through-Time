using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawn positions

    private Queue<GameObject> nextEnemies;
    private Dictionary<EnemySpawn, int> currentEnemies;
    [SerializeField]
    private float timeBetweenSpawns;
    private float timeLeftBetweenSpawns;
    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int initialMaxEnemies = 1;
    private int maxEnemies;

    void Start()
    {
        maxEnemies = initialMaxEnemies;

        nextEnemies = new Queue<GameObject>();
        currentEnemies = new Dictionary<EnemySpawn, int>();

        timeLeftBetweenSpawns = timeBetweenSpawns;
    }

    void Update()
    {
        if (nextEnemies.Count > 0)
        {
            if (timeLeftBetweenSpawns <= 0f)
            {
                SpawnNextEnemy();
                timeLeftBetweenSpawns = timeBetweenSpawns;
            }
            // If we aren't waiting, spawn the next enemy
        }
        
        if (currentEnemies.Count < maxEnemies)
        {
            // Pick which enemy to spawn
            // Pick position
            // Add to next enemies
        }
    }

    void SpawnNextEnemy()
    {

    }
}

[System.Serializable]
public struct EnemySpawn
{

}