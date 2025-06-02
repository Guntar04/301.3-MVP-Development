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

        if (LevelManager.Main.health > 8)
        {
            // Increase difficulty for higher health
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.5f, 1.5f);
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.5f); // Increase enemies by 50%
            Debug.Log("Wave difficulty increased for wave: " + upcomingWave);
        }
        else if (LevelManager.Main.health > 5)
        {
            // Moderate difficulty
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(0.7f, 1.2f);
            Debug.Log("Wave difficulty remains moderate for wave: " + upcomingWave);
        }
        else if (LevelManager.Main.health > 2)
        {
            // Decrease difficulty for lower health
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(1.0f, 2.0f);
            adjustedWave -= Mathf.RoundToInt(upcomingWave * 0.3f); // Decrease enemies by 30%
            Debug.Log("Wave difficulty decreased for wave: " + upcomingWave);
        }
        else
        {
            // Minimum difficulty for very low health
            EnemySpawner.Main.enemiesPerSecond = 1f / Random.Range(1.5f, 3.0f);
            adjustedWave -= Mathf.RoundToInt(upcomingWave * 0.3f); // Decrease enemies by 30%
            Debug.Log("Wave difficulty significantly decreased for wave: " + upcomingWave);
        }

        // Ensure at least one enemy is spawned
        return Mathf.Max(1, adjustedWave);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
