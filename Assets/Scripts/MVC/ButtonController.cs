using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonController : MonoBehaviour
{
    public bool ButtonEnabled { get; private set; }
    private ObjectRotation _objectRotation;
    private ObjectBobbing _objectBobbing;
    public event Action<float> _onPressed;
    private GameObject _buttonOpaque;
    private GameObject _buttonTransparent;
    private GameObject _buttonOpaquePushButton;
    private ObjectShake _objectShake;

    void Awake()
    {
        _objectBobbing = GetComponentInChildren<ObjectBobbing>();
        _objectRotation = GetComponent<ObjectRotation>();
        _objectShake = GetComponent<ObjectShake>();
        _buttonOpaque = transform.Find(FileStrings.ButtonOpaque).gameObject;
        _buttonTransparent = transform.Find(FileStrings.ButtonTransparent).gameObject;
        _buttonOpaquePushButton = transform.Find(FileStrings.ButtonOpaquePushButton).gameObject;
    }
    void Start()
    {
    }
    public void TogglePushButtonAnimation(bool playerIsNearby)
    {
        _objectBobbing.ToggleAnimation(playerIsNearby);
    }

    public void ToggleRotation(bool isRotating)
    {
        _objectRotation.ToggleAnimation(isRotating);       
    }
    // TODO: Should just emit onSelected and let the View inform the controller for approval.
    public void PressButton()
    {
        PerformQuickRotation();
        _onPressed?.Invoke(_objectRotation.getQuickRotationDuration());
    }

    public void PressButtonRefuse()
    {
        PerformShake();
    }

    public void PerformQuickRotation()
    {
        _objectRotation.PerformQuickRotation();
    }

    public void PerformShake()
    {
        _objectShake.StartShake();
    }

    public void SubscribeToOnPressed(Action<float> subscriber)
    {
        _onPressed += subscriber;
    }

    public void SetOpaque(bool opaque)
    {
        _buttonOpaque.SetActive(opaque);
        _buttonTransparent.SetActive(!opaque);
    }

    internal void StartPushButtonColorCycle(HashSet<Color> colors)
    {
        _buttonOpaquePushButton.GetComponent<ColorCycler>().StartCycle(colors);
    }

    internal void SetPushButtonColor(Color color)
    {
        Material material = _buttonOpaquePushButton.GetComponent<Renderer>().material;
        
        material.color = color;
        
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);

        Debug.Log($"ButtonController: GameObject {gameObject.name} color updated to: {color}");
    }

    internal void StopPushButtonColorCycle()
    {
        _buttonOpaquePushButton.GetComponent<ColorCycler>().StopCycle();
    }
    /*

void OnDestroy()
{
}
*/
}
