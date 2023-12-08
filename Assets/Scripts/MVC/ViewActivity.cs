using System;
using System.Collections;
using System.IO;
using UnityEngine;
public class ViewActivity : MonoBehaviour
{
    public string Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    private ParticleSystem _glitterParticleSystem;
    //private event Action<ViewActivity> _onExecuted;
    //private string ButtonName = FileStrings.ButtonName;
    private string GlitterPath = FileStrings.GlitterPath;
    private string FogPath = FileStrings.FogPath;
    ButtonController ButtonController;

    void Awake(){
        _glitterParticleSystem = transform.Find(FileStrings.GlitterPath).GetComponent<ParticleSystem>();
        //_glitterParticleSystem = transform.Find("EffectsContainer/DisabledGlitterDisabledFalse").GetComponent<ParticleSystem>();
        ButtonController = GetComponentInChildren<ButtonController>();
        //ButtonController.SubscribeToOnPressed(OnButtonPressed);

        GetComponentInChildren<ProximityDetector>().SubscribeToIsTargetNearby(OnPlayerNearButton);
    }

    public void SetProximityDetectorTarget(int targetLayer)
    {
        transform.GetComponentInChildren<ProximityDetector>().SetTargetLayer(targetLayer);
    }

    private void OnPlayerNearButton(bool playerNearButton)
    {
        ButtonController.TogglePlayerIsNearby(playerNearButton);
    }

    public void Initialize(string id, string label)
    {
        //Child Object Components
        Id = id;
        Label = label;
    }
    public void SetExecuted(bool isExecuted){
        if(isExecuted){
            ParticleSystem ps = _glitterParticleSystem;
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.startColor = Color.green;
        }
        
        string materialPath = isExecuted ? FileStrings.ButtonGreenEmissionPath : FileStrings.ButtonGreenPath;
        UpdateMaterial(FileStrings.PushButtonPath, materialPath);
        
        var gameObject = transform.Find(FileStrings.PushButtonPath).gameObject;
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material = Resources.Load<Material>(materialPath);

        
        ToggleChildObjects(isExecuted,
            FileStrings.LightPath);
    }
    public void SetPending(bool isPending)
    {
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
        ToggleChildObjects(isDisabled, FogPath);
        ToggleChildObjects(!isDisabled, GlitterPath);
        SetButtonsEnabled(!isDisabled);
    }
    public void SetHasUnmetMilestones(bool hasUnmetMilestones)
    {
        if(!transform.Find(FogPath).gameObject.activeInHierarchy){

        ToggleChildObjects(hasUnmetMilestones, FogPath);
        ToggleChildObjects(!hasUnmetMilestones, GlitterPath);
        SetButtonsEnabled(!hasUnmetMilestones);
        }
    }
    public void SetIncluded(bool isIncluded)
    {
        // Enable or disable specified child components
        ToggleChildObjects(
            isIncluded, 
            FileStrings.ButtonPath, 
            FileStrings.SceneryPath
        );

        ToggleChildObjects(
            !isIncluded, 
            FileStrings.ButtonTransparentPath, 
            FileStrings.SceneryTransparentPath
        ); // Also handling "Glitter" in the same manner

        // Handle button states as per disabled logic
        if(!isIncluded)
        {
            ToggleChildObjects(isIncluded, GlitterPath);
            SetButtonsEnabled(isIncluded);
        }
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
    private void SetButtonsEnabled(bool isEnabled)
    {
        GetComponentInChildren<ButtonController>().SetButtonEnabled(isEnabled);
    }
    /*
    // TODO: Move to separate script.
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
    public void SubscribeToOnExecuted(Action<ViewActivity> subscriber){
        _onExecuted+=subscriber;
    }
    public override string ToString()
    {
        return $"Activity ID: {Id}, Label: {Label}";
    }
    public void OnButtonPressed(float quickRotationDuration)
    {
        _onExecuted?.Invoke(this);
        GlitterBurst(quickRotationDuration);
        // Display execution
        // Highlight outgoing constraints
    }
    */
}
