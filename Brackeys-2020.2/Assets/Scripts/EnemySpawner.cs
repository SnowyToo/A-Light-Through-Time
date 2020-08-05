using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawn positions
    [SerializeField]
    private Vector2 rangeX;
    [SerializeField]
    private Vector2 rangeY;

    [SerializeField]
    private GameObject alert;

    // Spawning
    private int nextEnemies;
    private Dictionary<EnemyType, int> currentEnemies;
    [SerializeField]
    private float timeBetweenSpawns;
    private float timeLeftBetweenSpawns;

    // Max enemies
    [SerializeField]
    private int initialMaxEnemies = 1;
    private int maxEnemies;
    [SerializeField]
    private int maxEnemiesScoreIncrement = 250;
    [SerializeField]
    private int maxEnemiesCap = 20;
    private int[] maxEnemyTypes;

    // Enemies
    public GameObject[] enemyPrefabs;
    public static GameObject[] _enemyPrefabs;
    [HideInInspector]
    public enum EnemyType {SpikeEnemy, SnapEnemy, LaserEnemy};

    // Attribute probabilities
    [SerializeField]
    private List<AttributeProbability> attributeProbabilities;
    [SerializeField]
    private ShieldProbabilities shieldProbabilities;
    
    void Start()
    {
        maxEnemyTypes = new int[enemyPrefabs.Length];
        UpdateMaxEnemies();
        nextEnemies = maxEnemies;
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
        if (nextEnemies > 0)
        {
            if (timeLeftBetweenSpawns <= 0f)
                SpawnNextEnemy();
            else
                timeLeftBetweenSpawns -= Time.deltaTime;
        }
        
        // If we don't have enough enemies, add one to the next enemies
        if (CurrentEnemyCount() + nextEnemies < maxEnemies)
            nextEnemies ++;

        // Update maximum enemies on screen and per type
        UpdateMaxEnemies();
    }

    void UpdateMaxEnemies()
    {
        // Increases max enemies by one for every maxEnemiesScoreIncrement (250) score
        maxEnemies = Mathf.Clamp(initialMaxEnemies + GameManager.score / maxEnemiesScoreIncrement, initialMaxEnemies, maxEnemiesCap);

        maxEnemyTypes[(int) EnemyType.LaserEnemy] = Mathf.Clamp((int) Mathf.Floor(maxEnemies/4), 1, maxEnemies);
        maxEnemyTypes[(int) EnemyType.SnapEnemy] = Mathf.Clamp((int) Mathf.Ceil(maxEnemies/3), 1, maxEnemies);
        maxEnemyTypes[(int) EnemyType.SpikeEnemy] = 1000;
    }

    EnemySpawn PickNextEnemy()
    {
        EnemyType type = PickType();
        Vector3 position = PickPosition();
        List<EnemyAttribute> attributes = PickAttributes();

        return new EnemySpawn(type, position, attributes);
    }

    void SpawnNextEnemy()
    {
        timeLeftBetweenSpawns = timeBetweenSpawns;

        // Instantiate enemy
        EnemySpawn enemyToSpawn = PickNextEnemy();
        nextEnemies --;
        GameObject enemyGO = Instantiate(enemyToSpawn.enemyPrefab, enemyToSpawn.position, Quaternion.identity);
        Instantiate(alert, Vector3.zero, Quaternion.identity).GetComponent<Alert>().SetTarget(enemyGO.transform);
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

            // Secret :o
            if (Random.value < 0.002f)
            {
                children[0].Sadden();
                children[1].Sadden();
            }
        }

        // Update current enemies
        currentEnemies[enemyToSpawn.type] ++;
    }

    EnemyType PickType()
    {
        // Pick next enemy type based on current enemy types and score
        List<EnemyType> types = new List<EnemyType>();
        foreach(KeyValuePair<EnemyType, int> entry in currentEnemies)
        {
            int i = (int) entry.Key;
            if (entry.Value < maxEnemyTypes[i])
                types.Add(entry.Key);
        }

        int index = Random.Range(0, types.Count);
        return types[index];
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
        // Pick attributes based on current score
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
    public float firstProbability;
    public float probabilityChange;
    public int firstThreshold;
    public int thresholdIncrement;
    public float max;

    public float CalculateProbability(int score)
    {
        int threshold = (int) Mathf.Floor(score/thresholdIncrement);
        float probability;

        if (threshold >= firstThreshold)
            probability = firstProbability + (probabilityChange * (threshold - firstThreshold));
        else
            probability = 0f;
        
        if (probability > max) probability = max;
        return probability;
    }
}

[System.Serializable]
public struct ShieldProbabilities
{
    public float[] initialProbabilities;
    public float[] finalProbabilities;
    public float[] probabilityChanges;
    public float[] secondProbabilityChanges;
    public int[] thresholds;
    public int thresholdIncrement;

    public int CalculateNumber(int score)
    {
        float[][] changes = {probabilityChanges, secondProbabilityChanges};

        int threshold = (int) Mathf.Floor(score/thresholdIncrement);
        int lastThreshold = EnemySpawner.HighestThresholdIndex(threshold, thresholds);
        float[] probabilities = new float[probabilityChanges.Length];

        if (lastThreshold == -1)
            return CalculateShields(probabilities);
        else if (lastThreshold == thresholds.Length - 1)
            return CalculateShields(finalProbabilities);

        probabilities = initialProbabilities;

        for (int i = 0; i < lastThreshold; i ++)
        {
            for (int j = 0; j < probabilities.Length; j ++)
            {
                probabilities[j] += changes[i][j] * (threshold - lastThreshold);
            }
        }
        return CalculateShields(probabilities);
    }

    int CalculateShields(float[] probabilities)
    {
        float max = 0f;
        float r = Random.value;
        for (int i = 0; i < probabilities.Length; i ++)
        {
            max += probabilities[i];
            if (r < max) return i;
        }
        return 0;
    }
}