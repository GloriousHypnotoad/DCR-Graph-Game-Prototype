
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private View _view;
    private Model _model;
    private string _activeSceneName;
    private ColorGenerator _colorGenerator;
    private Dictionary<string, Color> _activityColors;
    private Dictionary<string, HashSet<string>> _activitiesWithActiveConditionsAndOrMilestones;
    private string _sceneName;
    private AudioSource _audioSource;
    private List<AudioClip> _soundEffects;
    

    private

    void Awake()
    {
        GameSettings.SceneName = SceneManager.GetActiveScene().name;
        _sceneName = GameSettings.SceneName;
        _view = GetComponentInChildren<View>();
        _model = transform.Find("Model").gameObject.GetComponent<Model>();
        _colorGenerator = new ColorGenerator();
        _audioSource = transform.GetComponent<AudioSource>();
        _soundEffects = new List<AudioClip>();

        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/bells"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/fence"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/choir-short"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/choir-long"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/0. Call in sick"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/1. Register notification of sickness"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/2. Assign shiftplanner"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/3. Roster exceeds budget"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/4. Roster is within budget"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/5. Inform the manager"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/6. An employee works over-time"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/7. Call in replacement"));
        _soundEffects.Add(Resources.Load<AudioClip>("Sounds/8. Complete process"));

    }

    void Start()
    {
        //Run(Path.Combine(Application.dataPath, FileStrings.GameDataPath, $"{FileStrings.GetActiveSceneName()}{FileStrings.GraphFileExtension}"));
        Run($"{_sceneName}");
    }
    
    public void Run(string selectedGraphName)
    {
        // Listen for executed events from the view
        _view.SubscribeToActivityExecuted(OnActivityExecuted);
        _view.SubscribeToActivityExecuteRefused(OnActivityExecuteRefused);
        _view.SubscribeToActivityLockSelected(OnLockSelected);
        _view.SubscribeToActivitySimulationIsExecuting(OnActivitySimulationIsExecuting);


        // Parse graph XML file -> JSON -> Local data structures.
        _model.ParseXmlFile(selectedGraphName);
        _model.ProcessJsonFile($"{_sceneName}.json");

        _activityColors = GenerateColorsHelper();

        // Create Activity objects in the view from data in Model.
        _view.CreateActivities(_model.GetActivityLabels(), _model.GetActivityDescriptions(), _activityColors);
    
        // Once created call private method to set Activity states to initial values.
        UpdateView();
        
    }

    private void OnActivitySimulationIsExecuting(bool isExecuting)
    {
        if(!isExecuting)
        {
            int simulatedHistoryLength = _model.GetHistoryLength();
            _model.RevertHistoryBackTo(simulatedHistoryLength-2);
            UpdateView();
        }
    }

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
        if(GameSettings.ActiveCamera != CameraMode.BirdsEye)
        {   
            if(activityId == "Activity8")
            {
                _audioSource.clip = _soundEffects[3];
            }
            else {
                _audioSource.clip = _soundEffects[0];
            }
        }
        else
        {
            switch(activityId)
            {
                case "Activity0": _audioSource.clip = _soundEffects[4];
                break;
                case "Activity1": _audioSource.clip = _soundEffects[5];
                break;
                case "Activity2": _audioSource.clip = _soundEffects[6];
                break;
                case "Activity3": _audioSource.clip = _soundEffects[7];
                break;
                case "Activity4": _audioSource.clip = _soundEffects[8];
                break;
                case "Activity5": _audioSource.clip = _soundEffects[9];
                break;
                case "Activity6": _audioSource.clip = _soundEffects[10];
                break;
                case "Activity7": _audioSource.clip = _soundEffects[11];
                break;
                case "Activity8": _audioSource.clip = _soundEffects[12];
                break;
            }
        }
        _audioSource.Play();

        _model.ExecuteActivity(activityId);

        UpdateView();
    }
    
    private void OnActivityExecuteRefused(string activityId)
    {
        if(GameSettings.ActiveCamera != CameraMode.BirdsEye)
        {   
            _audioSource.clip = _soundEffects[1];
        }
        else
        {
            switch(activityId)
            {
                case "Activity0": _audioSource.clip = _soundEffects[4];
                break;
                case "Activity1": _audioSource.clip = _soundEffects[5];
                break;
                case "Activity2": _audioSource.clip = _soundEffects[6];
                break;
                case "Activity3": _audioSource.clip = _soundEffects[7];
                break;
                case "Activity4": _audioSource.clip = _soundEffects[8];
                break;
                case "Activity5": _audioSource.clip = _soundEffects[9];
                break;
                case "Activity6": _audioSource.clip = _soundEffects[10];
                break;
                case "Activity7": _audioSource.clip = _soundEffects[11];
                break;
                case "Activity8": _audioSource.clip = _soundEffects[12];
                break;
            }
        }
        
        _audioSource.Play();
        _model.ExecuteActivity();
        UpdateView();
    }

    private void OnLockSelected(string activityId)
    {
        foreach (KeyValuePair<string, HashSet<string>> kvp in _activitiesWithActiveConditionsAndOrMilestones)
        {
            _view.ClearLockSignal(kvp.Key);

            foreach (string activity in kvp.Value)
            {
                _view.ClearKeySignal(activity);
            }            
        }

        _view.ActivityLockSignal(activityId);

        foreach (string activity in _activitiesWithActiveConditionsAndOrMilestones[activityId])
        {
            _view.ActivityKeySignal(activity);
        }
    }

    // Gets data from the Model, process and forward to the View for rendering.
    private void UpdateView()
    {
        HashSet<string> activities = _model.GetActivityIds();
        HashSet<string> included = _model.GetIncluded();
        HashSet<string> executed = _model.GetExecuted();
        HashSet<string> pending = _model.GetPending();
        
        _activitiesWithActiveConditionsAndOrMilestones = CalculateActivitiesWithActiveConditionsAndOrMilestones(included, executed, pending, new Dictionary<string, HashSet<string>>());

        foreach (string activityId in activities)
        {
            HashSet<Color> colors = new HashSet<Color>();
            
            if(_activitiesWithActiveConditionsAndOrMilestones.TryGetValue(activityId, out var conditionsOrMilestones))
            {
                Debug.Log($"Controller.UpdateView(): {activityId} has the following constraints:");
                foreach (string activity in conditionsOrMilestones)
                {
                    Color color = _activityColors[activity];
                    colors.Add(color);
                    Debug.Log($"{activity} - Color: {color}");
                }
            }
            
            _view.SetActivityExecuted(activityId, executed.Contains(activityId));
            _view.SetActivityPending(activityId, pending.Contains(activityId));
            _view.SetActivityDisabled(activityId, colors);
            _view.SetActivityIncluded(activityId, included.Contains(activityId));
        }

        /*
        bool unMetMilestones = false;
        foreach (KeyValuePair<string, HashSet<string>> kvp in _model.GetMilestones())
        {
            if (included.Contains(kvp.Key) && pending.Contains(kvp.Key))
            {
                foreach (string value in kvp.Value)
                {
                    Debug.Log($"{value} has unmet milestone {kvp.Key}");
                }
                
                unMetMilestones = true;
                break;
            }            
        }

        _view.UpdateGlobalEnvironment(unMetMilestones);
        */

        _view.UpdateGlobalEnvironment(pending.Count() > 0);

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
/*
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
*/        }
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