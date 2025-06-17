using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float physicalDamage = 1;
    [SerializeField] private float magicDamage = 1;

    
    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetDamage(float physical, float magic)
    {
        physicalDamage = physical;
        magicDamage = magic;
    }
    
    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject); // Destroy the bullet if the target is gone
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        rb.linearVelocity = direction * bulletSpeed;

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        var healthComponent = target.GetComponent<Health>();
        if (healthComponent != null)
        {
            if (physicalDamage > 0)
            {
                int effectivePhysicalDamage = Mathf.Max((int)physicalDamage - Mathf.RoundToInt(physicalDamage * (healthComponent.physicalArmor / 50f)), 0);
                healthComponent.TakeDamage(effectivePhysicalDamage);
            }

            if (magicDamage > 0)
            {
                int effectiveMagicDamage = Mathf.Max((int)magicDamage - Mathf.RoundToInt(magicDamage * (healthComponent.magicArmor / 50f)), 0);
                healthComponent.TakeDamage(effectiveMagicDamage);
            }
        }

        Destroy(gameObject); // Destroy the bullet after hitting the target
    }
}
