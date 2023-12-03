using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    public GameObject ViewObject;
    private View _view;
    private Model _model;

    void Start()
    {
        // Run the game with the path to the graph to be rendered.
        Run(GameSetupData.SelectedGraphPath);
    }

    public void Run(string fileName)
    {
        _view = ViewObject.GetComponent<View>();
        _view.Initialize();
        _model = new Model();
        _model.ParseXmlFile(fileName);
        _model.ProcessJsonFile("output.json");

        // Create Activity objects in the view from data in Model.
        _view.CreateActivities(
            _model.GetActivityLabels()
        );

        // Once created call private method to set Activity states to initial values.
        UpdateView();

        // Add listeners to run whenever the view registers a click on an activity by the user.
        _view.SubscribeToExecutedActivityReceived(HandleExecutedActivity);
    }

    // Listens to events emitted by the View when an Activity is clicked.
    private void HandleExecutedActivity(string executedActivityId)
    {
        // Instruct the Model to make marking changes based on executed activity.
        _model.ExecuteActivity(executedActivityId);

        // Call private method to handle updating the states of the activities in the view.
        UpdateView();
    }
    private void UpdateView()
    {
        HashSet<string> activities = _model.GetActivityIds();
        HashSet<string> executed = _model.GetExecuted();
        HashSet<string> included = _model.GetIncluded();
        HashSet<string> pending = _model.GetPending();
        HashSet<string> disabled  = new HashSet<string>();
        HashSet<string> haveUnmetMilestones = new HashSet<string>();

        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetConditions())
        {
            if (included.Contains(kvp.Key) && !executed.Contains(kvp.Key))
            {
                disabled.UnionWith(kvp.Value);
            }            
        }

        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetMilestones())
        {
            if (included.Contains(kvp.Key) && pending.Contains(kvp.Key))
            {
                haveUnmetMilestones.UnionWith(kvp.Value);
            }            
        }

        foreach (string activityId in activities)
        {
            GameObject activityObject = _view.Activities[activityId];
            Activity activity = activityObject.GetComponent<Activity>();
            
            bool activityIsExecuted = _model.Executed.Contains(activityId);
            bool activityIsPending = _model.Pending.Contains(activityId);
            bool activityIsIncluded = _model.Included.Contains(activityId);
            bool activityIsDisabled = disabled.Contains(activityId);
            bool activityHasUnmetMilestones = haveUnmetMilestones.Contains(activityId);

            activity.SetLightState(activityIsExecuted);
            activity.UpdatePushButtonMaterial(activityIsExecuted);
            activity.SetPendingState(activityIsPending);
            activity.SetIncludedState(activityIsIncluded);
            activity.SetDisabledOrUnmetMilestonesState(activityIsDisabled || activityHasUnmetMilestones);
        }
        UpdateGlobalEnvironment(pending.Count > 0);
    }

    private void UpdateGlobalEnvironment(bool hasPendingActivities)
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