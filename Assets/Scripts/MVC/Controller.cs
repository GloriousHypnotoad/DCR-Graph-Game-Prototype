
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Controller : MonoBehaviour
{
    private View _view;
    private Model _model;
    private string _activeSceneName;

    void Awake()
    {
        _view = GetComponentInChildren<View>();
        _model = transform.Find("Model").gameObject.GetComponent<Model>();
    }

    void Start()
    {
        //Run(Path.Combine(Application.dataPath, FileStrings.GameDataPath, $"{FileStrings.GetActiveSceneName()}{FileStrings.GraphFileExtension}"));
        Run("Abstract.xml");
    }
    
    public void Run(string selectedGraphName)
    {
        
        //_view.SubscribeToOnActivityExecuted(OnActivityExecuted);
        _model.ParseXmlFile(selectedGraphName);
        //_model.ProcessJsonFile($"{FileStrings.GetActiveSceneName()}{FileStrings.JsonExtension}");
        _model.ProcessJsonFile("Abstract.json");

        // Create Activity objects in the view from data in Model.
        _view.CreateActivities(_model.GetActivityLabels());

        // Once created call private method to set Activity states to initial values.
        UpdateView();
        
    }
    /*
    // Listens to events emitted by the View when an Activity is clicked.
    private void OnActivityExecuted(string activityId)
    {       
        // Instruct the Model to make marking changes based on executed activity.
        _model.ExecuteActivity(activityId);

        // Call private method to handle updating the states of the activities in the view.
        UpdateView();
    }
    */
    private void UpdateView()
    {
        HashSet<string> activities = _model.GetActivityIds();
        HashSet<string> executed = _model.GetExecuted();
        HashSet<string> included = _model.GetIncluded();
        HashSet<string> pending = _model.GetPending();
        HashSet<string> disabled = new HashSet<string>();
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
            _view.SetActivityExecuted(activityId, executed.Contains(activityId));
            _view.SetActivityPending(activityId, pending.Contains(activityId));
            _view.SetActivityDisabled(activityId, disabled.Contains(activityId));
            _view.SetActivityHasUnmetMilestones(activityId, haveUnmetMilestones.Contains(activityId));
            _view.SetActivityIncluded(activityId, included.Contains(activityId));
        }

        _view.UpdateGlobalEnvironment(pending.Count > 0);
    }

}