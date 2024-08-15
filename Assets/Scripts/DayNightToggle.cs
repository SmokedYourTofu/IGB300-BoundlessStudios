using UnityEngine;

public class DayNightToggle : MonoBehaviour
{
    public Light directionalLight;  // Assign your directional light in the Inspector

    void Start()
    {
        // Ensure the directional light is assigned
        if (directionalLight == null)
        {
            Debug.LogError("Directional light not assigned in the Inspector.");
            return;
        }

        // Toggle between day and night mode
        ToggleDayNightMode();
    }

    void ToggleDayNightMode()
    {
        // Generate a random number between 0 and 1
        int randomValue = Random.Range(0, 2);

        // If randomValue is 0, it's night mode (light off); if 1, it's day mode (light on)
        bool isDay = randomValue == 1;

        // Set the light's active state based on the random value
        directionalLight.gameObject.SetActive(isDay);

        // Log the current mode for debugging purposes
        Debug.Log(isDay ? "Day mode activated." : "Night mode activated.");
    }
}
