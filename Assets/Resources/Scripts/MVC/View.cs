using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    void Awake()
    {
            camera1.enabled = true;
            camera2.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            camera1.enabled = false;
            camera2.enabled = true;
        }
    }
    private Action<string> _onActivityExecuted;
    public Dictionary<string, GameObject> Activities { get; private set; }  = new Dictionary<string, GameObject>();

    public void CreateActivities(
        Dictionary<string, string> idsAndLabels
    )
    {
        //TODO: Create Setup activity scripts
        foreach (KeyValuePair<string, string> kvp in idsAndLabels)
        {
            GameObject activtyObject = GameObject.Find(kvp.Value);
            Activity activity = activtyObject.GetComponent<Activity>();

            activity.Initialize(kvp.Key, kvp.Value);
            
            Activities.Add(kvp.Key, activtyObject);

            activity.SubscribeToOnExecuted(OnExecuted);
        }
    }
    public void SetActivityExecuted(string activityId, bool isExecuted){
        Activities[activityId].GetComponent<Activity>().SetExecuted(isExecuted);
    }
    public void SetActivityPending(string activityId, bool isPending){
        Activities[activityId].GetComponent<Activity>().SetPending(isPending);
    }
    public void SetActivityIncluded(string activityId, bool isIncluded){
        Activities[activityId].GetComponent<Activity>().SetIncluded(isIncluded);
    }
    public void SetActivityDisabled(string activityId, bool isDisabled){
        Activities[activityId].GetComponent<Activity>().SetDisabled(isDisabled);
    }
    public void SetActivityHasUnmetMilestones(string activityId, bool hasUnmetMilestones){
        Activities[activityId].GetComponent<Activity>().SetHasUnmetMilestones(hasUnmetMilestones);
    }
    public void OnExecuted(Activity activity){
        _onActivityExecuted?.Invoke(activity.Id);
    }
    public void SubscribeToOnActivityExecuted(Action<string> subscriber){
        _onActivityExecuted+=subscriber;
    }

    public void UpdateGlobalEnvironment(bool hasPendingActivities)
    {
        GameObject moonlight = GameObject.Find("Moonlight");
        GameObject sunlight = GameObject.Find("Sunlight");

        if (moonlight != null) moonlight.SetActive(hasPendingActivities);
        if (sunlight != null) sunlight.SetActive(!hasPendingActivities);

        string skyboxMaterialPath = hasPendingActivities ? "Skyboxes/FS000_Night_01" : "Skyboxes/FS000_Day_03";
        RenderSettings.skybox = Resources.Load<Material>(skyboxMaterialPath);
        DynamicGI.UpdateEnvironment(); 
    }
}
