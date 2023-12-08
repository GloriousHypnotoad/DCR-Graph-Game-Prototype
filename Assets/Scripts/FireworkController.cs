using UnityEngine;

public class FireworkController : MonoBehaviour
{
    private Light _spotLight, _pointLight;
    private bool isPulsating = false;

    // Common properties for _pointLight
    private float minRange = 13f;
    private float maxRange = 15f;
    private float minIntensity = 26f;
    private float maxIntensity = 30f;

    // Specific properties for _spotLight
    private float minSpotAngle = 10f;
    private float maxSpotAngle = 12f;
    private float minSpotIntensity = 20f;
    private float maxSpotIntensity = 30f;

    private float pulseDuration = 8f;
    private float pulseTimer = 0f;

    void Start()
    {
        // Locate the spot and point light components
        _spotLight = transform.Find("SpotLightPendingTrue").GetComponent<Light>();
        _pointLight = transform.Find("PointLightPendingTrue").GetComponent<Light>();
    }

    void Update()
    {
        if (isPulsating)
        {
            float cycleFraction = (Mathf.Sin(pulseTimer / pulseDuration * Mathf.PI * 2) + 1) / 2;

            // Pulsate _pointLight
            _pointLight.range = Mathf.Lerp(minRange, maxRange, cycleFraction);
            _pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, cycleFraction);

            // Pulsate _spotLight with unique properties
            _spotLight.spotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, cycleFraction);
            _spotLight.intensity = Mathf.Lerp(minSpotIntensity, maxSpotIntensity, cycleFraction);

            pulseTimer += Time.deltaTime;

            if (pulseTimer > pulseDuration)
                pulseTimer = 0f;
        }
    }

    public void StartEffect()
    {
        isPulsating = true;
        pulseTimer = 0f; // Reset the timer when the effect starts
    }

    public void StopEffect()
    {
        isPulsating = false;
    }
}