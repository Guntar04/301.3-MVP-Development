using UnityEngine;

public class AIWaveHandler : MonoBehaviour
{

    public int enemiesThatReachedPoint;
    public int enemiesThatReachedEndpoint;
    public int adjustedWave;
    public int enemiesThatFailed;
    public static AIWaveHandler Main;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Main = this;
    }
    void Start()
    {
        
    }

    public int AdjustWaveDifficulty(int upcomingWave)
    {
        int adjustedWave = upcomingWave;

        // Check enemy progress
        enemiesThatReachedPoint = EnemySpawner.Main.enemiesReachedPathPoint; // Track enemies that reached halfway
        enemiesThatReachedEndpoint = EnemySpawner.Main.enemiesReachedEndpoint; // Track enemies that reached the endpoint

        // Ensure enemiesThatFailed is calculated correctly
        enemiesThatFailed = upcomingWave - enemiesThatReachedPoint - enemiesThatReachedEndpoint;
        Debug.Log($"halfway: {enemiesThatReachedPoint}, endpoint: {enemiesThatReachedEndpoint}, failed: {enemiesThatFailed}");

        if (enemiesThatReachedEndpoint >= 2) // If more than 2 enemies reach the endpoint
        {
            // Keep difficulty the same
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(1.2f, 2f);
            Debug.Log($"Wave difficulty decreased: {adjustedWave}");
        }

        else if (enemiesThatFailed > 0 && enemiesThatReachedPoint < upcomingWave * 0.5f) // If less than 50% of enemies reach halfway
        {
            // Increase difficulty significantly
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.5f, 1f);
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.6f); // Increase enemies by 70%
            Debug.Log($"Wave difficulty increased significantly: {adjustedWave}");
        }
        else if (enemiesThatReachedPoint >= upcomingWave * 0.5f && enemiesThatReachedPoint < upcomingWave * 0.8f) // If 50%-80% of enemies reach halfway
        {
            // Increase difficulty moderately
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.7f, 1.2f);
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.4f); // Increase enemies by 50%
            Debug.Log($"Wave difficulty moderately increased: {adjustedWave}");
        }
        else if (enemiesThatReachedPoint >= upcomingWave * 0.8f) // If more than 80% of enemies reach halfway
        {
            // Increase difficulty slightly
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.8f, 1.5f);
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.2f); // Increase enemies by 40%
            Debug.Log($"Wave difficulty slightly increased: {adjustedWave}");
        }

        // Ensure at least one enemy is spawned
        return Mathf.Max(1, adjustedWave);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
