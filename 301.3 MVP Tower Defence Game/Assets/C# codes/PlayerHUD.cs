using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    public Image healthbarfill;
    public TMP_Text scoreText;

    private float maxHealth = 100f;
    private float currentHealth;
    private int score;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        updateScore(0);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void updateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    private void UpdateHealthBar()
    {
        healthbarfill.fillAmount = currentHealth / maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
