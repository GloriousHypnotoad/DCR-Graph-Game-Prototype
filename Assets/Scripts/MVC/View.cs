
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    
    private PlayerObjectController playerObjectController;    
    private CameraController FirstPersonCamera;
    private CameraController ThirdPersonCamera;
    private CameraController TopDownCamera;
    private CameraController BirdsEyeCamera;
    private RawImage _topDownCamFeed;

    void Awake()
    {
        GetComponentInChildren<RaycastController>().SetTargetLayer(LayerMask.NameToLayer("InteractiveElements"));

        playerObjectController = GetComponentInChildren<PlayerObjectController>();
        
        FirstPersonCamera = transform.Find("PlayerObject/FirstPersonCamera").GetComponent<CameraController>();
        ThirdPersonCamera = transform.Find("PlayerObject/ThirdPersonCamera").GetComponent<CameraController>();
        TopDownCamera = transform.Find("PlayerObject/TopDownCamera").GetComponent<CameraController>();
        BirdsEyeCamera = transform.Find("BirdsEyeCamera").GetComponent<CameraController>();

        _topDownCamFeed = transform.GetComponentInChildren<RawImage>();
        
        FirstPersonCamera.SetCameraMode(CameraMode.FirstPerson);
        ThirdPersonCamera.SetCameraMode(CameraMode.ThirdPerson);
        TopDownCamera.SetCameraMode(CameraMode.TopDown);
        BirdsEyeCamera.SetCameraMode(CameraMode.BirdsEye);

        SetCamerasAreEnabled(new bool[]{true, false, true, false});
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SetCamerasAreEnabled(new bool[]{true, false, true, false});
            playerObjectController.SetCameraMode(CameraMode.FirstPerson);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SetCamerasAreEnabled(new bool[]{false, true, true, false});
            playerObjectController.SetCameraMode(CameraMode.ThirdPerson);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SetCamerasAreEnabled(new bool[]{false, false, false, true});
            playerObjectController.SetCameraMode(CameraMode.BirdsEye);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            _topDownCamFeed.gameObject.SetActive(!_topDownCamFeed.gameObject.activeSelf);
        }
    }

    private void SetCamerasAreEnabled(bool[] isEnabled)
    {
        FirstPersonCamera.gameObject.SetActive(isEnabled[0]);
        ThirdPersonCamera.gameObject.SetActive(isEnabled[1]);
        _topDownCamFeed.gameObject.SetActive(isEnabled[2]);
        BirdsEyeCamera.gameObject.SetActive(isEnabled[3]);
    }
/*
    private event Action<string> _onActivityExecuted;
*/
    public Dictionary<string, GameObject> Activities { get; private set; }  = new Dictionary<string, GameObject>();
    public void CreateActivities(Dictionary<string, string> idsAndLabels)
    {
        //TODO: Create Setup activity scripts
        foreach (KeyValuePair<string, string> kvp in idsAndLabels)
        {
            GameObject activtyObject = transform.Find("ViewActivityContainer").Find(kvp.Value).gameObject;
            ViewActivity activity = activtyObject.GetComponent<ViewActivity>();

            activity.Initialize(kvp.Key, kvp.Value);
            Activities.Add(kvp.Key, activtyObject);

            //activity.SubscribeToOnExecuted(OnExecuted);
            activity.SetProximityDetectorTarget(transform.Find("PlayerObject/PlayerBody").gameObject.layer);
        }
    }
    public void SetActivityExecuted(string activityId, bool isExecuted){
        Activities[activityId].GetComponent<ViewActivity>().SetExecuted(isExecuted);
    }
    public void SetActivityPending(string activityId, bool isPending){
        Activities[activityId].GetComponent<ViewActivity>().SetPending(isPending);
    }
    public void SetActivityDisabled(string activityId, bool isDisabled){
        Activities[activityId].GetComponent<ViewActivity>().SetDisabled(isDisabled);
    }
    public void SetActivityHasUnmetMilestones(string activityId, bool hasUnmetMilestones){
        Activities[activityId].GetComponent<ViewActivity>().SetHasUnmetMilestones(hasUnmetMilestones);
    }
    public void SetActivityIncluded(string activityId, bool isIncluded){
        Activities[activityId].GetComponent<ViewActivity>().SetIncluded(isIncluded);
    }
    /*
    public void OnExecuted(ViewActivity activity){
        _onActivityExecuted?.Invoke(activity.Id);
    }
    public void SubscribeToOnActivityExecuted(Action<string> subscriber){
        _onActivityExecuted+=subscriber;
    }
*/

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
}