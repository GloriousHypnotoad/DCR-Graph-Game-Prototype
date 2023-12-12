using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class ViewActivity : MonoBehaviour
{
    public string Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    public Color Color { get; private set; }
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
    internal void Initialize(string id, string label, string description, Color color)
    {
        Id = id;
        Label = label;
        Description = description;
        Color = color;
    }

    internal void SetPending(bool isPending)
    {
        // Toggle lights and effects on the activity scene
        _effectsController.ToggleSceneryLight(isPending);
        _effectsController.ChangeSceneryLightColor(Color);
        _effectsController.TogglePulseOnSceneryLight(isPending);
    }

    /*
    internal void SetDisabled(bool isDisabled)
    {
        // Toggle effects
        _effectsController.ToggleFog(isDisabled);
        _buttonController.ToggleRotation(!isDisabled);

        // Set class variable
        Disabled = isDisabled;
    }
    */

    internal void SetDisabled(HashSet<Color> colors)
    {
        foreach (Color color in colors)
        {
            Debug.Log(color.ToString());
        }
        bool isDisabled = false;
        if (colors.Count != 0){
            isDisabled = true;
            if (colors.Count == 1)
            {
                _buttonController.SetPushButtonColor(colors.First());
                _buttonController.StopPushButtonColorCycle();
            }
            else
            {
                _buttonController.StartPushButtonColorCycle(colors);
            }
        }
        else
        {
            _buttonController.SetPushButtonColor(Color.white);
        }

        // Toggle effects
        _effectsController.ToggleFog(isDisabled);
        _buttonController.ToggleRotation(!isDisabled);

        // Set class variable
        Disabled = isDisabled;
    }

    // Public methods to set the visual state of the Activity
    internal void SetExecuted(bool isExecuted){

        // Toggle Glitter off
        _effectsController.ToggleGlitter(!isExecuted);
        _effectsController.ToggleSceneryLight(isExecuted);
        _effectsController.TogglePulseOnSceneryLight(!isExecuted);
        
        // Toggle animated elements on/off
        _sceneryController.ToggleAnimatedElements(isExecuted);

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

            _buttonController.ToggleRotation(false);
            _sceneryController.SetOpaque(isIncluded);
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

    // Forward MouseOver event to View
    internal void OnActivityMouseOver()
    {
        _effectsController.ToggleGodray(false);
        _activityMouseOver?.Invoke(this);
    }

    // Forward MouseOver event to View
    internal void OnActivityMouseExit()
    {
        _activityMouseExit?.Invoke(this);
    }

    // Forward control to ButtonController On Mouse Down.
    internal void OnActivityMouseDown()
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
    internal void OnPlayerNearButton(bool playerNearButton)
    {
        if(!Disabled){
            _buttonController.TogglePushButtonAnimation(playerNearButton);

        }
    }
    private void OnButtonPressed(float quickRotationDuration)
    {
        _onExecuted?.Invoke(this);
        _effectsController.GlitterBurst(quickRotationDuration);
        // Highlight outgoing constraints
    }

    internal void SetStateChanged()
    {
        _effectsController.ToggleGodray(true);
    }
}