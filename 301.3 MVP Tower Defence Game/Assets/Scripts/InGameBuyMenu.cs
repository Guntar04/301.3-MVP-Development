using UnityEngine;
using TMPro;

public class InGameBuyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;

    public static InGameBuyMenu Main { get; private set; }

    private bool isMenuOpen = false;

    private void Awake()
    {
        Main = this;
        isMenuOpen = false;
        gameObject.SetActive(false); // Start with the menu disabled
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        gameObject.SetActive(isMenuOpen); // Enable or disable the menu GameObject

        if (isMenuOpen)
        {
            Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
        }
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        gameObject.SetActive(false); // Disable the menu GameObject
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.Main.currency.ToString();
        healthUI.text = LevelManager.Main.health.ToString();
    }
}
