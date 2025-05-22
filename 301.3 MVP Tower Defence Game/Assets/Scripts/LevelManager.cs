using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;
    public int health;

    private void Awake()
    {
        Main = this;
    }

    private void Start()
    {
        currency = 250;
        health = 10;
        InGameBuyMenu.Main.OnGUI();
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        InGameBuyMenu.Main.OnGUI();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        Debug.Log("Player health decreased. Current health: " + health);
        if (health <= 0)
        {
            Debug.Log("Game Over!");
            // Add game over logic here
        }
        InGameBuyMenu.Main.OnGUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            InGameBuyMenu.Main.OnGUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough currency");
            return false;
        }
    }
}
