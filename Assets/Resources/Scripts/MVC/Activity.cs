using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class Activity : MonoBehaviour
{
    public string Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    private Action<Activity> _onExecuted;
    ButtonController ButtonController;
    ButtonController ButtonControllerExcluded;

    // Start is called before the first frame update
    void Start()
    { 
    }

      public void Initialize(string id, string label)
    {       
        //Child Object Components
        ButtonController = transform.Find("ActivityExecuteButton").gameObject.GetComponent<ButtonController>();
        Debug.Log(ButtonController);
        ButtonControllerExcluded = transform.Find("ActivityExecuteButtonExcluded").gameObject.GetComponent<ButtonController>();
        Id = id;
        Label = label;
        ButtonController.SubscribeToOnPressed(OnButtonPressed);
        ButtonControllerExcluded.SubscribeToOnPressed(OnButtonPressed);
    }
    public void OnButtonPressed(float quickRotationDuration)
    {
        _onExecuted?.Invoke(this);
        GlitterBurst(quickRotationDuration);
        // Display execution
        // Highlight outgoing constraints
    }
    public void SetExecuted(bool isExecuted){
        if(isExecuted){
            SetGlitterColor(Color.green);
        }
        SetLightState(isExecuted);
        UpdatePushButtonMaterial(isExecuted);
    }
    public void SetPending(bool isPending)
    {
        ToggleChildObjects(isPending,
            "ActivityExecuteButton/BodyObject/Lights",
            "ActivityExecuteButtonExcluded/BodyObject/Lights",
            "ActivityProps/PropLights",
            "ActivityPropsExcluded/PropLights");

        if (isPending)
        {
            ButtonController.StartJumping();
        }
        else
        {
            ButtonController.StopJumping();
        }
    }
    public void SetDisabled(bool isDisabled)
    {
        if (!isDisabled)
        {
            ButtonController.StartRotation();
        }
        else
        {
            ButtonController.StopRotation();
        }
        // Disable or enable "Glitter" object and buttons
        ToggleChildObjects(isDisabled, "Fog/Fog");
        ToggleChildObjects(!isDisabled, "Glitter");
        SetButtonsEnabled(!isDisabled, "ActivityExecuteButton", "ActivityExecuteButtonExcluded");
    }
    public void SetHasUnmetMilestones(bool hasUnmetMilestones)
    {
        if(hasUnmetMilestones){
            ButtonController.StopRotation();
            ToggleChildObjects(true, "Fog/Fog");
            ToggleChildObjects(false, "Glitter");
            SetButtonsEnabled(false, "ActivityExecuteButton", "ActivityExecuteButtonExcluded");
        }
    }
    public void SetIncluded(bool isIncluded)
    {
        // Enable or disable specified child components
        ToggleChildObjects(
            isIncluded, 
            "ActivityExecuteButton", 
            "ActivityProps"
        );

        ToggleChildObjects(
            !isIncluded, 
            "ActivityExecuteButtonExcluded", 
            "ActivityPropsExcluded"
        ); // Also handling "Glitter" in the same manner

        // Handle button states as per disabled logic
        if(!isIncluded){
            ToggleChildObjects(isIncluded, "Glitter");
            SetButtonsEnabled(
                isIncluded, 
                "ActivityExecuteButton", 
                "ActivityExecuteButtonExcluded"
            );
        }
    }
    
    public void ToggleFog(bool on)
    {
        transform.Find("Fog/Fog").gameObject.SetActive(on);
    }
    public void SetLightState(bool isExecuted)
    {
        // Toggle Light under "ActivityExecuteButton"
        var lightObject = transform.Find("ActivityExecuteButton/PushButtonObject/Light")?.gameObject;
        if (lightObject != null)
        {
            lightObject.SetActive(isExecuted);
        }

        // Toggle Light under "ActivityExecuteButtonExcluded"
        var lightObjectExcluded = transform.Find("ActivityExecuteButtonExcluded/PushButtonObject/Light")?.gameObject;
        if (lightObjectExcluded != null)
        {
            lightObjectExcluded.SetActive(isExecuted);
        }
    }
    public void UpdatePushButtonMaterial(bool isExecuted)
    {
        string materialPath = isExecuted ? "Materials/ButtonGreenEmission" : "Materials/ButtonGreen";
        UpdateMaterial("ActivityExecuteButton/PushButtonObject/PushButton", materialPath);
    }
    private void UpdateMaterial(string objectPath, string materialPath)
    {
        var gameObject = transform.Find(objectPath)?.gameObject;
        if (gameObject != null)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = Resources.Load<Material>(materialPath);
            }
        }
    }
    private void ToggleChildObjects(bool isActive, params string[] childPaths)
    {
        foreach (var path in childPaths)
        {
            var childObject = transform.Find(path)?.gameObject;
            if (childObject != null)
            {
                childObject.SetActive(isActive);
            }
        }
    }
    private void SetButtonsEnabled(bool isEnabled, params string[] buttonPaths)
    {
        foreach (var path in buttonPaths)
        {
            var buttonObj = transform.Find(path)?.gameObject;
            if (buttonObj != null)
            {
                var buttonController = buttonObj.GetComponent<ButtonController>();
                buttonController?.SetButtonEnabled(isEnabled);
            }
        }
    }

    public void GlitterBurst(float duration)
    {
        SetGlitterColor(Color.green);
        // Access the particle system
        ParticleSystem glitterSystem = GetGlitterParticleSystem();

        // Save initial values
        float initialStartSpeedMin = glitterSystem.main.startSpeed.constantMin;
        float initialStartSpeedMax = glitterSystem.main.startSpeed.constantMax;
        
        float initialLifetimedMin = glitterSystem.main.startLifetime.constantMin;
        float initialLifetimedMax = glitterSystem.main.startLifetime.constantMax;

        float initialEmissionRate = glitterSystem.emission.rateOverTime.constant;

        // Triple the speed and emission rate
        var mainModule = glitterSystem.main;
        mainModule.startSpeed = 50;
        mainModule.startLifetime = 1000;

        var emissionModule = glitterSystem.emission;
        emissionModule.rateOverTime = 1000;

        // Wait for the duration
        StartCoroutine(ResetAfterDuration(duration, initialStartSpeedMin, initialStartSpeedMax, initialLifetimedMin, initialLifetimedMax, initialEmissionRate));
    }

    private IEnumerator ResetAfterDuration(float duration, float speedMin, float speedMax, float lifetimeMin, float lifetimeMax, float emissionRate)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset to initial values
        var mainModule = GetGlitterParticleSystem().main;
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(speedMin, speedMax);
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(lifetimeMin, lifetimeMax);

        var emissionModule = GetGlitterParticleSystem().emission;
        emissionModule.rateOverTime = emissionRate;
    }

    public void SetGlitterColor(Color newColor)
    {
        ParticleSystem ps = GetGlitterParticleSystem();
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startColor = newColor;
    }
    public ParticleSystem GetGlitterParticleSystem(){
        return transform.Find("Glitter").GetComponent<ParticleSystem>();
    }
    public void SubscribeToOnExecuted(Action<Activity> subscriber){
        _onExecuted+=subscriber;
    }
    public override string ToString()
    {
        return $"Activity ID: {Id}, Label: {Label}";
    }
}
