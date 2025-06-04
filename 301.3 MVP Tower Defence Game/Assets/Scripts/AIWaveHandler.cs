using UnityEngine;

public class AIWaveHandler : MonoBehaviour
{

    public static AIWaveHandler Main;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public int AdjustWaveDifficulty(int upcomingWave)
    {
        int adjustedWave = upcomingWave;

        // Check enemy progress
        int enemiesThatReachedPoint = EnemySpawner.Main.enemiesReachedPathPoint; // Track enemies that reached halfway
        int enemiesThatReachedEndpoint = EnemySpawner.Main.enemiesReachedEndpoint; // Track enemies that reached the endpoint

        // Ensure enemiesThatFailed is calculated correctly
        int enemiesThatFailed = adjustedWave - enemiesThatReachedPoint - enemiesThatReachedEndpoint;

        Debug.Log($"Adjusting Difficulty: Upcoming Wave = {upcomingWave}, Enemies Reached Halfway = {enemiesThatReachedPoint}, Enemies Reached Endpoint = {enemiesThatReachedEndpoint}, Enemies Failed = {enemiesThatFailed}");

        if (enemiesThatFailed > 0 && enemiesThatReachedPoint < upcomingWave * 0.5f) // If less than 50% of enemies reach halfway
        {
            // Increase difficulty significantly
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.5f, 1f);
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.5f); // Increase enemies by 50%
            Debug.Log($"Wave difficulty increased significantly: {adjustedWave}");
        }
        else if (enemiesThatReachedPoint >= upcomingWave * 0.5f && enemiesThatReachedPoint < upcomingWave * 0.8f) // If 50%-80% of enemies reach halfway
        {
            // Keep difficulty the same
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.7f, 1.2f);
            Debug.Log($"Wave difficulty remains the same: {adjustedWave}");
        }
        else if (enemiesThatReachedPoint >= upcomingWave * 0.8f) // If more than 80% of enemies reach halfway
        {
            // Decrease difficulty
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(1.0f, 2.0f);
            adjustedWave -= Mathf.RoundToInt(upcomingWave * 0.3f); // Decrease enemies by 30%
            Debug.Log($"Wave difficulty decreased: {adjustedWave}");
        }

        // Ensure at least one enemy is spawned
        return Mathf.Max(1, adjustedWave);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
