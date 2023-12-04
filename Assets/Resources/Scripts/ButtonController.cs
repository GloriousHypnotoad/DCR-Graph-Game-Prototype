using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool ButtonEnabled {get; private set;}
    public Action<float> _onPressed;
    private ObjectRotation _objectRotation;
    private ObjectBobbing _objectBobbing;
    private ObjectBounceAndSomersault _objectBounceAndSomersault;
    private ObjectShake _objectShake;

    void Awake()
    {
        _objectRotation = GetComponent<ObjectRotation>();
        _objectBobbing = GetComponent<ObjectBobbing>();
        _objectBounceAndSomersault = GetComponent<ObjectBounceAndSomersault>();
        _objectShake = GetComponent<ObjectShake>();
    }


    public void SetButtonEnabled(bool buttonEnabled)
    {
        ButtonEnabled = buttonEnabled;
    }
    public void OnSelected()
    {
        if (ButtonEnabled)
        {
            PerformQuickRotation();
            _onPressed?.Invoke(_objectRotation.getQuickRotationDuration());
        }
        else
        {
            PerformShake();
        }
    }
    public void StartRotation()
    {
        _objectRotation.StartAnimation();       
    }
    public void StopRotation()
    {
        _objectRotation.StopAnimation();
    }
    public void StartBobbing()
    {
        _objectBobbing.StartAnimation();       
    }
    public void StopBobbing()
    {
        _objectBobbing.StopAnimation();
    }
    public void PerformQuickRotation()
    {
        _objectRotation.PerformQuickRotation();
    }
    public void PerformShake()
    {
        _objectShake.StartShake();
    }
    public void StartJumping()
    {
        _objectBounceAndSomersault.StartAnimation();
    }
    public void StopJumping()
    {
        _objectBounceAndSomersault.StopAnimation();
    }

    public void SubscribeToOnPressed(Action<float> subscriber)
    {
        _onPressed += subscriber;
    }
}
