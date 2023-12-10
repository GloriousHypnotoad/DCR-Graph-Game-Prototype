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
    internal void Initialize(string id, string label)
    {
        Id = id;
        Label = label;
    }

    // Public methods to set the visual state of the Activity
    internal void SetExecuted(bool isExecuted){

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

    internal void SetPending(bool isPending)
    {
        // Toggle lights and effects on the activity scene
        _effectsController.ToggleSceneryLight(isPending);
        _effectsController.ToggleFireworks(isPending);
    }

    internal void SetDisabled(bool isDisabled)
    {
        // Toggle effects
        _effectsController.ToggleFog(isDisabled);
        _effectsController.ToggleGlitter(!isDisabled);
        _buttonController.ToggleRotation(!isDisabled);

        // Set class variable
        Disabled = isDisabled;

    }

    internal void SetIncluded(bool isIncluded)
    {
        // Set opacity to reflect inclusion.
        _buttonController.SetOpaque(isIncluded);
        _sceneryController.SetOpaque(isIncluded);

        if(!isIncluded)
        {
            // Disable activity if excluded.
            Disabled = true;
            _effectsController.ToggleGlitter(false);
            _buttonController.ToggleRotation(false);

            // Update button material to reflect executed status
            string buttonMaterial = FileStrings.TransparentPath;
            transform.Find(FileStrings.PushButtonPath).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>(buttonMaterial);
        }
    }
    // Allow subscribtion to Activity mouse events
    internal void SubscribeToActivityMouseOver(Action<ViewActivity> subscriber)
    {
        _activityMouseOver += subscriber;
    }
    internal void SubscribeToActivityMouseExit(Action<ViewActivity> subscriber)
    {
        _activityMouseExit += subscriber;
    }

    // Forwards configuration data to Proximity Detector.
    internal void SetProximityDetectorTarget(int targetLayer)
    {
        _proximityDetector.SetTargetLayer(targetLayer);
    }

    // Allows View to subscribe to activity execution event
    internal void SubscribeToOnExecuted(Action<ViewActivity> subscriber){
        _onExecuted+=subscriber;
    }

    internal void SetDescription(string description)
    {
        Description = description;
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
    private void OnButtonPressed(float quickRotationDuration)
    {
        _onExecuted?.Invoke(this);
        _effectsController.GlitterBurst(quickRotationDuration);
        // Highlight outgoing constraints
    }
}
