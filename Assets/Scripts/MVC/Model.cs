
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
public class Model : MonoBehaviour
{
        
    // Activity Constants
    public HashSet<string> ActivityIds { get; private set; } = new HashSet<string>();
    public Dictionary<string, Vector2> ActivityLocations { get; private set; } = new Dictionary<string, Vector2>();
    public Dictionary<string, string> ActivityDescriptions { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, string> ActivityPurposes { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, string> ActivityLabels { get; private set; } = new Dictionary<string, string>();

    // Activity Variables
    public HashSet<string> Executed { get; private set; } = new HashSet<string>();
    public HashSet<string> Included { get; private set; } = new HashSet<string>();
    public HashSet<string> Pending { get; private set; } = new HashSet<string>();

    // Constraints
    public Dictionary<string, HashSet<string>> Conditions { get; private set; } = new Dictionary<string, HashSet<string>>();
    public Dictionary<string, HashSet<string>> Responses { get; private set; } = new Dictionary<string, HashSet<string>>();
    public Dictionary<string, HashSet<string>> Excludes { get; private set; } = new Dictionary<string, HashSet<string>>();
    public Dictionary<string, HashSet<string>> Includes { get; private set; } = new Dictionary<string, HashSet<string>>();
    public Dictionary<string, HashSet<string>> Milestones { get; private set; } = new Dictionary<string, HashSet<string>>();

    private List<ModelState> history = new List<ModelState>();
    
    public void ParseXmlFile(string fileName)
    {
        // Read the XML file into a string
        string xmlContent = File.ReadAllText(Path.Combine(Application.dataPath, "GameData", fileName));

        // Load the XML content into an XmlDocument
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlContent);

        // Convert the XML to JSON
        string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        // Define the path for the output file
        string outputPath = Path.Combine(Application.persistentDataPath, "Abstract.json");

        // Save the JSON to a file
        File.WriteAllText(outputPath, jsonText);

        #if UNITY_EDITOR
        // Save to the Assets/Resources folder (for development use)
        string resourcesFolderPath = Path.Combine(Application.dataPath, "GameData");
        if (!Directory.Exists(resourcesFolderPath))
        {
            Directory.CreateDirectory(resourcesFolderPath);
        }
        string resourcesPath = Path.Combine(resourcesFolderPath, "Abstract.json");
        File.WriteAllText(resourcesPath, jsonText);
        #endif
    }
    
    public void ProcessJsonFile(string filePath)
    {
        string jsonText = "";
        JObject jsonObject = new JObject();
        JToken resources = new JObject();
        JToken constraints = new JObject();
        JToken marking = new JObject();

        try
        {
            // Create JSON objects
            jsonText = File.ReadAllText(Application.persistentDataPath + $"/{filePath}");
            jsonObject = JObject.Parse(jsonText);
        }
        catch (JsonReaderException jsonEx)
        {
            Debug.Log($"JSON parsing error: {jsonEx.Message}");
        }
        catch (FileNotFoundException fnfEx)
        {
            // Handle the case where the file is not found
            Debug.Log($"File not found: {fnfEx.Message}\n");
        }
        catch (DirectoryNotFoundException dnfEx)
        {
            // Handle the case where the directory is not found
            Debug.Log($"Directory not found: {dnfEx.Message}\n");
        }
        catch (IOException ioEx)
        {
            // Handle other I/O errors
            Debug.Log($"I/O error: {ioEx.Message}\n");
        }
        catch (Exception ex)
        {
            // Handle any other unforeseen errors
            Debug.Log($"An error occurred: {ex.Message}\n");
        }
        try
        {
            resources = jsonObject["dcrgraph"]["specification"]["resources"];
            // ... Additional processing for 'resources'
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($"{e.Message}: The JSON file contains no resources fields");
        }
        try
        {
            constraints = jsonObject["dcrgraph"]["specification"]["constraints"];
        }
        catch (InvalidOperationException e)
        {
            
            Debug.Log($" {e.Message}: The JSON file contains no constraints fields\n");
        }
        try
        {
            marking = jsonObject["dcrgraph"]["runtime"]["marking"];
        }
        catch (InvalidOperationException e)
        {
            
            Debug.Log($" {e.Message}: The JSON file contains no marking fields\n");
        }

        // Process events
        try
        {
            JToken events = resources["events"]["event"];
            foreach (var evt in events)
            {
                string id = evt["@id"].ToString();
                int x = (int) evt["custom"]["visualization"]["location"]["@xLoc"];
                int y = (int) evt["custom"]["visualization"]["location"]["@yLoc"];
                string eventDescription = (string) evt["custom"]["eventDescription"];
                string purpose = (string) evt["custom"]["purpose"];

                ActivityIds.Add(id);
                
                if (!ActivityLocations.TryGetValue(id, out _)){
                    ActivityLocations.Add(id, new Vector2(x, y));
                };
                
                if (!ActivityDescriptions.TryGetValue(id, out _)){
                    if(eventDescription != null)
                    {
                        if (eventDescription.Length > 0)
                        {
                            ActivityDescriptions.Add(id, RemoveHtmlTags(eventDescription));
                        }
                        else
                        {
                            ActivityDescriptions.Add(id, "");
                        }
                    }
                    else
                    {
                        ActivityDescriptions.Add(id, "");
                    }
                };
                
                if (!ActivityPurposes.TryGetValue(id, out _)){
                    ActivityPurposes.Add(id, purpose);
                };
            }
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no events\n");
        }
    
        // Get label Mappings
        try
        {
            JToken labelMappings = resources["labelMappings"]["labelMapping"];

            foreach (var labelMapping in labelMappings)
            {
                string eventId = labelMapping["@eventId"].ToString();
                string label = labelMapping["@labelId"].ToString();

                if (!ActivityLabels.TryGetValue(eventId, out _)){
                    ActivityLabels.Add(eventId, label);
                }
            }
        }
        catch (InvalidOperationException e)
        {
            
            Debug.Log($" {e.Message}: The JSON file contains no mappings\n");
        }
        // Map conditions
        try
        {
            JToken conditions = constraints["conditions"]["condition"];
            CreateConstraints(Conditions, conditions);

        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no conditions\n");
        }

        // Create responses
        try
        {   
            JToken responses = constraints["responses"]["response"];
            CreateConstraints(Responses, responses);

        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no responses\n");
        }

        // Create excludes
        try
        {
            JToken excludes = constraints["excludes"]["exclude"];
            CreateConstraints(Excludes, excludes);
        }
        catch (InvalidOperationException e)
        {  
            Debug.Log($" {e.Message}: The JSON file contains no excludes\n");
        }

        // Create includes
        try
        {
            JToken includes = constraints["includes"]["include"];
            CreateConstraints(Includes, includes);
        }
        catch (InvalidOperationException e)
        {  
            Debug.Log($" {e.Message}: The JSON file contains no includes\n");
        }

        // Create milestones
        try
        {
            JToken milestones = constraints["milestones"]["milestone"];
            CreateConstraints(Milestones, milestones);
        }
        catch (InvalidOperationException e)
        {  
            Debug.Log($" {e.Message}: The JSON file contains no milestones\n");
        }

        // Create initial executed markings
        try
        {
            JToken executedEvents = marking["executed"]["event"];
            CreateMarkings(Executed, executedEvents);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no executed events\n");
        }

        // Create initial included markings
        try
        {
            JToken includedEvents = marking["included"]["event"];
            CreateMarkings(Included, includedEvents);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no included events\n");
        }

        // Create initial pending markings
        try
        {
            JToken pendingResponses = marking["pendingResponses"]["event"];
            CreateMarkings(Pending, pendingResponses);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($" {e.Message}: The JSON file contains no pending events\n");
        }

        history.Add(new ModelState(GetExecuted(), GetIncluded(), GetPending()));

    }
    public Dictionary<string, string> GetActivityLabels()
    {
        return CloneDictionary(ActivityLabels);
    }
    public HashSet<string> GetActivityIds()
    {
        return CloneHashSet(ActivityIds);
    }

    public Dictionary<string, Vector2> GetActivityLocations()
    {
        return CloneDictionary(ActivityLocations);
    }

    public Dictionary<string, string> GetActivityDescriptions()
    {
        return CloneDictionary(ActivityDescriptions);
    }

    public Dictionary<string, string> GetActivityPurposes()
    {
        return CloneDictionary(ActivityPurposes);
    }

    public HashSet<string> GetExecuted()
    {
        return CloneHashSet(Executed);
    }

    public HashSet<string> GetIncluded()
    {
        return CloneHashSet(Included);
    }

    public HashSet<string> GetPending()
    {
        return CloneHashSet(Pending);
    }

    public Dictionary<string, HashSet<string>> GetMilestones()
    {
        return CloneDictionary(Milestones);
    }

    public Dictionary<string, HashSet<string>> GetConditions()
    {
        return CloneDictionary(Conditions);
    }

    public Dictionary<string, HashSet<string>> GetResponses()
    {
        return CloneDictionary(Responses);
    }

    public Dictionary<string, HashSet<string>> GetExcludes()
    {
        return CloneDictionary(Excludes);
    }

    public Dictionary<string, HashSet<string>> GetIncludes()
    {
        return CloneDictionary(Includes);
    }
    
    public void ExecuteActivity(string clickedActivityId)
    {
        // Mark Activity as Executed and remove any Pending markings
        Executed.Add(clickedActivityId);
        Pending.Remove(clickedActivityId);

        // Add Pending markings to any Activities affected by Response constraints.
        if (Responses.TryGetValue(clickedActivityId, out HashSet<string> responseActivities))
        {
            foreach (string activity in responseActivities)
            {
                Pending.Add(activity);            
            }
        }

        // Add Excluded markings to any Activities affected by Excludes constraints.
        if (Excludes.TryGetValue(clickedActivityId, out HashSet<string> excludeActivities))
        {
            foreach (string activity in excludeActivities)
            {
                Included.Remove(activity);            
            }
        }

        // Add Included markings to any Activities affected by Includes constraints.
        if (Includes.TryGetValue(clickedActivityId, out HashSet<string> includeActivities))
        {
            foreach (string activity in includeActivities)
            {
                Included.Add(activity);
            }
        }
        history.Add(new ModelState(GetExecuted(), GetIncluded(), GetPending()));
    }

    // Get specific state in history
    public ModelState GetStateAt(int index)
    {
        return history[index];
    }
    public int GetHistoryLength()
    {
        return history.Count;
    }

    // Helper methods
    internal void CreateConstraints(Dictionary<string, HashSet<string>> constraintsDictionary, JToken constraints)
    {
        if (constraints != null)
        {
            if (constraints.Type == JTokenType.Array)
            {
                foreach (var pair in constraints)
                {
                    string sourceId = pair["@sourceId"].ToString();
                    string targetId = pair["@targetId"].ToString();

                    InsertValuesIntoDictionary(constraintsDictionary, sourceId, targetId);
                }
            }
            else if (constraints.Type == JTokenType.Object)
            {
                string sourceId = constraints["@sourceId"].ToString();
                string targetId = constraints["@targetId"].ToString();

                InsertValuesIntoDictionary(constraintsDictionary, sourceId, targetId);
            }
        }
    }
    internal void CreateMarkings(HashSet<string> markingsHashSet, JToken markings)
    {
        if (markings.Type == JTokenType.Array)
        {
            foreach (var marking in markings)
            {
                InsertValueIntoHashSet(markingsHashSet, marking["@id"].ToString());
            }
        }
        else if(markings.Type == JTokenType.Object)
        {
            InsertValueIntoHashSet(markingsHashSet, markings["@id"].ToString());
        }
    }
    
    internal void InsertValuesIntoDictionary(Dictionary<string, HashSet<string>> constraintsDictionary, string sourceId, string targetId)
    {
        if (!ActivityIds.TryGetValue(sourceId, out string key))
        {
            Debug.Log($"The key '{sourceId}' was not found in the Events dictionary.\n");
            return;
        }

        if (!ActivityIds.TryGetValue(targetId, out string value))
        {
            Debug.Log($"The key '{targetId}' was not found in the Events dictionary.\n");
            return;
        }

        if (constraintsDictionary.TryGetValue(key, out HashSet<string> existingValues))
        {
            if (existingValues.Contains(value))
            {
                Debug.Log("The constraint already exists.\n");
                return;
            }
            existingValues.Add(value);
        }
        else
        {
            constraintsDictionary[key] = new HashSet<string> { value };
        }
    }

    internal void InsertValueIntoHashSet(HashSet<string> markingsHashSet, string eventId)
    {
        if (!ActivityIds.TryGetValue(eventId, out string evt))
        {
            Debug.Log($"The key '{eventId}' was not found in the Events dictionary.\n");
            return;
        }

        markingsHashSet.Add(evt);
    }
    internal HashSet<T> CloneHashSet<T>(HashSet<T> originalHashSet)
    {
        return new HashSet<T>(originalHashSet);
    }

    internal Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(Dictionary<TKey, TValue> originalDictionary)
    {
        return new Dictionary<TKey, TValue>(originalDictionary);
    }

    internal string RemoveHtmlTags(string input)
    {
        // Regular expression to match any HTML tag
        var regex = new Regex("<.*?>");

        // Replace all HTML tags with an empty string
        return regex.Replace(input, "");
    }
}