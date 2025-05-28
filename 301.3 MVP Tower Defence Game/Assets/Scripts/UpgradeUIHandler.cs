using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.Main != null)
        {
            UIManager.Main.SetHoveringState(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Main != null)
        {
            UIManager.Main.SetHoveringState(false);
        }

        gameObject.SetActive(false); // Hide the upgrade UI when not hovering
    }
}
