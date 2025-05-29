using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start() {
        target = LevelManager.Main.path[pathIndex];
    }

    private void Update() {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) {
            pathIndex++;

            if (pathIndex == LevelManager.Main.path.Length) {
                EnemySpawner.enemyKilled.Invoke();
                LevelManager.Main.DecreaseHealth(1);
                Destroy(gameObject);
                return;
            }
            else {
                target = LevelManager.Main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate() {
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