using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Main.SetHoveringState(false);
        gameObject.SetActive(false); // Hide the upgrade UI when not hovering
    }
}
