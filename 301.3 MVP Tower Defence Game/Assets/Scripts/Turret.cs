using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class Turret : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject upgradedUpgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private SpriteRenderer baseSpriteRenderer;
    [SerializeField] private Sprite[] upgradeSprites; // 0: base, 1: level 2, 2: level 3

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f; // Bullets per second
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    private float targetingRangeBase;
    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(Sell);
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= (1f / bps))
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot() {
        
        GameObject bulletObj = Instantiate(bulletPrefab[0], firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget() {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + -90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    public void OpenUpgradeUI() {
        if (level < 3) {
            upgradeUI.SetActive(true);
            if (upgradedUpgradeUI != null) upgradedUpgradeUI.SetActive(false);
        } else {
            if (upgradeUI != null) upgradeUI.SetActive(false);
            if (upgradedUpgradeUI != null) upgradedUpgradeUI.SetActive(true);
        }
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null) upgradeUI.SetActive(false);
        if (upgradedUpgradeUI != null) upgradedUpgradeUI.SetActive(false);

        if (UIManager.Main != null)
        {
            UIManager.Main.SetHoveringState(false);
        }
    }

    public void Sell()
    {
        Debug.Log("Sell method called.");

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
        Debug.Log($"Turret sold for {refundAmount} currency.");
    }

    public void Upgrade()
    {
        if (level >= 3)
        {
            Debug.Log("Max level reached!");
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

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        // Change the base sprite if an upgraded sprite exists
        if (baseSpriteRenderer != null && upgradeSprites != null && level - 1 < upgradeSprites.Length)
        {
            Debug.Log("Setting base sprite to: " + upgradeSprites[level - 1]);
            baseSpriteRenderer.sprite = upgradeSprites[level - 1];
        }

        // Move the rotation point up for higher levels
        if (turretRotationPoint != null)
        {
            if (level == 2)
                turretRotationPoint.localPosition = new Vector3(turretRotationPoint.localPosition.x, 0.2f, turretRotationPoint.localPosition.z); // adjust 0.5f as needed
            else if (level == 3)
                turretRotationPoint.localPosition = new Vector3(turretRotationPoint.localPosition.x, 0.4f, turretRotationPoint.localPosition.z); // adjust 0.8f as needed
        }

        if (level >= 3 && upgradeButton != null)
        {
            upgradeButton.interactable = false;
        }

        CloseUpgradeUI();
        Debug.Log($"Turret upgraded to level {level}. New BPS: {bps}, New Range: {targetingRange}, New Cost: {CalculateCost()}");
    }

        private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }
    
    private float CalculateRange()
    {
        return Mathf.Round(targetingRangeBase * Mathf.Pow(level, 0.5f));
    }

    private float CalculateBPS()
    {
        return Mathf.Round(bpsBase * Mathf.Pow(level, 0.3f));
    }  

    private void OnDrawGizmosSelected()
    {

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

    }

}
