using UnityEngine;

public class AIWaveHandler : MonoBehaviour
{
    public int enemiesThatReachedPoint;
    public int enemiesThatReachedEndpoint;
    public int adjustedWave;
    public int enemiesThatFailed;
    public static AIWaveHandler Main;

    private float playerPerformanceScore = 1.0f; // Tracks player performance (1.0 = balanced, >1 = good, <1 = struggling)
    private float learningRate = 0.2f; // Determines how quickly the AI adapts to player performance

    private void Awake()
    {
        Main = this;
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

        // Calculate player performance score
        playerPerformanceScore = UpdatePerformanceScore(upcomingWave, enemiesThatReachedPoint, enemiesThatReachedEndpoint);

        // Adjust difficulty based on player performance
        if (playerPerformanceScore > 0.8f || playerPerformanceScore < 1.2f) // Player is performing exceptionally well
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond, 0.5f, 1.5f); // Keep spawn rate the same
            Debug.Log($"Player performance is balanced. Keeping difficulty the same: {adjustedWave}");
        }
        else if (playerPerformanceScore > 1.2f) // Player is performing well
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond * (1.2f), 0.5f, 2.5f); // Increase spawn rate
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.5f); // Increase enemies by 50%
            Debug.Log($"Player is performing well. Increasing difficulty: {adjustedWave}");
        }
        else if (playerPerformanceScore < 0.8f) // Player is struggling
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond * (0.8f), 0.2f, 1f); // Decrease spawn rate
            adjustedWave -= Mathf.RoundToInt(upcomingWave * 0.3f); // Decrease enemies by 30%
            Debug.Log($"Player is struggling. Decreasing difficulty: {adjustedWave}");
        }

        // Ensure at least one enemy is spawned
        return Mathf.Max(1, adjustedWave);
    }

    private float UpdatePerformanceScore(int upcomingWave, int halfwayReached, int endpointReached)
    {
        // Calculate the number of enemies killed before reaching halfway
        int enemiesKilledBeforeHalfway = upcomingWave - halfwayReached - endpointReached;

        // Performance score based on enemies killed early, halfway reached, and endpoint reached
        float earlyKillRatio = (float)enemiesKilledBeforeHalfway / upcomingWave; // Higher = better performance
        float halfwayRatio = (float)halfwayReached / upcomingWave; // Moderate = balanced performance
        float endpointPenalty = (float)endpointReached / upcomingWave; // Higher = worse performance

        // Combine metrics to calculate performance score
        float newScore = (earlyKillRatio * 2f) + (halfwayRatio * 1f) - (endpointPenalty * 2f); // Boost early kills and penalize endpoint

        // Smoothly update the performance score using a learning rate
        playerPerformanceScore = Mathf.Lerp(playerPerformanceScore, newScore, learningRate);

        Debug.Log($"Updated Player Performance Score: {playerPerformanceScore}");
        return playerPerformanceScore;
    }
}
