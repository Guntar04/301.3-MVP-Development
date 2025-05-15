using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    private bool isKilled = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isKilled)
        {
            EnemySpawner.enemyKilled.Invoke();
            LevelManager.Main.IncreaseCurrency(currencyWorth);
            isKilled = true;
            Destroy(gameObject);
        }
    }
    
}
