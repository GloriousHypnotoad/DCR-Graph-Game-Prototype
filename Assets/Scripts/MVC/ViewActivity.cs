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

    private ButtonController _buttonController;
    private EffectsController _effectsController;
    private ProximityDetector _proximityDetector;
    private SceneryController _sceneryController;
    private ActivityDetectionTrigger _activityDetectionTrigger;

    private event Action<ViewActivity> _activityMouseOver;
    private event Action<ViewActivity> _activityMouseExit;
    private event Action<ViewActivity> _onExecuted;

    void Awake()
    {
        // Sub component controllers
        _buttonController = GetComponentInChildren<ButtonController>();
        _effectsController = GetComponentInChildren<EffectsController>();
        _proximityDetector = GetComponentInChildren<ProximityDetector>();
        _sceneryController = GetComponentInChildren<SceneryController>();
        _activityDetectionTrigger = GetComponentInChildren<ActivityDetectionTrigger>();

        // Subscribe to sub component events
        _buttonController.SubscribeToOnPressed(OnButtonPressed);
        _proximityDetector.SubscribeToIsTargetNearby(OnPlayerNearButton);
        _activityDetectionTrigger.SubscribeToOnMouseOver(OnActivityMouseOver);
        _activityDetectionTrigger.SubscribeToOnMouseExit(OnActivityMouseExit);
        _activityDetectionTrigger.SubscribeToOnMouseDown(OnActivityMouseDown);
    }

    // Initialize object with activity data from graph
    public void Initialize(string id, string label)
    {
        Id = id;
        Label = label;
    }

    // Public methods to set the visual state of the Activity
    public void SetExecuted(bool isExecuted){

        if(isExecuted)
        {
            _effectsController.SwitchParticleColor(Color.green);
        }
        
        // Toggle animated elements on/off
        _sceneryController.ToggleAnimatedElements(isExecuted);
        
        // Update button material to reflect executed status
        string buttonMaterial = isExecuted ? FileStrings.ButtonGreenEmissionPath : FileStrings.ButtonGreenPath;
        transform.Find(FileStrings.PushButtonPath).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>(buttonMaterial);

        // Update button Light to reflect executed status
        _effectsController.TogglePushButtonLight(isExecuted);
    }

    public void SetPending(bool isPending)
    {
        // Toggle light on the activity scene
        _effectsController.ToggleSceneryLight(isPending);
        _effectsController.ToggleFireworks(isPending);

        /*
        if (isPending)
        {
            ButtonController.StartJumping();
            
        }
        else
        {
            ButtonController.StopJumping();
        }
        */
    }

    public void SetDisabled(bool isDisabled)
    {
        // Toggle effects
        _effectsController.ToggleFog(isDisabled);
        _effectsController.ToggleGlitter(!isDisabled);
        _buttonController.ToggleRotation(!isDisabled);

        // Set class variable
        Disabled = isDisabled;

    }

    public void SetIncluded(bool isIncluded)
    {
        // TODO: Toggle pushbutton material dynamically
        _buttonController.SetOpaque(isIncluded);
        _sceneryController.SetOpaque(isIncluded);

        if(!isIncluded)
        {
            Disabled = true;
            _effectsController.ToggleGlitter(false);
            _buttonController.ToggleRotation(false);

            // Update button material to reflect executed status
            string buttonMaterial = FileStrings.TransparentPath;
            transform.Find(FileStrings.PushButtonPath).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>(buttonMaterial);
        }
    }
    // Allow subscribtion to Activity mouse events
    public void SubscribeToActivityMouseOver(Action<ViewActivity> subscriber)
    {
        _activityMouseOver += subscriber;
    }
    public void SubscribeToActivityMouseExit(Action<ViewActivity> subscriber)
    {
        _activityMouseExit += subscriber;
    }

    // Forwards configuration data to Proximity Detector.
    public void SetProximityDetectorTarget(int targetLayer)
    {
        _proximityDetector.SetTargetLayer(targetLayer);
    }

    // Forward MouseOver event to View
    private void OnActivityMouseOver()
    {
        _activityMouseOver?.Invoke(this);
    }

    // Forward MouseOver event to View
    private void OnActivityMouseExit()
    {
        _activityMouseExit?.Invoke(this);
    }

    // Forward control to ButtonController On Mouse Down.
    private void OnActivityMouseDown()
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

    // Inform Button Controller on event from Proximity Detector
    private void OnPlayerNearButton(bool playerNearButton)
    {
        _buttonController.TogglePlayerIsNearby(playerNearButton);
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
