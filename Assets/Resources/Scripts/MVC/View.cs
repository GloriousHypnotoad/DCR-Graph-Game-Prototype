using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public event Action<string> onActivityExecutedReceived;
    public Dictionary<string, GameObject> Activities { get; private set; }  = new Dictionary<string, GameObject>();

    Light Moonlight;
    Light Sunlight;
    public void Initialize()
    {
        GameObject MoonLightObject = GameObject.Find("Moonlight");
        GameObject SunLightObject = GameObject.Find("Sunlight");
        
        Moonlight = MoonLightObject.GetComponent<Light>();
        Sunlight = SunLightObject.GetComponent<Light>();
    }

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
            activity.SubscribeToExecutedActivity(forwardExecutedActivityId);
            
            Activities.Add(kvp.Key, activtyObject);
        }
    }

    public void UpdateView(
        HashSet<string> activityIds,
        HashSet<string> executed,
        HashSet<string> included,
        HashSet<string> pending,
        HashSet<string> disabled,
        HashSet<string> hasUnmetMilestones
    ){
        foreach (string id in activityIds)
        {
            if (Activities.TryGetValue(id, out GameObject activity))
            {
                Activity viewActivity = activity.GetComponent<Activity>();
                viewActivity.UpdateActivity(executed.Contains(id), included.Contains(id), pending.Contains(id), disabled.Contains(id), hasUnmetMilestones.Contains(id));
            }
            else
            {
                Debug.Log($"{id} not found in Activities");
            }
        }
        
        if (pending.Count != 0)
        {
            if (!Moonlight.enabled)
            {
                Moonlight.enabled = true;
            }
            if (Sunlight.enabled)
            {
                Sunlight.enabled = false;
            }
            RenderSettings.skybox = Resources.Load<Material>("Skyboxes/FS000_Night_01");
        }
        else
        {
            if (!Sunlight.enabled)
            {
                Sunlight.enabled = true;
            }
            if (Moonlight.enabled)
            {
                Moonlight.enabled = false;
            }
            RenderSettings.skybox = Resources.Load<Material>("Skyboxes/FS000_Day_03");
        }
        
        DynamicGI.UpdateEnvironment(); // Update global illumination
    }
    public void forwardExecutedActivityId(Activity executedActivity){
        // Have Activy react to interaction (spi or something)
        onActivityExecutedReceived?.Invoke(executedActivity.Id);
    }
    public void SubscribeToExecutedActivityReceived(Action<string> subscriber){
        onActivityExecutedReceived+=subscriber;
    }
}
