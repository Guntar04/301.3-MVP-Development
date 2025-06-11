using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // Reference to a TextMeshProUGUI element to display FPS
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate the time between frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Clamp FPS to a maximum of 60
        fps = Mathf.Min(fps, 60f);

        // Update the TextMeshProUGUI element
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}