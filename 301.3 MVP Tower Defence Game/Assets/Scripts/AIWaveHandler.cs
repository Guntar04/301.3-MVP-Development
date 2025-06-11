using UnityEngine;
using System.Collections.Generic;

public class AIWaveHandler : MonoBehaviour
{
    public int enemiesThatReachedPoint;
    public int enemiesThatReachedEndpoint;
    public int adjustedWave;
    public int enemiesThatFailed;
    public static AIWaveHandler Main;

    private List<float> performanceHistory = new List<float>(); // Track player performance over waves
    private float playerPerformanceScore = 1f; // Current performance score
    private float learningRate = 0.1f; // How quickly the AI adapts
    private int maxHistorySize = 10; // Maximum number of waves to track
    private bool isFirstWave = true; // Flag to track if it's the first wave

    private void Awake()
    {
        Main = this;
    }

    public int AdjustWaveDifficulty(int upcomingWave)
    {
        // Skip difficulty adjustment for the first wave
        if (isFirstWave)
        {
            isFirstWave = false; // Mark the first wave as completed
            //Debug.Log("First wave started. Skipping AI difficulty adjustment.");
            return upcomingWave; // Return the original wave without adjustments
        }

        int adjustedWave = upcomingWave;

        // Check enemy progress
        enemiesThatReachedPoint = EnemySpawner.Main.enemiesReachedPathPoint; // Track enemies that reached halfway
        enemiesThatReachedEndpoint = EnemySpawner.Main.enemiesReachedEndpoint; // Track enemies that reached the endpoint

        // Ensure enemiesThatFailed is calculated correctly
        enemiesThatFailed = upcomingWave - enemiesThatReachedPoint - enemiesThatReachedEndpoint;
        //Debug.Log($"halfway: {enemiesThatReachedPoint}, endpoint: {enemiesThatReachedEndpoint}, failed: {enemiesThatFailed}");

        // Update player performance score
        playerPerformanceScore = UpdatePerformanceScore(upcomingWave);

        // Add the performance score to history
        performanceHistory.Add(playerPerformanceScore);
        if (performanceHistory.Count > maxHistorySize)
        {
            performanceHistory.RemoveAt(0); // Keep history size within limit
        }

        // Calculate average performance over history
        float averagePerformance = CalculateAveragePerformance();

        // Adjust difficulty based on average performance
        if (playerPerformanceScore >= 1.2f) // Player is performing exceptionally well
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond * 2f, 0.5f, 5f); // Aggressively increase spawn rate to be very fast
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.5f); // Increase enemies by 50%
            Debug.Log($"Player is performing exceptionally well. Aggressively increasing difficulty: {adjustedWave}");
        }
        else if (playerPerformanceScore >= 9.0f && playerPerformanceScore < 1.2) // Player is performing well
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond * 1.2f, 0.5f, 2.5f); // Increase spawn rate moderately
            adjustedWave += Mathf.RoundToInt(upcomingWave * 0.3f); // Increase enemies by 30%
            Debug.Log($"Player is performing well. Increasing difficulty moderately: {adjustedWave}");
        }
        else //if (playerPerformanceScore < 0.9f) // Player is struggling
        {
            EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(EnemySpawner.Main.enemiesPerSecond * 0.8f, 0.2f, 1f); // Increase spawn rate slightly
            adjustedWave -= Mathf.RoundToInt(upcomingWave * 0.1f); // Increase enemies by 10%
            Debug.Log($"Player is struggling. Slightly increasing difficulty: {adjustedWave}");
        }

        // Ensure at least one enemy is spawned
        return Mathf.Max(1, adjustedWave);
    }

    private float UpdatePerformanceScore(int upcomingWave)
    {
        // Get the total number of path points in the current level
        int totalPathPoints = EnemySpawner.Main.totalPathPoints;

        // Calculate the number of enemies killed before reaching any path point
        int enemiesKilledBeforePathPoints = upcomingWave - enemiesThatReachedPoint - enemiesThatReachedEndpoint;

        // Calculate ratios for each path point
        float earlyKillRatio = (float)enemiesKilledBeforePathPoints / upcomingWave; // Higher = better performance
        float halfwayRatio = (float)enemiesThatReachedPoint / upcomingWave; // Moderate = balanced performance
        float endpointPenalty = (float)enemiesThatReachedEndpoint / upcomingWave; // Higher = worse performance

        // Adjust weights dynamically based on the number of path points
        float pathPointPenalty = (float)(enemiesThatReachedPoint + enemiesThatReachedEndpoint) / (upcomingWave * totalPathPoints);

        // Combine metrics to calculate performance score
        // Adjusted weights to make the scoring system more balanced
        float newScore = (earlyKillRatio * 3f) - (halfwayRatio * 4f) - (endpointPenalty * 6f) - (pathPointPenalty * 5f);

        Debug.Log($"Early Kill Ratio: {earlyKillRatio}, Halfway Ratio: {halfwayRatio}, Endpoint Penalty: {endpointPenalty}, Path Point Penalty: {pathPointPenalty}");

        // Ensure the score is clamped between a reasonable range
        newScore = Mathf.Clamp(newScore, 0.1f, 2.0f);

        // Smoothly update the performance score using a learning rate
        playerPerformanceScore = Mathf.Lerp(playerPerformanceScore, newScore, learningRate);

        Debug.Log($"Updated Player Performance Score: {playerPerformanceScore}");
        return playerPerformanceScore;
    }

    private float CalculateAveragePerformance()
    {
        if (performanceHistory.Count == 0) return 1f; // Default to balanced performance if no history
        float total = 0f;
        foreach (float score in performanceHistory)
        {
            total += score;
        }
        return total / performanceHistory.Count;
    }
}