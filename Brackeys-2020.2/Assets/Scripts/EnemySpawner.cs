﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawn positions
    [SerializeField]
    private Vector2 rangeX;
    [SerializeField]
    private Vector2 rangeY;

    private Queue<EnemySpawn> nextEnemies;
    private Dictionary<EnemyType, int> currentEnemies;
    [SerializeField]
    private float timeBetweenSpawns;
    private float timeLeftBetweenSpawns;

    [SerializeField]
    private int initialMaxEnemies = 1;
    private int maxEnemies;

    // Enemies
    public GameObject[] enemyPrefabs;
    public static GameObject[] _enemyPrefabs;
    [HideInInspector]
    public enum EnemyType {SpikeEnemy, SnapEnemy, LaserEnemy};

    // Probabilities
    [SerializeField]
    private EnemyAttribute[] allAttributes;
    [SerializeField]
    private List<AttributeProbability> attributeProbabilities;
    [SerializeField]
    private ShieldProbabilities shieldProbabilities;
    
    void Start()
    {
        maxEnemies = initialMaxEnemies;

        nextEnemies = new Queue<EnemySpawn>();
        currentEnemies = new Dictionary<EnemyType, int>();
        
        InitializeCurrentEnemies();

        timeLeftBetweenSpawns = timeBetweenSpawns;

        _enemyPrefabs = enemyPrefabs;
    }

    void InitializeCurrentEnemies()
    {
        for (int i = 0; i < enemyPrefabs.Length; i ++)
        {
            currentEnemies.Add((EnemyType) i, 0);
        }
    }

    int CurrentEnemyCount()
    {
        int count = 0;
        foreach(KeyValuePair<EnemyType, int> entry in currentEnemies)
        {
            count += entry.Value;
        }
        return count;
    }

    void Update()
    {        
        // If we aren't waiting and there are enemies left to spawn, spawn the next enemy
        if (nextEnemies.Count > 0)
        {
            if (timeLeftBetweenSpawns <= 0f)
                SpawnNextEnemy();
            else
                timeLeftBetweenSpawns -= Time.deltaTime;
        }
        
        // If we don't have enough enemies, pick one to spawn and enqueue it
        if (CurrentEnemyCount() + nextEnemies.Count < maxEnemies)
        {
            //Debug.Log("Creating new enemy spawn");
            EnemyType type = PickType();
            Vector3 position = PickPosition();
            List<EnemyAttribute> attributes = PickAttributes();

            EnemySpawn enemySpawn = new EnemySpawn(type, position, attributes);
            nextEnemies.Enqueue(enemySpawn);
        }
    }

    void SpawnNextEnemy()
    {
        //Debug.Log("Spawning new enemy");
        timeLeftBetweenSpawns = timeBetweenSpawns;

        // Instantiate enemy
        EnemySpawn enemyToSpawn = nextEnemies.Dequeue();
        GameObject enemyGO = Instantiate(enemyToSpawn.enemyPrefab, enemyToSpawn.position, Quaternion.identity);
        if (enemyToSpawn.type != EnemyType.LaserEnemy)
        {
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            
            // Addtributes to enemy
            foreach (EnemyAttribute attribute in enemyToSpawn.attributes)
            {
                enemy.Addtribute(attribute);
            }
        }
        else
        {
            LaserEnemy[] children = enemyGO.GetComponentsInChildren<LaserEnemy>();

            // Addtributes to both laser enemies
            foreach (EnemyAttribute attribute in enemyToSpawn.attributes)
            {
                children[0].Addtribute(attribute);
                children[1].Addtribute(attribute);
            }
        }

        // Update current enemies
        currentEnemies[enemyToSpawn.type] ++;
        //Debug.Log(currentEnemies[enemyToSpawn.type]);
    }

    EnemyType PickType()
    {
        // Pick next enemy type based on current enemy types and difficulty

        int index = Random.Range(0, enemyPrefabs.Length);
        EnemyType type = (EnemyType) index;
        return type;
    }

    Vector3 PickPosition()
    {
        int xSign = Random.Range(0, 2) * 2 - 1;
        int ySign = Random.Range(0, 2) * 2 - 1;
        float x = Random.Range(rangeX.x, rangeX.y) * xSign;
        float y = Random.Range(rangeY.x, rangeY.y) * ySign;
        return new Vector3(x, y, 0f);
    }

    List<EnemyAttribute> PickAttributes()
    {
        // Pick attributes based on difficulty
        //GenerateAttributeList();

        List<EnemyAttribute> attributes = new List<EnemyAttribute>();
        
        int shieldNumber = shieldProbabilities.CalculateNumber(GameManager.score);
        EnemyAttribute shieldAttribute = new EnemyAttribute(Enemy.AttributeType.SHIELD, shieldNumber);
        attributes.Add(shieldAttribute);

        foreach (AttributeProbability att in attributeProbabilities)
        {
            float probability = att.CalculateProbability(GameManager.score);
            if (Random.value <= probability)
                attributes.Add(new EnemyAttribute(att.type));
        }

        /*int shields = Random.Range(0, 2);
        EnemyAttribute shieldAttribute = new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD, shields);
        if (shields == 0 && Random.value > 0.75)
        {
            EnemyAttribute reflectAttribute = new EnemyAttribute(EnemyAttribute.AttributeType.REFLECT);
            attributes.Add(reflectAttribute);
        }

        if (Random.value > 0.75)
        {
            EnemyAttribute timeWarpAttribute = new EnemyAttribute(EnemyAttribute.AttributeType.TIME_ONLY);
            attributes.Add(timeWarpAttribute);
        }

        attributes.Add(shieldAttribute);*/

        return attributes;
    }

    public void RemoveEnemy(EnemyType type)
    {
        currentEnemies[type] --;
    }

    public static int HighestThresholdIndex(int score, int[] scoreThresholds)
    {
        for (int i = scoreThresholds.Length - 1; i >= 0; i --)
        {
            if (scoreThresholds[i] <= score) return i;
        }
        return -1;
    }
}

public struct EnemySpawn
{
    public GameObject enemyPrefab;
    public EnemySpawner.EnemyType type;
    public Vector3 position;
    public List<EnemyAttribute> attributes;

    public EnemySpawn(EnemySpawner.EnemyType _type, Vector3 _position, List<EnemyAttribute> _attributes)
    {
        enemyPrefab = EnemySpawner._enemyPrefabs[(int) _type];
        type = _type;
        position = _position;
        attributes = _attributes;
    }
}

[System.Serializable]
public struct AttributeProbability
{
    public Enemy.AttributeType type;
    public int[] scoreThresholds;
    public float[] probabilities;

    public float CalculateProbability(int score)
    {
        int index = EnemySpawner.HighestThresholdIndex(score, scoreThresholds);
        if (index == -1)
            return 0f;
        else
            return probabilities[index];
    }
}

[System.Serializable]
public struct ShieldProbabilities
{
    public int[] scoreThresholds;

    public int CalculateNumber(int score)
    {
        int index = EnemySpawner.HighestThresholdIndex(score, scoreThresholds);
        return index;
    }
}

[System.Serializable]
public struct ShieldNumbers
{
    public float noShields;
    public float oneShield;
    public float twoShields;
}