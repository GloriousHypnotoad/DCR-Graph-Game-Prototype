using System;
using UnityEngine;

public class ActivtyExecuteButtonLightController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleLightsEnabled(bool enabled)
    {
        applyToAllLights(light => light.enabled = enabled);
    }

    public void setLightColors(Color color)
    {
        applyToAllLights(light => light.color = color);
    }

    private void applyToAllLights(Action<Light> action)
    {
        Light[] lights = transform.Find("Lights").gameObject.GetComponentsInChildren<Light>();
        foreach (Light light in lights)
        {
            action(light);
        }
    }
}