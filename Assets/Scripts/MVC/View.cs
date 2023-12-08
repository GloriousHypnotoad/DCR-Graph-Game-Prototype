
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class View : MonoBehaviour
{    
    //public Camera FirstPersonCamera;
    //public Camera ThirdPersonCamera;
    //public Camera TopDownCamera;

    void Awake()
    {
        GetComponentInChildren<RaycastController>().SetTargetLayer(LayerMask.NameToLayer("InteractiveElements"));
            //FirstPersonCamera.enabled = true;
            //ThirdPersonCamera.enabled = false;
            //TopDownCamera.enabled = false;
    }
/*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            FirstPersonCamera.enabled = true;
            ThirdPersonCamera.enabled = false;
            TopDownCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            FirstPersonCamera.enabled = false;
            ThirdPersonCamera.enabled = true;
            TopDownCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            FirstPersonCamera.enabled = false;
            ThirdPersonCamera.enabled = false;
            TopDownCamera.enabled = true;
        }
    }
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