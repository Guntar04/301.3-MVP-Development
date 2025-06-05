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
    public int enemiesReachedPathPoint = 0;
    public int enemiesReachedEndpoint = 0; // Track enemies that reached the endpoint

    private Transform target;
    private int pathIndex = 0;


    private void Awake()
    {
        Main = this;
        enemyKilled.AddListener(OnEnemyKilled);
        AIWaveHandler.Main = FindFirstObjectByType<AIWaveHandler>();
    }


    private void Start()
    {
        if (LevelManager.Main.path.Length > 0)
        {
            target = LevelManager.Main.path[pathIndex];
        }
        else
        {
            Debug.LogError("LevelManager.Main.path is empty! Ensure waypoints are assigned.");
        }

        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        if (LevelManager.Main == null || LevelManager.Main.health <= 0)
        {
            Debug.LogError("LevelManager.Main is null or health is zero!");
            isSpawning = false;
            GameOver.Main.ShowGameOver();
            return;
        }

        if (target == null)
        {
            Debug.LogError("Target is null! Ensure waypoints are assigned.");
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            enemiesPerSecond = 1f / Random.Range(0.4f, 1.5f);
            timeSinceLastSpawn = 0f;
        }

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.Main.path.Length)
            {
                EnemySpawner.enemyKilled.Invoke();
                LevelManager.Main.DecreaseHealth(1);
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.Main.path[pathIndex];
            }
        }

        if (enemiesLeftToSpawn <= 0 && enemiesAlive <= 0)
        {
            EndWave();
        }

    }

    private void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        enemiesReachedPathPoint = 0; // Reset the halfway counter for the new wave
        enemiesReachedEndpoint = 0;  // Reset the endpoint counter for the new wave
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave(); // Calculate the number of enemies for the wave
        //Debug.Log($"Starting Wave {currentWave}: Enemies to Spawn = {enemiesLeftToSpawn}");
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        //Debug.Log($"Wave {currentWave} ended. Preparing for next wave...");
        currentWave++; // Increment the wave counter

        // Adjust difficulty after the wave ends
        upcomingWave = EnemiesPerWave();
        Debug.Log($"Adjusting Difficulty: Upcoming Wave = {upcomingWave}, Enemies Reached Halfway = {AIWaveHandler.Main.enemiesThatReachedPoint}, Enemies Reached Endpoint = {AIWaveHandler.Main.enemiesThatReachedEndpoint}, Enemies Failed = {AIWaveHandler.Main.enemiesThatFailed}");

        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject prefabToSpawn = enemyPrefabs[0]; // Default to the first enemy type

        if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            if (currentWave >= 7)
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            }
            else if (currentWave >= 5)
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(3, enemyPrefabs.Length))];
            }
            else if (currentWave >= 3)
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
            }
        }
        else if (sceneName == "Level 4" || sceneName == "Level 5" || sceneName == "Level 6")
        {
            if (currentWave >= 6)
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(7, enemyPrefabs.Length))];
            }
            else if (currentWave >= 5)
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            }
            else
            {
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
            }
        }

        Instantiate(prefabToSpawn, LevelManager.Main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        // Calculate the base number of enemies for the wave
        int baseWaveEnemies = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));

        // Adjust the wave difficulty based on enemy progress
        upcomingWave = AIWaveHandler.Main.AdjustWaveDifficulty(baseWaveEnemies);

        //Debug.Log($"Wave {currentWave}: Base Enemies = {baseWaveEnemies}, Adjusted Enemies = {upcomingWave}");
        return upcomingWave;
    }
    
}
