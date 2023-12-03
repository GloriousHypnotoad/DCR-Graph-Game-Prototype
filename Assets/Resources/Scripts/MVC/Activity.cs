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
    Light[] ButtonBodyLights;
    Light[] PropLights;
    Light PushButtonLight;
    Light PushButtonLightExcluded;
    ParticleSystem Fog;

    // Start is called before the first frame update
    void Start()
    {
        //Child Objects
        GlitterObject = transform.Find("Glitter").gameObject;
        ActivityExecuteButton = transform.Find("ActivityExecuteButton").gameObject;
        ActivityExecuteButtonExcluded = transform.Find("ActivityExecuteButtonExcluded").gameObject;
        ActivityProps = transform.Find("ActivityProps").gameObject;
        ActivityPropsExcluded = transform.Find("ActivityPropsExcluded").gameObject;
        PushButtonObject = transform.Find("ActivityExecuteButton/PushButtonObject").gameObject;
        PushButtonObjectExcluded = transform.Find("ActivityExecuteButtonExcluded/PushButtonObject").gameObject;
        FogEffect = transform.Find("Fog/Fog").gameObject;

        
        //Child Object Components
        ButtonController = ActivityExecuteButton.GetComponent<ButtonController>();
        
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

    }

    // Update is called once per frame
    void Update()
    {
    }
  
    public void Initialize(string id, string label)
    {
        Id = id;
        Label = label;
        UpdateActivity(false, true, false, false, false);

        ButtonController.SubscribeToOnButtonPressed(OnButtonPressed);
    }

    public void OnButtonPressed(ButtonController button)
    {

        _onActivityExecuted?.Invoke(this);
        // Display execution
        // Highlight outgoing constraints
    }

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
