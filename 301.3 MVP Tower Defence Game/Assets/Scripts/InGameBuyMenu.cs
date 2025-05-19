using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InGameBuyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = false;

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.Main.currency.ToString();
        healthUI.text = LevelManager.Main.health.ToString();
    }

}
