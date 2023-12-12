using UnityEngine;

public class PulsatingLight : MonoBehaviour
{
    
    private Light _lightComponent;
    private float _initialIntensity;
    private float _pulseIntensity;
    private float _pulseSpeed = 1.0f;
    private bool _isPulsating = false;
    
    void Start()
    {
        _lightComponent = GetComponent<Light>();
        _initialIntensity = _lightComponent.intensity;
        _pulseIntensity = _initialIntensity * 2;
    }

    void Update()
    {
        if (_isPulsating)
        {
            // Adjust the intensity to oscillate between 0 and the maximum value
            _lightComponent.intensity = (Mathf.Sin(Time.time * _pulseSpeed) * 0.5f + 0.5f) * _pulseIntensity;
        }
    }

    public void TogglePulse(bool shouldPulsate)
    {
        _isPulsating = shouldPulsate;
        
        if (!shouldPulsate)
        {
            // Reset the light's intensity to zero when pulsation is stopped
            _lightComponent.intensity = _initialIntensity;
        }
    }
}
