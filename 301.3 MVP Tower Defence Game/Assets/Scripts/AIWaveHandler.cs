using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AIWaveHandler uses AI learning to adapt wave difficulty based on how far enemies get.
/// </summary>
public class AIWaveHandler : MonoBehaviour
{
    public static AIWaveHandler Main;

    private List<float> enemyProgresses = new List<float>();
    private List<float> performanceHistory = new List<float>();
    private float aiDifficulty = 1f;
    private float learningRate = 0.5f;
    private float maxDifficulty = 1.3f;
    private float minDifficulty = 0.7f;
    private int maxHistory = 8;

    private Dictionary<int, List<float>> enemyTypeProgresses = new Dictionary<int, List<float>>();

    private void Awake()
    {
        Main = this;
    }

    /// <summary>
    /// Called by EnemyMovement when an enemy dies or reaches the end.
    /// </summary>
    public void ReportEnemyProgress(int pathIndex)
    {
        // Normalize progress as a fraction of the path length
        float progress = (float)pathIndex / (LevelManager.Main.path.Length - 1);
        enemyProgresses.Add(progress);
    }

    public void ReportEnemyProgress(int pathIndex, int enemyType)
    {
        float progress = (float)pathIndex / (LevelManager.Main.path.Length - 1);
        if (!enemyTypeProgresses.ContainsKey(enemyType))
            enemyTypeProgresses[enemyType] = new List<float>();
        enemyTypeProgresses[enemyType].Add(progress);
    }

    /// <summary>
    /// Call this at the end of each wave to update AI and get the next wave's enemy count.
    /// </summary>
    public int AdjustWaveDifficulty(int baseEnemies)
    {
        // Calculate average progress for this wave
        float avgProgress = 0f;
        if (enemyProgresses.Count > 0)
        {
            foreach (var p in enemyProgresses) avgProgress += p;
            avgProgress /= enemyProgresses.Count;
        }

        // Higher avgProgress means enemies get further (player struggling)
        // Lower avgProgress means player is stopping enemies early (player strong)
        float score = 1f - avgProgress; // 1 = stopped early, 0 = all reached end

        // AI "learns" by blending new score with history
        if (performanceHistory.Count == 0)
            aiDifficulty = 1f;
        else
            aiDifficulty = Mathf.Lerp(aiDifficulty, 1f + (score - 0.5f), learningRate);

        // Store history for smoothing
        performanceHistory.Add(score);
        if (performanceHistory.Count > maxHistory)
            performanceHistory.RemoveAt(0);

        float avgScore = 0f;
        foreach (var s in performanceHistory) avgScore += s;
        avgScore /= performanceHistory.Count;

        // Adjust difficulty
        if (avgScore > 0.7f)
            aiDifficulty = Mathf.Min(aiDifficulty + 0.05f, maxDifficulty);
        else if (avgScore < 0.4f)
            aiDifficulty = Mathf.Max(aiDifficulty - 0.05f, minDifficulty);

        // Calculate new enemy count and spawn rate
        int adjustedEnemies = Mathf.RoundToInt(baseEnemies * aiDifficulty);
        EnemySpawner.Main.enemiesPerSecond = Mathf.Clamp(0.3f * aiDifficulty, 0.15f, 2f);

        // Ensure at least 1 enemy
        adjustedEnemies = Mathf.Max(1, adjustedEnemies);

        Debug.Log($"[AIWaveHandler] AvgProgress: {avgProgress:F2}, Player avgScore: {avgScore:F2}, AI Difficulty: {aiDifficulty:F2}, Next Enemies: {adjustedEnemies}, SpawnRate: {EnemySpawner.Main.enemiesPerSecond:F2}");

        // Reset for next wave
        enemyProgresses.Clear();
        enemyTypeProgresses.Clear();

        return adjustedEnemies;
    }

    private Dictionary<int, float> GetAverageProgressPerType()
    {
        var averages = new Dictionary<int, float>();
        foreach (var kvp in enemyTypeProgresses)
        {
            float avg = 0f;
            foreach (var p in kvp.Value) avg += p;
            avg /= Mathf.Max(1, kvp.Value.Count);
            averages[kvp.Key] = avg;
        }
        return averages;
    }

    public int[] DecideEnemyComposition(int baseEnemies, int wave, int enemyTypeCount)
    {
        var avgProgress = GetAverageProgressPerType();
        int[] result = new int[enemyTypeCount];

        // Determine which types are unlocked this wave (follow your EnemySpawner logic)
        int maxType = 0;
        if (wave >= 7) maxType = Mathf.Min(3, enemyTypeCount - 1);
        else if (wave >= 5) maxType = Mathf.Min(2, enemyTypeCount - 1);
        else if (wave >= 3) maxType = Mathf.Min(1, enemyTypeCount - 1);

        // Default ratios (can be tuned)
        float[] baseRatios = new float[enemyTypeCount];
        for (int i = 0; i <= maxType; i++) baseRatios[i] = 1f;
        float totalRatio = maxType + 1;

        // Adjust ratios based on performance
        for (int i = 0; i <= maxType; i++)
        {
            float perf = 1f - (avgProgress.ContainsKey(i) ? avgProgress[i] : 0.5f); // 1=player strong, 0=player weak
            // If player is struggling, reduce ratio for this type (especially for tanky types)
            if (perf < 0.4f)
                baseRatios[i] *= 0.6f;
            else if (perf > 0.7f)
                baseRatios[i] *= 1.2f;

            // For tanky types (e.g., Enemy 02, 06, 07), never let their ratio exceed 0.3
            if (i == 1 || i == 5 || i == 6)
                baseRatios[i] = Mathf.Min(baseRatios[i], 0.3f);
        }

        // Normalize ratios
        float sum = 0f;
        for (int i = 0; i <= maxType; i++) sum += baseRatios[i];
        for (int i = 0; i <= maxType; i++) baseRatios[i] /= sum;

        // Assign counts
        for (int i = 0; i <= maxType; i++)
            result[i] = Mathf.RoundToInt(baseEnemies * baseRatios[i]);

        // Ensure at least 1 of each unlocked type
        for (int i = 0; i <= maxType; i++)
            result[i] = Mathf.Max(result[i], 1);

        return result;
    }
}