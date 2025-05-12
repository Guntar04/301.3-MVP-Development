using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
}
