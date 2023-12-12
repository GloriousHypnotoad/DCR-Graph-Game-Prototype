
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Controller : MonoBehaviour
{
    private View _view;
    private Model _model;
    private string _activeSceneName;
    private ColorGenerator _colorGenerator;
    private Dictionary<string, Color> _activityColors;
    

    private

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

        _activityColors = GenerateColorsHelper();

        // Create Activity objects in the view from data in Model.
        _view.CreateActivities(_model.GetActivityLabels(), _model.GetActivityDescriptions(), _activityColors);
    
        // Once created call private method to set Activity states to initial values.
        UpdateView();
        
    }

    
        // This guy needs to access the list of colors for the string. colors is a Dict of strings and colors.
        // I need to:
        //  1. Get the Conditions list.
        //      i. loop thorugh it
        //      ii. for each condition, get its color from the colors dict you are sending to view.CreateActivities
        //      iii. get its values and assign the values to a dictionary of string hashset of colors.
        //      iv. if the value is already in the dict, just add the color.
        //  2. Do the same for the Responses list
        //  3. You now know the hashset of colors for each object with an incoming condition or a response relation.

    // Method to generate a dictionary of all activities that should have a unique identifying color
    // (initially pending activities, activities that are responses, conditions)
    private Dictionary<string, Color> GenerateColorsHelper(){
        
        Dictionary<string, HashSet<string>> Conditions = _model.GetConditions();
        Dictionary<string, HashSet<string>> Responses = _model.GetResponses();
        HashSet<string> pending = _model.GetPending();
        
        Dictionary<string, Color> activityColors = new Dictionary<string, Color>();

        HashSet<string> rgbs = new HashSet<string>();

        foreach (string id in _model.GetActivityIds())
        {
            activityColors[id] = Color.white;
        }

        foreach (KeyValuePair<string, HashSet<string>> kvp in Conditions)
        {
            rgbs.Add(kvp.Key);
        }

        foreach (string item in pending)
        {
            rgbs.Add(item);
        }

        foreach (KeyValuePair<string, HashSet<string>> kvp in Responses)
        {
            foreach (string value in kvp.Value)
            {
                rgbs.Add(value);                
            }
        }
        Dictionary<string, Color> rgbColors = _colorGenerator.GenerateColors(rgbs);
        foreach (KeyValuePair<string, Color> kvp in rgbColors)
        {
            activityColors[kvp.Key] = kvp.Value;
        }

        return activityColors;
    }
    // Listens to events emitted by the View when an Activity is clicked, then update the Model and the View.
    private void OnActivityExecuted(string activityId)
    {       
        _model.ExecuteActivity(activityId);

        UpdateView();
    }

    // Gets data from the Model, process and forward to the View for rendering.
    private void UpdateView()
    {
        HashSet<string> activities = _model.GetActivityIds();
        HashSet<string> included = _model.GetIncluded();
        HashSet<string> executed = _model.GetExecuted();
        HashSet<string> pending = _model.GetPending();
        
        Dictionary<string, HashSet<string>> activitiesWithActiveConditionsAndOrMilestones = CalculateActivitiesWithActiveConditionsAndOrMilestones(included, executed, pending, new Dictionary<string, HashSet<string>>());

        foreach (string activityId in activities)
        {
            HashSet<Color> colors = new HashSet<Color>();
            
            if(activitiesWithActiveConditionsAndOrMilestones.TryGetValue(activityId, out var conditionsOrMilestones))
            {
                foreach (string activity in conditionsOrMilestones)
                {
                    Color color = _activityColors[activity];
                    colors.Add(color);
                }
            }
            
            _view.SetActivityExecuted(activityId, executed.Contains(activityId));
            _view.SetActivityPending(activityId, pending.Contains(activityId));
            _view.SetActivityDisabled(activityId, colors);
            _view.SetActivityIncluded(activityId, included.Contains(activityId));
        }

        _view.UpdateGlobalEnvironment(pending.Count > 0);
    
        int currentStateIndex = _model.GetHistoryLength() - 1;
        int previousStateIndex = currentStateIndex - 1;
        
        if (previousStateIndex >= 0)
        {
            var currentState = _model.GetStateAt(currentStateIndex);
            var previousState = _model.GetStateAt(previousStateIndex);

            Dictionary<string, HashSet<string>> currentStateHasActiveConditionsAndOrMilestonesDictionary = CalculateActivitiesWithActiveConditionsAndOrMilestones(currentState.Included, currentState.Executed, currentState.Pending, new Dictionary<string, HashSet<string>>());
            Dictionary<string, HashSet<string>> previousStateHasActiveConditionsAndOrMilestonesDictionary = CalculateActivitiesWithActiveConditionsAndOrMilestones(previousState.Included, previousState.Executed, previousState.Pending, new Dictionary<string, HashSet<string>>());
            HashSet<string> currentStateHasActiveConditionsAndOrMilestones = new HashSet<string>(currentStateHasActiveConditionsAndOrMilestonesDictionary.Keys);
            HashSet<string> previousStateHasActiveConditionsAndOrMilestones = new HashSet<string>(previousStateHasActiveConditionsAndOrMilestonesDictionary.Keys);

            var addedToIncluded = currentState.Included.Except(previousState.Included).ToList();
            var removedFromIncluded = previousState.Included.Except(currentState.Included).ToList();

            var addedToPending = currentState.Pending.Except(previousState.Pending).ToList();

            var removedFromDisabledOrMilestones = previousStateHasActiveConditionsAndOrMilestones.Except(currentStateHasActiveConditionsAndOrMilestones).ToList();
            var addedToDisabledOrMilestones = currentStateHasActiveConditionsAndOrMilestones.Except(previousStateHasActiveConditionsAndOrMilestones).ToList();

            // Pass these lists to the view
            UpdateDeltaLists(addedToIncluded, removedFromIncluded, addedToPending, removedFromDisabledOrMilestones, addedToDisabledOrMilestones);

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

    private Dictionary<string, HashSet<string>> CalculateActivitiesWithActiveConditionsAndOrMilestones(HashSet<string> included, HashSet<string> executed, HashSet<string> pending, Dictionary<string, HashSet<string>> dictionary)
    {
       
        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetConditions())
        {
            if (included.Contains(kvp.Key) && !executed.Contains(kvp.Key))
            {
                ValuesToKeysKeyToValue(kvp, dictionary);
            }            
        }
        
        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetMilestones())
        {
            if (included.Contains(kvp.Key) && pending.Contains(kvp.Key))
            {
                ValuesToKeysKeyToValue(kvp, dictionary);
            }            
        }
        return dictionary;
    }

    private void ValuesToKeysKeyToValue(KeyValuePair<string, HashSet<string>> kvp, Dictionary<string, HashSet<string>> target)
    {
            foreach (string value in kvp.Value)
            {
                if (!target.ContainsKey(value))
                {
                    target[value] = new HashSet<string>();
                }
                target[value].Add(kvp.Key);
            }
    }
}