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


    private void Awake()
    {
        Main = this;
        enemyKilled.AddListener(OnEnemyKilled);
        AIWaveHandler.Main = FindFirstObjectByType<AIWaveHandler>();
    }


    private void Start() {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            enemiesPerSecond = 1f / Random.Range(0.4f, 1.5f);
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            if (currentWave >= 10)
            {
                isSpawning = false;
                GameOver.Main.WinGame(); // Show the victory screen
            }
            else
            {
                EndWave();
            }
        }

        if (LevelManager.Main.health <= 0)
        {
            isSpawning = false;
            GameOver.Main.ShowGameOver(); // Show the game over screen
        }
    }

    private void OnEnemyKilled() {
        enemiesAlive--;
    }

    private IEnumerator StartWave() {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave() {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
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
        upcomingWave = Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
        return AIWaveHandler.Main.AdjustWaveDifficulty(upcomingWave);
    }
}
