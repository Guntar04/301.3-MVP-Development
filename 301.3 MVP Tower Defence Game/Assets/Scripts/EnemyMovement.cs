using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

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
    }

    public void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.Main.path.Length)
            {
                //Debug.Log("Enemy reached the endpoint, dealing damage.");
                EnemySpawner.enemyKilled.Invoke();

                // Increment the counter for enemies that reached the endpoint
                EnemySpawner.Main.enemiesReachedEndpoint++;

                LevelManager.Main.DecreaseHealth(1);
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.Main.path[pathIndex];
            }

            // // Notify EnemySpawner when the enemy reaches the halfway point
            if (pathIndex == LevelManager.Main.path.Length / 2) // Halfway through the path
            {
                //Debug.Log("Enemy reached halfway point, incrementing enemiesReachedPathPoint.");
                EnemySpawner.Main.enemiesReachedPathPoint++;
                // Debug.Log($"Enemies reached halfway point: {EnemySpawner.Main.enemiesReachedPathPoint}");
                
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    public float GetPathProgress()
    {
        if (pathIndex >= LevelManager.Main.path.Length)
            return float.MaxValue; // Enemy has reached the end of the path

        if (pathIndex == 0)
            return 0f; // Enemy is at the start of the path

        float distanceToNextWaypoint = Vector2.Distance(transform.position, LevelManager.Main.path[pathIndex].position);
        float distanceBetweenWaypoints = Vector2.Distance(LevelManager.Main.path[pathIndex - 1].position, LevelManager.Main.path[pathIndex].position);
        
        return pathIndex + (1f - (distanceToNextWaypoint / distanceBetweenWaypoints));
    }
}