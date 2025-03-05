using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int zombieCount;
        public float spawnRate;
        public int fastZombieCount;
        public int armoredZombieCount;
    }
    
    [Header("Spawn Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject fastZombiePrefab;
    [SerializeField] private GameObject armoredZombiePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;
    
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    
    public delegate void WaveChanged(int currentWave, int totalWaves);
    public static event WaveChanged OnWaveChanged;
    
    public delegate void GameWon();
    public static event GameWon OnGameWon;
    
    private void Start()
    {
        Enemy.OnEnemyDeath += OnEnemyKilled;
        StartCoroutine(StartNextWave());
    }
    
    private void OnDestroy()
    {
        Enemy.OnEnemyDeath -= OnEnemyKilled;
    }
    
    private void OnEnemyKilled(int points)
    {
        enemiesAlive--;
        
        if (enemiesAlive <= 0 && !isSpawning)
        {
            if (currentWave >= waves.Length)
            {
                // Game won
                if (OnGameWon != null)
                    OnGameWon();
            }
            else
            {
                StartCoroutine(StartNextWave());
            }
        }
    }
    
    private IEnumerator StartNextWave()
    {
        isSpawning = true;
        
        yield return new WaitForSeconds(timeBetweenWaves);
        
        StartCoroutine(SpawnWave());
        
        if (OnWaveChanged != null)
            OnWaveChanged(currentWave + 1, waves.Length);
    }
    
    private IEnumerator SpawnWave()
    {
        Wave wave = waves[currentWave];
        
        // Spawn regular zombies
        for (int i = 0; i < wave.zombieCount; i++)
        {
            SpawnZombie(zombiePrefab);
            enemiesAlive++;
            yield return new WaitForSeconds(wave.spawnRate);
        }
        
        // Spawn fast zombies
        for (int i = 0; i < wave.fastZombieCount; i++)
        {
            SpawnZombie(fastZombiePrefab);
            enemiesAlive++;
            yield return new WaitForSeconds(wave.spawnRate);
        }
        
        // Spawn armored zombies
        for (int i = 0; i < wave.armoredZombieCount; i++)
        {
            SpawnZombie(armoredZombiePrefab);
            enemiesAlive++;
            yield return new WaitForSeconds(wave.spawnRate);
        }
        
        currentWave++;
        isSpawning = false;
    }
    
    private void SpawnZombie(GameObject zombiePrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
    }
} 