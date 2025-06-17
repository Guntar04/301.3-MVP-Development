using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turret : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private TextMeshProUGUI towerLevelText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private Button sellButton;
    [SerializeField] private SpriteRenderer baseSpriteRenderer;
    [SerializeField] private Sprite[] upgradeSprites; // 0: base, 1: level 2, 2: level 3

    [Header("Attributes")]
    [SerializeField] public float targetingRange = 4f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] public float bps = 1.5f; // Bullets per second
    [SerializeField] private int baseUpgradeCost = 100;

    // Damage values (set per tower type in Inspector)
    [SerializeField] private float basePhysicalDamage = 6f;
    [SerializeField] private float baseMagicDamage = 0f;

    // Upgrade increments (set per tower type in Inspector)
    [SerializeField] private float physicalDamageUpgrade = 3f;
    [SerializeField] private float magicDamageUpgrade = 0f;
    [SerializeField] private float rangeUpgrade = 0.5f;
    [SerializeField] private float bpsUpgrade = 0.2f;

    private float fireCooldown = 0f;
    private int level = 1;
    private Transform target;

    private void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(Sell);

        UpdateUpgradeCostText(); // Initialize the upgrade cost text
    }

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            RotateTowardsTarget();

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / bps;
            }
        }

        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        UpdateUpgradeCostText();
    }

    private void Shoot() {
        GameObject bulletObj = Instantiate(bulletPrefab[0], firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);

        // Set bullet damage based on current tower stats
        bulletScript.SetDamage(basePhysicalDamage, baseMagicDamage);
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        EnemyMovement frontEnemy = null;
        int furthestPathIndex = -1;

        foreach (var hit in hits)
        {
            EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
            if (enemy != null && enemy.pathIndex > furthestPathIndex)
            {
                furthestPathIndex = enemy.pathIndex;
                frontEnemy = enemy;
            }
        }

        target = frontEnemy != null ? frontEnemy.transform : null;
    }

    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget() {
        Vector2 direction = (target.position - turretRotationPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            Quaternion.Euler(0, 0, angle - 90),
            rotationSpeed * Time.deltaTime
        );
    }
    
    public void OpenUpgradeUI() {
        if (level <= 3)
        {
            upgradeUI.SetActive(true);  
        }
        else
        {
            if (upgradeUI != null) upgradeUI.SetActive(false);
        }
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null) upgradeUI.SetActive(false);

        if (UIManager.Main != null)
        {
            UIManager.Main.SetHoveringState(false);
        }
    }

    public void Sell()
    {
        //Debug.Log("Sell method called.");

        if (sellButton != null) sellButton.interactable = false;

        if (LevelManager.Main == null)
        {
            Debug.LogError("LevelManager.Main is null in Sell!");
            return;
        }

        // Refund a portion of the cost based on the level
        int refundAmount = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f) * 0.6f);
        LevelManager.Main.IncreaseCurrency(refundAmount);

        // Destroy the turret object
        Destroy(gameObject);
        //Debug.Log($"Turret sold for {refundAmount} currency.");
    }

    public void Upgrade()
    {
        if (level >= 3)
        {
            if (upgradeButton != null) upgradeButton.interactable = false;
            return;
        }

        if (CalculateCost() > LevelManager.Main.currency)
        {
            Debug.Log("You can't afford this upgrade");
            return;
        }

        LevelManager.Main.SpendCurrency(CalculateCost());
        level++;

        // Upgrade stats
        targetingRange += rangeUpgrade;
        bps += bpsUpgrade;
        basePhysicalDamage += physicalDamageUpgrade;
        baseMagicDamage += magicDamageUpgrade;

        // Change the base sprite if an upgraded sprite exists
        if (baseSpriteRenderer != null && upgradeSprites != null && level - 1 < upgradeSprites.Length)
        {
            //Debug.Log("Setting base sprite to: " + upgradeSprites[level - 1]);
            baseSpriteRenderer.sprite = upgradeSprites[level - 1];
        }

        // Move the rotation point up for higher levels
        if (turretRotationPoint != null)
        {
            if (level == 2)
                turretRotationPoint.localPosition = new Vector3(turretRotationPoint.localPosition.x, 0.1f, turretRotationPoint.localPosition.z); // adjust 0.5f as needed
            else if (level == 3)
                turretRotationPoint.localPosition = new Vector3(turretRotationPoint.localPosition.x, 0.3f, turretRotationPoint.localPosition.z); // adjust 0.8f as needed
        }

        // if (level >= 3 && upgradeButton != null)
        // {
        //     upgradeButton.interactable = false;
        // }

        UpdateUpgradeCostText(); // Update the cost text after upgrading
        CloseUpgradeUI();
        //Debug.Log($"Turret upgraded to level {level}. New BPS: {bps}, New Range: {targetingRange}, New Cost: {CalculateCost()}");
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.7f));
    } 

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private void UpdateUpgradeCostText()
{
    if (upgradeCostText != null)
    {
            towerLevelText.text = "Lv: " + level;
        if (level >= 3)
            {
                upgradeCostText.text = "Max Level";
            }
            else
            {
                upgradeCostText.text = $"${CalculateCost()}";
            }
    }
}
}
