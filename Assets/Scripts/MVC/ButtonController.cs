using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonController : MonoBehaviour
{
    public bool ButtonEnabled { get; private set; }
    private ObjectRotation _objectRotation;
    public event Action<float> _onPressed;
    private GameObject _buttonOpaque;
    private GameObject _buttonTransparent;
    
    private ObjectShake _objectShake;

    void Awake()
    {
        _objectRotation = GetComponent<ObjectRotation>();
        _objectShake = GetComponent<ObjectShake>();
        _buttonOpaque = transform.Find(FileStrings.ButtonOpaque).gameObject;
        _buttonTransparent = transform.Find(FileStrings.ButtonTransparent).gameObject;
    }
    void Start()
    {
    }
    public void TogglePlayerIsNearby(bool playerIsNearby)
    {
        if (playerIsNearby)
        {
            // Do something if button enabled.
        } 
        else
        {    
            // Do something if button enabled.
        }
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
    /*

   void OnDestroy()
   {
   }
   */
}
