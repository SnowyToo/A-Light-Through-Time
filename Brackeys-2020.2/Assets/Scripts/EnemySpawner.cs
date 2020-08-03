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

    private Queue<EnemySpawn> nextEnemies;
    private Dictionary<EnemyType, int> currentEnemies;
    [SerializeField]
    private float timeBetweenSpawns;
    private float timeLeftBetweenSpawns;
    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int initialMaxEnemies = 1;
    private int maxEnemies;

    // Enemies
    [HideInInspector]
    public enum EnemyType {SpikeEnemy, SnapEnemy, LaserEnemy};
    [SerializeField]
    private GameObject[] enemyPrefabs;
    
    private Dictionary<EnemyType, GameObject> enemies;

    void Start()
    {
        maxEnemies = initialMaxEnemies;

        nextEnemies = new Queue<EnemySpawn>();
        currentEnemies = new Dictionary<EnemyType, int>();
        enemies = new Dictionary<EnemyType, GameObject>();
        
        InitializeCurrentEnemies();

        timeLeftBetweenSpawns = timeBetweenSpawns;
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
        foreach(KeyValuePair<EnemyType, int> entry in myDictionary)
        {
            count += entry.Value;
        }
        return count;
    }

    void Update()
    {
        if (nextEnemies.Count > 0)
        {
            if (timeLeftBetweenSpawns <= 0f)
                SpawnNextEnemy();
            else
                timeLeftBetweenSpawns -= Time.deltaTime;
            // If we aren't waiting, spawn the next enemy
        }
        
        if (CurrentEnemyCount() < maxEnemies)
        {
            // Pick which enemy to spawn
            int index = Random.Range(0, enemyPrefabs.Length);
            EnemyType type = (EnemyType) index;
            // Pick position
            Vector3 position = SpawnPosition();
            // Pick attributes
            List<EnemyAttribute> attributes = PickAttributes();
            // Add to next enemies
            EnemySpawn enemySpawn = new EnemySpawn(type, position, attributes);
            nextEnemies.Enqueue(enemySpawn);
        }
    }

    void SpawnNextEnemy()
    {
        timeLeftBetweenSpawns = timeBetweenSpawns;

        EnemySpawn enemy = nextEnemies.Dequeue();
    }

    EnemyType PickType()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        EnemyType type = (EnemyType) index;
        return type;
    }

    Vector3 SpawnPosition()
    {
        int xSign = Random.Range(0, 2) * 2 - 1;
        int ySign = Random.Range(0, 2) * 2 - 1;
        float x = Random.Range(rangeX.x, rangeX.y) * xSign;
        float y = Random.Range(rangeY.x, rangeY.y) * ySign;
        return new Vector3(x, y, 0f);
    }

    List<EnemyAttribute> PickAttributes()
    {
        List<EnemyAttribute> attributes = new List<EnemyAttribute>();
        EnemyAttribute shieldAttribute = new EnemyAttribute(EnemyAttribute.AttributeType.SHIELD, Random.Range(0, 3)); //  0, 1 or 2 shields
        attributes.Add(shieldAttribute);
        return attributes;
    }
}

public struct EnemySpawn
{
    public GameObject enemyPrefab;
    public Vector3 position;
    public List<EnemyAttribute> attributes;

    public EnemySpawn(EnemyType type, Vector3 _position, List<EnemyAttribute> _attributes)
    {
        enemyPrefab = enemies[type];
        position = _position;
        attributes = _attributes;
    }
}