
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    
    private PlayerObjectController _playerObjectController;    
    private CameraController _firstPersonCamera;
    private CameraController _thirdPersonCamera;
    private CameraController _topDownCamera;
    private CameraController _birdsEyeCamera;
    private RawImage _topDownCamFeed;
    private Image _reticule;
    private event Action<string> _activityExecuted;
    private Dictionary<string, GameObject> _activities = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Controller for moving the player avatar
        _playerObjectController = GetComponentInChildren<PlayerObjectController>();
        
        // Controllers for all four cameras in the game.
        _firstPersonCamera = transform.Find("PlayerObject/FirstPersonCamera").GetComponent<CameraController>();
        _thirdPersonCamera = transform.Find("PlayerObject/ThirdPersonCamera").GetComponent<CameraController>();
        _topDownCamera = transform.Find("PlayerObject/TopDownCamera").GetComponent<CameraController>();
        _birdsEyeCamera = transform.Find("BirdsEyeCamera").GetComponent<CameraController>();

        // UI elements (Top down camera and targeting reticule for first person)
        _topDownCamFeed = transform.GetComponentInChildren<RawImage>();
        _reticule = GetComponentInChildren<Image>();
        
        // Configure Camera controllers for individual purposes
        _firstPersonCamera.SetCameraMode(CameraMode.FirstPerson);
        _thirdPersonCamera.SetCameraMode(CameraMode.ThirdPerson);
        _topDownCamera.SetCameraMode(CameraMode.TopDown);
        _birdsEyeCamera.SetCameraMode(CameraMode.BirdsEye);

        // Initally set player controls for first person
        _playerObjectController.SetCameraMode(CameraMode.FirstPerson);

        // Enable first person camera
        SetCamerasAreEnabled(new bool[]{true, false, true, false});
    }

    void Start()
    {
        // Remove cursor from initial first person mode
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Configurations for each camera mode
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SetCamerasAreEnabled(new bool[]{true, false, true, false});
            _playerObjectController.SetCameraMode(CameraMode.FirstPerson);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SetCamerasAreEnabled(new bool[]{false, true, true, false});
            _playerObjectController.SetCameraMode(CameraMode.ThirdPerson);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SetCamerasAreEnabled(new bool[]{false, false, false, true});
            _playerObjectController.SetCameraMode(CameraMode.BirdsEye);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            // Toggle topdown feed on/off
            _topDownCamFeed.gameObject.SetActive(!_topDownCamFeed.gameObject.activeSelf);
        }
    }

    // Create individual activities in View
    public void CreateActivities(Dictionary<string, string> idsAndLabels, Dictionary<string, string> descriptions)
    {
        
        foreach (KeyValuePair<string, string> kvp in idsAndLabels)
        {
            // Get pre-configured activity object from game space
            GameObject activtyObject = transform.Find("ViewActivityContainer").Find(kvp.Value).gameObject;
            ViewActivity activity = activtyObject.GetComponent<ViewActivity>();

            // Pass ID and label to to game object script
            activity.Initialize(kvp.Key, kvp.Value);
            activity.SetDescription(descriptions[kvp.Key]);
            _activities.Add(kvp.Key, activtyObject);

            // Subscribe to user input events handled by the Activity
            activity.SubscribeToActivityMouseOver(OnActivityMouseOver);
            activity.SubscribeToActivityMouseExit(OnActivityMouseExit);
            activity.SubscribeToOnExecuted(OnActivityExecuted);

            // Configure proximity detector for player object.
            activity.SetProximityDetectorTarget(transform.Find("PlayerObject/PlayerBody").gameObject.layer);
        }
    }

    // Methods for passing state data to Activities
    public void SetActivityExecuted(string activityId, bool isExecuted){
        _activities[activityId].GetComponent<ViewActivity>().SetExecuted(isExecuted);
    }
    public void SetActivityPending(string activityId, bool isPending){
        _activities[activityId].GetComponent<ViewActivity>().SetPending(isPending);
    }
    public void SetActivityDisabled(string activityId, bool isDisabled){
        _activities[activityId].GetComponent<ViewActivity>().SetDisabled(isDisabled);
    }
    public void SetActivityIncluded(string activityId, bool isIncluded){
        _activities[activityId].GetComponent<ViewActivity>().SetIncluded(isIncluded);
    }

    // Method for updating the global lighting when activities are pending or milestones are unmet
    public void UpdateGlobalEnvironment(bool hasPendingActivities)
    {
        string skyBoxesPath = FileStrings.SkyBoxesPath;
        string environmentLightsPath = FileStrings.EnvironmentLightsPath;
            
        string skybox = hasPendingActivities ? Path.Combine(skyBoxesPath, FileStrings.SkyBoxPendingName) : Path.Combine(skyBoxesPath, FileStrings.SkyBoxAllClearName);
        string sun = hasPendingActivities ? Path.Combine(environmentLightsPath, FileStrings.EnvironmentLightsPendingName) : Path.Combine(environmentLightsPath, FileStrings.EnvironmentLightsAllClearName);
        
        RenderSettings.skybox = Resources.Load<Material>(skybox);
        RenderSettings.sun = Resources.Load<Light>(sun);
        DynamicGI.UpdateEnvironment(); 
    }
    
    // Method for subscribing to Activity ID
    public void SubscribeToActivityExecuted(Action<string> subscriber){
        _activityExecuted+=subscriber;
    }

    // Helper method for toggling cameras on/off
    private void SetCamerasAreEnabled(bool[] isEnabled)
    {
        _firstPersonCamera.gameObject.SetActive(isEnabled[0]);
        _thirdPersonCamera.gameObject.SetActive(isEnabled[1]);
        _topDownCamFeed.gameObject.SetActive(isEnabled[2]);
        _birdsEyeCamera.gameObject.SetActive(isEnabled[3]);
    }

    // Event handlers
    private void OnActivityMouseOver(ViewActivity activity)
    {
        if (!activity.Disabled)
        {
            _reticule.color = Color.green;
        }
        else
        {
            _reticule.color = Color.red;
        }
    }
    private void OnActivityMouseExit(ViewActivity activity){
        _reticule.color = Color.white;
    }

    // Method for passing on Activity ID to the Controller when Activity executes
    private void OnActivityExecuted(ViewActivity activity){
        _activityExecuted?.Invoke(activity.Id);
    }
}