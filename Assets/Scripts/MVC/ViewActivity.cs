using System;
using System.Collections;
using System.IO;
using UnityEngine;
public class ViewActivity : MonoBehaviour
{
    public string Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    public bool Disabled { get; private set; }
    private ParticleSystem _glitterParticleSystem;
    private event Action<ViewActivity> _onExecuted;
    //private string ButtonName = FileStrings.ButtonName;
    //private string GlitterPath = FileStrings.GlitterPath;
    //private string FogPath = FileStrings.FogPath;
    private ButtonController _buttonController;
    private SceneryController _sceneryController;
    private EffectsController _effectsController;
    private ProximityDetector _proximityDetector;
    private event Action<ViewActivity> _mouseOver;
    private event Action<ViewActivity> _mouseExit;
    private event Action<ViewActivity> _mouseDown;

    void Awake(){
        _effectsController = GetComponentInChildren<EffectsController>();
        _proximityDetector = GetComponentInChildren<ProximityDetector>();
        //_glitterParticleSystem = transform.Find(FileStrings.GlitterPath).GetComponent<ParticleSystem>();
        //_glitterParticleSystem = transform.Find("EffectsContainer/DisabledGlitterDisabledFalse").GetComponent<ParticleSystem>();
        _buttonController = GetComponentInChildren<ButtonController>();
        _sceneryController = GetComponentInChildren<SceneryController>();
        _buttonController.SubscribeToOnPressed(OnButtonPressed);

        _proximityDetector.SubscribeToIsTargetNearby(OnPlayerNearButton);
        ActivityDetectionTrigger activityDetectionTrigger = GetComponentInChildren<ActivityDetectionTrigger>();
        activityDetectionTrigger.SubscribeToOnMouseOver(HandleMouseOverActivityDetectionTrigger);
        activityDetectionTrigger.SubscribeToOnMouseExit(HandleMouseExitActivityDetectionTrigger);
        activityDetectionTrigger.SubscribeToOnMouseDown(HandleMouseDownActivityDetectionTrigger);
    }

    private void HandleMouseOverActivityDetectionTrigger()
    {
        _mouseOver?.Invoke(this);
    }

    private void HandleMouseExitActivityDetectionTrigger()
    {
        _mouseExit?.Invoke(this);
    }

    private void HandleMouseDownActivityDetectionTrigger()
    {
        if(Disabled)
        {
            _buttonController.PressButtonRefuse();
        }
        else
        {
            _buttonController.PressButton();
        }
    }
    public void SubscribeToOnMouseOver(Action<ViewActivity> subscriber)
    {
        _mouseOver += subscriber;
    }
    public void SubscribeToOnMouseExit(Action<ViewActivity> subscriber)
    {
        _mouseExit += subscriber;
    }
    public void SubscribeToOnMouseDown(Action<ViewActivity> subscriber)
    {
        _mouseDown += subscriber;
    }

    public void SetProximityDetectorTarget(int targetLayer)
    {
        _proximityDetector.SetTargetLayer(targetLayer);
    }

    private void OnPlayerNearButton(bool playerNearButton)
    {
        _buttonController.TogglePlayerIsNearby(playerNearButton);
    }

    public void Initialize(string id, string label)
    {
        //Child Object Components
        Id = id;
        Label = label;
    }
    public void SetExecuted(bool isExecuted){
        if(isExecuted){
            _effectsController.SwitchParticleColor(Color.green);
        }
        
        _sceneryController.ToggleAnimatedElements(isExecuted);
        
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
            //ButtonController.StartJumping();
        }
        else
        {
            //ButtonController.StopJumping();
        }
    }
    public void SetDisabled(bool isDisabled)
    {
        if (!isDisabled)
        {
            _buttonController.StartRotation();
        }
        else
        {
            _buttonController.StopRotation();
        }

        _effectsController.ToggleFog(isDisabled);
        _effectsController.ToggleGlitter(!isDisabled);


        // ToggleChildObjects(isDisabled, FogPath);
        //ToggleChildObjects(!isDisabled, GlitterPath);
        Disabled = isDisabled;
        //SetButtonsEnabled(!isDisabled);
    }/*
    public void SetHasUnmetMilestones(bool hasUnmetMilestones)
    {
        if(!transform.Find(FogPath).gameObject.activeInHierarchy){

        ToggleChildObjects(hasUnmetMilestones, FogPath);
        ToggleChildObjects(!hasUnmetMilestones, GlitterPath);
        SetButtonsEnabled(!hasUnmetMilestones);
        }
    }
    */
    public void SetIncluded(bool isIncluded)
    {
        _buttonController.SetOpaque(isIncluded);
        _sceneryController.SetOpaque(isIncluded);
        /*
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
        */

        // Handle button states as per disabled logic
        if(!isIncluded)
        {
            _effectsController.ToggleGlitter(false);
            //ToggleChildObjects(isIncluded, GlitterPath);
            Disabled = !isIncluded;
            //SetButtonsEnabled(false);
            _buttonController.StopRotation();
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
    /*
    private void SetButtonsEnabled(bool isEnabled)
    {
        GetComponentInChildren<ButtonController>().SetButtonEnabled(isEnabled);
    }
    */
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
        //GlitterBurst(quickRotationDuration);
        // Display execution
        // Highlight outgoing constraints
    }
    // TODO: Move to separate script.
    /*
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
    */
}
