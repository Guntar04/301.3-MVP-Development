using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    

    [Header("Attributes")]
    [SerializeField] private float hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;
    [SerializeField] public int physicalArmor = 0;
    [SerializeField] public int magicArmor = 0;

    private bool isKilled = false;

    public void TakeDamage(int dmg)
    {
        int effectiveDamage = dmg;

        if (physicalArmor > 0 && magicArmor == 0) // Physical damage scenario
        {
            effectiveDamage -= Mathf.RoundToInt(dmg * (physicalArmor / 50f));
        }
        else if (magicArmor > 0 && physicalArmor == 0) // Magic damage scenario
        {
            effectiveDamage -= Mathf.RoundToInt(dmg * (magicArmor / 50f));
        }
        else if (physicalArmor > 0 && magicArmor > 0) // Mixed damage scenario
        {
            effectiveDamage -= Mathf.RoundToInt(dmg * ((physicalArmor + magicArmor) / 100f));
        }

        effectiveDamage = Mathf.Max(effectiveDamage, 0); // Ensure damage is not negative
        dmg = effectiveDamage;
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isKilled)
        {
            EnemySpawner.enemyKilled.Invoke();
            LevelManager.Main.IncreaseCurrency(currencyWorth);
            isKilled = true;

            // Let EnemyMovement handle death and progress reporting
            EnemyMovement movement = GetComponent<EnemyMovement>();
            if (movement != null)
                movement.Die();
            else
                Destroy(gameObject);
        }
    }
    
}
