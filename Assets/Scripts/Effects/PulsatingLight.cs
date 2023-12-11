using UnityEngine;

public class PulsatingLight : MonoBehaviour
{
     private float pulseSpeed = 1.0f;
    private float pulseIntensity = 4.0f; // The maximum intensity during the pulse

    private Light lightComponent;
    private bool isPulsating = false;
    
    void Start()
    {
        lightComponent = GetComponent<Light>();
        lightComponent.intensity = 0; // Start with intensity at zero
    }

    void Update()
    {
        if (isPulsating)
        {
            // Adjust the intensity to oscillate between 0 and the maximum value
            lightComponent.intensity = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f) * pulseIntensity;
        }
    }

    public void TogglePulse(bool shouldPulsate)
    {
        isPulsating = shouldPulsate;
        
        if (!shouldPulsate)
        {
            // Reset the light's intensity to zero when pulsation is stopped
            lightComponent.intensity = 0;
        }
    }
}
