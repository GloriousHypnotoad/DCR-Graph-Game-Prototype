
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Controller : MonoBehaviour
{
    private View _view;
    private Model _model;
    private string _activeSceneName;
    private ColorGenerator _colorGenerator;

    void Awake()
    {
        _view = GetComponentInChildren<View>();
        _model = transform.Find("Model").gameObject.GetComponent<Model>();
        _colorGenerator = new ColorGenerator();
    }

    void Start()
    {
        //Run(Path.Combine(Application.dataPath, FileStrings.GameDataPath, $"{FileStrings.GetActiveSceneName()}{FileStrings.GraphFileExtension}"));
        Run("Abstract.xml");
    }
    
    public void Run(string selectedGraphName)
    {
        // Listen for executed events from the view
        _view.SubscribeToActivityExecuted(OnActivityExecuted);

        // Parse graph XML file -> JSON -> Local data structures.
        _model.ParseXmlFile(selectedGraphName);
        _model.ProcessJsonFile("Abstract.json");

        // Create Activity objects in the view from data in Model.
        _view.CreateActivities(_model.GetActivityLabels(), _model.GetActivityDescriptions());

        Dictionary<string, HashSet<string>> Conditions = _model.GetConditions();
        Dictionary<string, HashSet<string>> Responses = _model.GetResponses();
        HashSet<string> pending = _model.GetPending();

        Dictionary<string, Color> colors = new Dictionary<string, Color>();

        foreach (KeyValuePair<string, HashSet<string>> kvp in Conditions)
        {
            colors[kvp.Key] = Color.black;
        }

        foreach (string item in pending)
        {
                if(!colors.ContainsKey(item))
                {
                    colors[item] = Color.black;
                }
        }

        foreach (KeyValuePair<string, HashSet<string>> kvp in Responses)
        {
            foreach (string value in kvp.Value)
            {
                if(!colors.ContainsKey(value))
                {
                    colors[value] = Color.black;
                }
                
            }
        }
        colors = _colorGenerator.GenerateColors(colors);
        foreach (KeyValuePair<string, Color> kvp in colors)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }

        // Once created call private method to set Activity states to initial values.
        UpdateView();
        
    }
    // Listens to events emitted by the View when an Activity is clicked, then update the Model and the View.
    internal void OnActivityExecuted(string activityId)
    {       
        _model.ExecuteActivity(activityId);

        UpdateView();
    }

    // Gets data from the Model, process and forward to the View for rendering.
    internal void UpdateView()
    {
        HashSet<string> activities = _model.GetActivityIds();
        HashSet<string> executed = _model.GetExecuted();
        HashSet<string> included = _model.GetIncluded();
        HashSet<string> pending = _model.GetPending();
        HashSet<string> disabled = CalculateDisabled(included, executed);
        HashSet<string> haveUnmetMilestones = CalculateHaveUnmetMilestones(included, pending);
/*
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
*/
        foreach (string activityId in activities)
        {           
            _view.SetActivityExecuted(activityId, executed.Contains(activityId));
            _view.SetActivityPending(activityId, pending.Contains(activityId));
            _view.SetActivityDisabled(activityId, disabled.Contains(activityId)||haveUnmetMilestones.Contains(activityId));
            _view.SetActivityIncluded(activityId, included.Contains(activityId));
        }

        _view.UpdateGlobalEnvironment(pending.Count > 0);
    
        int currentStateIndex = _model.GetHistoryLength() - 1;
        int previousStateIndex = currentStateIndex - 1;
        
        if (previousStateIndex >= 0)
        {
            var currentState = _model.GetStateAt(currentStateIndex);
            var previousState = _model.GetStateAt(previousStateIndex);

            HashSet<string> currentStatedisabled = CalculateDisabled(currentState.Included, currentState.Executed);
            HashSet<string> previousStatedisabled = CalculateDisabled(previousState.Included, previousState.Executed);

            HashSet<string> currentStateHaveUnmetMilestones = CalculateHaveUnmetMilestones(currentState.Included, currentState.Pending);
            HashSet<string> previousStateHaveUnmetMilestones = CalculateHaveUnmetMilestones(previousState.Included, previousState.Pending);

            var addedToIncluded = currentState.Included.Except(previousState.Included).ToList();
            var removedFromIncluded = previousState.Included.Except(currentState.Included).ToList();

            var addedToPending = currentState.Pending.Except(previousState.Pending).ToList();

            var removedFromDisabled = previousStatedisabled.Except(currentStatedisabled).ToList();

            var addedToHaveUnmetMilestones = currentStateHaveUnmetMilestones.Except(previousStateHaveUnmetMilestones).ToList();
            var removedFromHaveUnmetMilestones = previousStateHaveUnmetMilestones.Except(currentStateHaveUnmetMilestones).ToList();

            // Pass these lists to the view
            UpdateDeltaLists(addedToIncluded, removedFromIncluded, addedToPending, addedToHaveUnmetMilestones, removedFromHaveUnmetMilestones, removedFromDisabled);

            void UpdateDeltaLists(params List<string>[] lists)
            {
                foreach (var list in lists)
                {
                    foreach (var id in list)
                    {
                        _view.SetActivityStateChanged(id);
                    }
                }
            }
        }
    }
    internal HashSet<string> CalculateDisabled(HashSet<string> included, HashSet<string> executed)
    {
        HashSet<string> disabled = new HashSet<string>();

        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetConditions())
        {
            if (included.Contains(kvp.Key) && !executed.Contains(kvp.Key))
            {
                disabled.UnionWith(kvp.Value);
            }            
        }

        return disabled;
    }
    
    internal HashSet<string> CalculateHaveUnmetMilestones(HashSet<string> included, HashSet<string> pending)
    {
        HashSet<string> haveUnmetMilestones = new HashSet<string>();

        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetMilestones())
        {
            if (included.Contains(kvp.Key) && pending.Contains(kvp.Key))
            {
                haveUnmetMilestones.UnionWith(kvp.Value);
            }            
        }

        return haveUnmetMilestones;
    }
}