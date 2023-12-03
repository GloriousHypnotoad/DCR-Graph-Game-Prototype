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
        HashSet<string> hasUnmetMilestones = new HashSet<string>();

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
                hasUnmetMilestones.UnionWith(kvp.Value);
            }            
        }

        _view.UpdateView
        (
            activities,
            executed,
            included,
            pending,
            disabled,
            hasUnmetMilestones
        );
    }
}