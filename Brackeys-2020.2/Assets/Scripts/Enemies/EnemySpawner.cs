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
    public enum EnemyType {SpikeEnemy, SnapEnemy, LaserEnemy, BounceEnemy};

    // Attribute probabilities
    [SerializeField]
    private List<AttributeProbabilities> attributeProbabilities;
    [SerializeField]
    private List<AmountProbabilityPair> attributeAmounts;
    
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
        maxEnemyTypes[(int) EnemyType.BounceEnemy] = 1000;
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

        //Get attribute amount. Zero is the base value.
        int maxAttributes = 0;

        //Loop through all values from lowest to highest.
        foreach(AmountProbabilityPair a in attributeAmounts)
        {
            if(Random.value <= a.probability.CalculateProbability(GameManager.score))
            {
                maxAttributes = a.amount;
                break;
            }
        }

        int curAttributes = 0;

        int curAmount = 0;

        //Loop through all attributes
        foreach(AttributeProbabilities chosenAttribute in ShuffleList(attributeProbabilities))
        {
            if (curAttributes == maxAttributes)
                break;
            
            //Per attribute, loops through all possible amounts, starting from the highest one.
            for (int i = chosenAttribute.probabilityPerAmount.Count-1; i >= 0; i--)
            {
                if (Random.value <= chosenAttribute.probabilityPerAmount[i].probability.CalculateProbability(GameManager.score))
                {
                    //Ensure only 3 layers of shielding
                    int am = chosenAttribute.probabilityPerAmount[i].amount;
                    while (curAmount + am > 3)
                        am--;
                    if (am < 0)
                        break;

                    curAmount += am;

                    //If it succeeds, it skips all lower tiers and moves onto the next one.
                    //attributes.Add(new EnemyAttribute(chosenAttribute.type, am));
                    curAttributes++;
                    break;
                }
            }
        }

        //Note that the first step takes a bottom-up approach (lowest --> heighest), while the second takes a top-down approach (heighest --> lowest)
        //In general this means enemies will have less attributes, but harder ones.

        return attributes;
    }

    List<AttributeProbabilities> ShuffleList(List<AttributeProbabilities> shuffle)
    {
        List<AttributeProbabilities> copyList = new List<AttributeProbabilities>();

        foreach(AttributeProbabilities a in shuffle)
        {
            copyList.Add(a);
        }

        int iterations = shuffle.Count;

        for(int i = 0; i < iterations; i++)
        {
            shuffle[i] = copyList[Random.Range(0, copyList.Count)];
            copyList.Remove(shuffle[i]);
        }

        return shuffle;
    }

    public void RemoveEnemy(EnemyType type)
    {
        currentEnemies[type] --;
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
public struct AttributeProbabilities
{
    public Enemy.AttributeType type;
    public List<AmountProbabilityPair> probabilityPerAmount;
}

[System.Serializable]
public struct AmountProbabilityPair
{
    public int amount;
    public SpawnProbability probability;
}

[System.Serializable]
public struct SpawnProbability
{
    public float initialChance;
    public ChancePerScore chanceIncreasePerScore;
    public float maxChance;

    public int minimalScoreNeeded;

    public float CalculateProbability(int score)
    {
        int scoreInterval = chanceIncreasePerScore.scoreInterval;
        if (scoreInterval <= 0)
            scoreInterval = 1;

        if (minimalScoreNeeded > score)
            return -1;

        return Mathf.Clamp(initialChance + chanceIncreasePerScore.chanceIncrease * (score-minimalScoreNeeded)/scoreInterval, 0, maxChance);
    }
}

[System.Serializable]
public struct ChancePerScore
{
    public int scoreInterval;
    public float chanceIncrease;
}