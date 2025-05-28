using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Main;

    private bool isHoveringUI;

    private void Awake()
    {
        Main = this; // Assign the static reference
        Debug.Log("UIManager initialized.");
    }

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
