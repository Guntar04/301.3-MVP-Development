using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;

    private bool isKilled = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isKilled)
        {
            EnemySpawner.enemyKilled.Invoke();
            isKilled = true;
            Destroy(gameObject);
        }
    }
    
}
