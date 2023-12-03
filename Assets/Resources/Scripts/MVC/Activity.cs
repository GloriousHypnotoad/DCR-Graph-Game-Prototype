using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class Activity : MonoBehaviour
{
    public string Id { get; private set; }
    public string Label { get; private set; }
    public string Description { get; private set; }
    private Action<Activity> _onActivityExecuted;

    GameObject GlitterObject;
    GameObject ActivityExecuteButton;
    GameObject ActivityExecuteButtonExcluded;
    GameObject ActivityProps;
    GameObject ActivityPropsExcluded;
    GameObject PushButtonObject;
    GameObject PushButton;
    GameObject PushButtonObjectExcluded;
    GameObject FogEffect;
    ButtonController ButtonController;
    ButtonController ButtonControllerExcluded;
    Light[] ButtonBodyLights;
    Light[] PropLights;
    Light PushButtonLight;
    Light PushButtonLightExcluded;
    ParticleSystem Fog;

    // Start is called before the first frame update
    void Start()
    {/*
        //Child Objects
        GlitterObject = transform.Find("Glitter").gameObject;
        ActivityExecuteButton = transform.Find("ActivityExecuteButton").gameObject;
        ActivityExecuteButtonExcluded = transform.Find("ActivityExecuteButtonExcluded").gameObject;
        ActivityProps = transform.Find("ActivityProps").gameObject;
        ActivityPropsExcluded = transform.Find("ActivityPropsExcluded").gameObject;
        PushButtonObject = transform.Find("ActivityExecuteButton/PushButtonObject").gameObject;
        PushButtonObjectExcluded = transform.Find("ActivityExecuteButtonExcluded/PushButtonObject").gameObject;
        FogEffect = transform.Find("Fog/Fog").gameObject;

        
        // Light Component arrays
        ButtonBodyLights = CombineLights(
            transform.Find("ActivityExecuteButton/BodyObject/Lights"),
            transform.Find("ActivityExecuteButtonExcluded/BodyObject/Lights")
        );

        PropLights = CombineLights(
            transform.Find("ActivityProps/PropLights"), 
            transform.Find("ActivityPropsExcluded/PropLights")
        );

        PushButtonLight = PushButtonObject.transform.Find("Light").gameObject.GetComponent<Light>();
        PushButton = PushButtonObject.transform.Find("PushButton").gameObject;

        PushButtonLightExcluded = PushButtonObjectExcluded.transform.Find("Light").gameObject.GetComponent<Light>();

        // Fog Component
        Fog = FogEffect.GetComponent<ParticleSystem>();
*/
        
        //Child Object Components
        ButtonController = transform.Find("ActivityExecuteButton").gameObject.GetComponent<ButtonController>();
        ButtonControllerExcluded = transform.Find("ActivityExecuteButtonExcluded").gameObject.GetComponent<ButtonController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
  
    public void Initialize(string id, string label)
    {
        Id = id;
        Label = label;

        ButtonController.SubscribeToOnButtonPressed(OnButtonPressed);
        ButtonControllerExcluded.SubscribeToOnButtonPressed(OnButtonPressed);
    }

    public void OnButtonPressed(ButtonController button)
    {

        _onActivityExecuted?.Invoke(this);
        // Display execution
        // Highlight outgoing constraints
    }
    /*
    public void UpdateActivity(bool executed, bool included, bool pending, bool disabled, bool hasUnmetMilestones)
    {
        if (executed)
        {
            if (!PushButtonLight.enabled)
            {
                PushButtonLight.enabled = true;
            }
            
            if (!PushButtonLightExcluded.enabled)
            {
                PushButtonLightExcluded.enabled = true;
            }

            PushButton.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/ButtonGreenEmission");
        } 
        else
        {   
            if (PushButtonLightExcluded.enabled)
            {
                PushButtonLightExcluded.enabled = false;
            }
            if (PushButtonLightExcluded.enabled)
            {
                PushButtonLightExcluded.enabled = false;
            }

            PushButton.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/ButtonGreen");
        }
        if (pending)
        {
            ToggleLights(ButtonBodyLights, true);
            ToggleLights(PropLights, true);
        } 
        else
        {
            ToggleLights(ButtonBodyLights, false);
            ToggleLights(PropLights, false);
        }
        if (disabled || hasUnmetMilestones)
        {
            GlitterObject.SetActive(false);
            ButtonController.SetButtonEnabled(false);
            Fog.Play();
        } 
        else
        {
            GlitterObject.SetActive(true);
            ButtonController.SetButtonEnabled(true);
            Fog.Stop();
        }
        if (included)
        {
            ActivityExecuteButton.SetActive(true);
            ActivityProps.SetActive(true);

            ActivityExecuteButtonExcluded.SetActive(false);
            ActivityPropsExcluded.SetActive(false);
        } 
        else
        {
            GlitterObject.SetActive(false);
            ButtonController.SetButtonEnabled(false);
            ActivityExecuteButton.SetActive(false);
            ActivityProps.SetActive(false);

            ActivityExecuteButtonExcluded.SetActive(true);
            ActivityPropsExcluded.SetActive(true);
        }
    }*/

    public void SetLightState(bool isExecuted)
    {
        // Toggle Light under "ActivityExecuteButton"
        var lightObject = transform.Find("ActivityExecuteButton/PushButtonObject/Light")?.gameObject;
        if (lightObject != null)
        {
            lightObject.SetActive(isExecuted);
        }

        // Toggle Light under "ActivityExecuteButtonExcluded"
        var lightObjectExcluded = transform.Find("ActivityExecuteButtonExcluded/PushButtonObject/Light")?.gameObject;
        if (lightObjectExcluded != null)
        {
            lightObjectExcluded.SetActive(isExecuted);
        }
    }

    
    public void UpdatePushButtonMaterial(bool isExecuted)
    {
        string materialPath = isExecuted ? "Materials/ButtonGreenEmission" : "Materials/ButtonGreen";
        UpdateMaterial("ActivityExecuteButton/PushButtonObject/PushButton", materialPath);
    }


    private void UpdateMaterial(string objectPath, string materialPath)
    {
        var gameObject = transform.Find(objectPath)?.gameObject;
        if (gameObject != null)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = Resources.Load<Material>(materialPath);
            }
        }
    }

    /*
    public void SetPendingState(bool isPending)
    {
        ToggleChildObject("ActivityExecuteButton/BodyObject/Lights", isPending);
        ToggleChildObject("ActivityExecuteButtonExcluded/BodyObject/Lights", isPending);
        ToggleChildObject("ActivityProps/PropLights", isPending);
        ToggleChildObject("ActivityPropsExcluded/PropLights", isPending);
    }
    */
    public void SetPendingState(bool isPending)
    {
        ToggleChildObjects(isPending,
            "ActivityExecuteButton/BodyObject/Lights",
            "ActivityExecuteButtonExcluded/BodyObject/Lights",
            "ActivityProps/PropLights",
            "ActivityPropsExcluded/PropLights");
    }
    public void SetDisabledOrUnmetMilestonesState(bool isDisabledOrUnmetMilestones)
    {
        // Disable or enable "Glitter" object and buttons
        ToggleChildObjects(!isDisabledOrUnmetMilestones, "Glitter");
        SetButtonsEnabled(!isDisabledOrUnmetMilestones, "ActivityExecuteButton", "ActivityExecuteButtonExcluded");

        // Enable or disable Fog component
        var fogComponent = transform.Find("Fog/Fog")?.gameObject;

        if (fogComponent != null)
        {
            fogComponent.SetActive(isDisabledOrUnmetMilestones);
        }
    }

    public void SetIncludedState(bool isIncluded)
    {
        // Enable or disable specified child components
        ToggleChildObjects(isIncluded, 
            "ActivityExecuteButton", 
            "ActivityProps");

        ToggleChildObjects(!isIncluded, 
            "ActivityExecuteButtonExcluded", 
            "ActivityPropsExcluded",
            "Glitter"); // Also handling "Glitter" in the same manner

        // Handle button states as per disabled logic
        SetButtonsEnabled(!isIncluded, 
            "ActivityExecuteButton", 
            "ActivityExecuteButtonExcluded");
    }


    /*
    private void ToggleChildObject(string childPath, bool isActive)
    {
        var childObject = transform.Find(childPath)?.gameObject;
        if (childObject != null)
        {
            childObject.SetActive(isActive);
        }
    }
    */

    private void ToggleChildObjects(bool isActive, params string[] childPaths)
    {
        foreach (var path in childPaths)
        {
            var childObject = transform.Find(path)?.gameObject;
            if (childObject != null)
            {
                childObject.SetActive(isActive);
            }
        }
    }
    /*
    private void SetButtonEnabled(string buttonPath, bool isEnabled)
    {
        var buttonObj = transform.Find(buttonPath)?.gameObject;
        if (buttonObj != null)
        {
            var buttonController = buttonObj.GetComponent<ButtonController>();
            if (buttonController != null)
            {
                buttonController.SetButtonEnabled(isEnabled);
            }
        }
    }
    */

    private void SetButtonsEnabled(bool isEnabled, params string[] buttonPaths)
    {
        foreach (var path in buttonPaths)
        {
            var buttonObj = transform.Find(path)?.gameObject;
            if (buttonObj != null)
            {
                var buttonController = buttonObj.GetComponent<ButtonController>();
                buttonController?.SetButtonEnabled(isEnabled);
            }
        }
    }

    public void SubscribeToExecutedActivity(Action<Activity> subscriber){
        _onActivityExecuted+=subscriber;
    }

    public override string ToString()
    {
        return $"Activity ID: {Id}, Label: {Label}";
    }

    private void ToggleLights(Light[] lights, bool onOff)
    {
        foreach (Light light in lights)
        {
            if (light.enabled != onOff)
            {
                light.enabled = onOff;
            }
        }
    }
    private Light[] CombineLights(Transform container1, Transform container2)
{
    int totalLightCount = container1.childCount + container2.childCount;
    Light[] combinedLights = new Light[totalLightCount];

    for (int i = 0; i < container1.childCount; i++)
    {
        combinedLights[i] = container1.GetChild(i).GetComponent<Light>();
    }

    for (int i = 0; i < container2.childCount; i++)
    {
        combinedLights[container1.childCount + i] = container2.GetChild(i).GetComponent<Light>();
    }

    return combinedLights;
}
}
