using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        GameObject prefabToSpawn = enemyPrefabs[0]; // Default to the first enemy type

        if (currentWave >= 5) {
            // Waves 5-10: Spawn any of the three enemy types
            prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(3, enemyPrefabs.Length))];
        } 
        else if (currentWave >= 3) {
            // Waves 3-4: Spawn either the first or second enemy type
            prefabToSpawn = enemyPrefabs[Random.Range(0, Mathf.Min(2, enemyPrefabs.Length))];
        }

        Instantiate(prefabToSpawn, LevelManager.Main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave(){
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
