using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 20;
    [SerializeField] public float enemiesPerSecond = 0.3f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.7f;

    [Header("Events")]
    public static UnityEvent enemyKilled = new UnityEvent();
    public static EnemySpawner Main;

    public int currentWave = 1;
    private float timeSinceLastSpawn = 0f;
    public int enemiesAlive;
    public int enemiesLeftToSpawn;
    public int upcomingWave;
    private bool isSpawning = false;
    private bool isWaveInProgress = false;

    public int enemiesReachedPathPoint;
    public int enemiesReachedEndpoint;

    private int nextWaveEnemyCount = 0;

    private void Awake()
    {
        Main = this;
        enemyKilled.AddListener(OnEnemyKilled);
        AIWaveHandler.Main = FindFirstObjectByType<AIWaveHandler>();
        if (currentWave < 1)
            currentWave = 1;
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        // Stop spawning if player is dead or LevelManager is missing
        if (LevelManager.Main == null || LevelManager.Main.health <= 0)
        {
            isSpawning = false;
            GameOver.Main.ShowGameOver();
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        // Spawn enemies at the set rate
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            enemiesPerSecond = 1f / Random.Range(0.4f, 1.5f);
            timeSinceLastSpawn = 0f;
        }

        // End wave when all enemies are spawned and defeated
        if (enemiesLeftToSpawn <= 0 && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    private void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    // Starts a new wave after a delay
    private IEnumerator StartWave()
    {
        InGameBuyMenu.Main.OnGUI();
        if (isWaveInProgress)
            yield break;

        isWaveInProgress = true;

        // Use the stored value for this wave
        if (currentWave == 1)
            enemiesLeftToSpawn = baseEnemies;
        else
            enemiesLeftToSpawn = nextWaveEnemyCount;

        enemiesReachedPathPoint = 0;
        enemiesReachedEndpoint = 0;
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        Debug.Log($"Starting Wave {currentWave}: Enemies to Spawn = {enemiesLeftToSpawn}");
    }

    // Handles end-of-wave logic and checks for victory
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        isWaveInProgress = false;
        

        // If all waves are complete, player wins
        if (currentWave > 10)
        {
            GameOver.Main.WinGame();
            return;
        }

        // Only call AdjustWaveDifficulty here!
        int baseWaveEnemies = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
        nextWaveEnemyCount = AIWaveHandler.Main.AdjustWaveDifficulty(baseWaveEnemies);

        StartCoroutine(StartWave());
    }

    // Spawns an enemy based on the current scene and wave
    private void SpawnEnemy()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject prefabToSpawn = enemyPrefabs[0];

        if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            if (currentWave >= 7)
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            else if (currentWave >= 5)
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(3, enemyPrefabs.Length))];
            else if (currentWave >= 3)
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
        }
        else if (sceneName == "Level 4" || sceneName == "Level 5" || sceneName == "Level 6")
        {
            if (currentWave >= 6)
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(7, enemyPrefabs.Length))];
            else if (currentWave >= 5)
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            else
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
        }

        Instantiate(prefabToSpawn, LevelManager.Main.startPoint.position, Quaternion.identity);
    }

    // Calculates the number of enemies for the next wave using AIWaveHandler
    private int EnemiesPerWave()
    {
        int baseWaveEnemies = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
        return AIWaveHandler.Main.AdjustWaveDifficulty(baseWaveEnemies);
    }
}
