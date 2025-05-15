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
    
    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;  
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        rb.linearVelocity = direction * bulletSpeed;

        Destroy(gameObject, 6f); // Destroy the bullet after 5 seconds
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var healthComponent = other.gameObject.GetComponent<Health>();
        if (healthComponent == null) return;

        if (physicalDamage > 0)
        {
            int effectivePhysicalDamage = Mathf.Max((int)physicalDamage - Mathf.RoundToInt(physicalDamage * (healthComponent.physicalArmor / 50f)), 0);
            Debug.Log("Hit " + other.gameObject.name + " for " + effectivePhysicalDamage + " physical damage.");
            healthComponent.TakeDamage(effectivePhysicalDamage);
        }

        if (magicDamage > 0)
        {
            int effectiveMagicDamage = Mathf.Max((int)magicDamage - Mathf.RoundToInt(magicDamage * (healthComponent.magicArmor / 50f)), 0);
            Debug.Log("Hit " + other.gameObject.name + " for " + effectiveMagicDamage + " magic damage.");
            healthComponent.TakeDamage(effectiveMagicDamage);
        }

        Destroy(gameObject);
    }
}
