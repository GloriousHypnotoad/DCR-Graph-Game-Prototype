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
    private event Action<ViewActivity> _pressButtonRefused;
    private event Action<bool> _simulatedExecution;
    private HashSet<Color> _disabledColors;
    private bool _cursorIsOneActivity;

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
        _activityDetectionTrigger.SubscribeToOnSimulatedMouseOver(OnActivitySimulatedMouseOver);
        _activityDetectionTrigger.SubscribeToOnSimulatedMouseDown(OnActivitySimulatedMouseDown);
        _activityDetectionTrigger.SubscribeToOnSimulatedMouseExit(OnActivitySimulatedMouseExit);

        _cursorIsOneActivity = false;
    }

    private void OnActivitySimulatedMouseOver()
    {
        if (!_cursorIsOneActivity)
        {
            _cursorIsOneActivity = true;
            _simulatedExecution?.Invoke(true);
            OnActivityMouseOver();
            OnActivityMouseDown();
        }
    }

    private void OnActivitySimulatedMouseDown()
    {
    }

    private void OnActivitySimulatedMouseExit()
    {
        if (_cursorIsOneActivity)
        {
            _cursorIsOneActivity = false;
            _simulatedExecution?.Invoke(false);

        }
    }

    // Initialize object with activity data from graph
    internal void Initialize(string id, string label, string description, Color color)
    {
        Id = id;
        Label = label;
        Description = description;
        Color = color;
    }

    // Public methods to set the visual state of the Activity
    internal void SetExecuted(bool isExecuted){

        // Toggle Glitter off
        if(isExecuted)
        {
            _buttonController.SetPushButtonColor(Color.green);
            _effectsController.ChangeGlitterColor(Color.white);
        }

        _effectsController.ToggleGlitter(!isExecuted);
        _effectsController.ToggleSceneryLight(isExecuted);       
        
        // Toggle animated elements on/off
        _sceneryController.ToggleAnimatedElements(isExecuted);

    }

    internal void SetPending(bool isPending)
    {
        _effectsController.ToggleSceneryLight(isPending);
        _effectsController.ToggleGlitter(isPending);
        
        if(isPending){
            _effectsController.ChangeSceneryLightColor(Color);
            _effectsController.TogglePulseOnSceneryLight(true);
        }   
    }

    internal void SetDisabled(HashSet<Color> colors)
    {
        _disabledColors = colors;

        bool isDisabled = false;
        
        if (_disabledColors.Count != 0){
            isDisabled = true;
            if (_disabledColors.Count == 1)
            {
                _buttonController.StopPushButtonColorCycle();
                _buttonController.SetPushButtonColor(_disabledColors.First());
            }
            else
            {
                _buttonController.StartPushButtonColorCycle(_disabledColors);
            }
        }
        else
        {
            _buttonController.SetPushButtonColor(Color);
        }

        // Toggle effects
        _effectsController.ToggleFog(isDisabled);
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

            _buttonController.ToggleRotation(false);
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
    internal void SubscribeToPressButtonRefused(Action<ViewActivity> subscriber)
    {
        _pressButtonRefused += subscriber;
    }
    internal void SubscribeToSimulatedExecution(Action<bool> subscriber)
    {
        _simulatedExecution += subscriber;
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
            _pressButtonRefused?.Invoke(this);
            /*
            if (_disabledColors.Count != 0){
                if (_disabledColors.Count == 1)
                {
                    _buttonController.SetPushButtonColor(_disabledColors.First());
                    _buttonController.StopPushButtonColorCycle();
                }
                else
                {
                    _buttonController.StartPushButtonColorCycle(_disabledColors);
                }
            }
            else
            {
                _buttonController.SetPushButtonColor(Color.white);
            }*/
            _effectsController.ToggleGlitter(true);
            _effectsController.SetGlitterRate(50f);
            _effectsController.StartGlitterColorCycle(_disabledColors);
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

    internal void Signal()
    {
        _effectsController.StopGlitterColorCycle();
        _effectsController.ToggleGlitter(true);
        _effectsController.SetGlitterRate(50f);
        _effectsController.ChangeGlitterColor(Color);
    }

    internal void ResetSignal()
    {
        _effectsController.ResetGlitterRate();
        _effectsController.ChangeGlitterColor(Color.white);
    }
}