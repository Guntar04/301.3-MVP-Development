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
    [SerializeField] private float enemiesPerSecond = 0.3f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.7f;

    [Header("Events")]
    public static UnityEvent enemyKilled = new UnityEvent();
    public static EnemySpawner Main;

    public int currentWave = 1;
    private float timeSinceLastSpawn = 0f;
    public int enemiesAlive;
    public int enemiesLeftToSpawn;
    private bool isSpawning = false;
    
    private void Awake()
    {
        Main = this;
        enemyKilled.AddListener(OnEnemyKilled);
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

    private void SpawnEnemy() {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject prefabToSpawn = enemyPrefabs[0]; // Default to the first enemy type

        if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            if (currentWave >= 7 && currentWave <= 10)
            {
                // Waves 7-10: Spawn any of the first four enemy types
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            }
            else if (currentWave >= 5 && currentWave <= 6)
            {
                // Waves 5-6: Spawn any of the first three enemy types
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(3, enemyPrefabs.Length))];
            }
            else if (currentWave >= 3 && currentWave <= 4)
            {
                // Waves 3-4: Spawn any of the first two enemy types
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
            }
            // Waves 1-2: Only spawn the first enemy type (default behavior)
        }

        else if (sceneName == "Level 4" || sceneName == "Level 5" || sceneName == "Level 6")
        {
            if (currentWave >= 6 && currentWave <= 10)
            {
                // Waves 6-10: Spawn any of the first four enemy types
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(7, enemyPrefabs.Length))];
            }
            else if (currentWave >= 5 && currentWave <= 6)
            {
                // Waves 5-6: Spawn any of the first three enemy types
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(4, enemyPrefabs.Length))];
            }
            else if (currentWave >= 1 && currentWave <= 4)
            {
                // Waves 1-4: Spawn the first enemy type
                prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
            }
        }
        

        Instantiate(prefabToSpawn, LevelManager.Main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave(){
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
