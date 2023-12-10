using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityDetectionTrigger : MonoBehaviour
{
    private event Action _mouseOver;
    private event Action _mouseExit;
    private event Action _mouseDown;

    void OnMouseOver()
    {
        _mouseOver?.Invoke();
    }
    void OnMouseExit()
    {
        _mouseExit?.Invoke();
    }
    void OnMouseDown()
    {
        _mouseDown?.Invoke();
    }
    public void SubscribeToOnMouseOver(Action subject)
    {
        _mouseOver += subject;
    }
    public void SubscribeToOnMouseExit(Action subject)
    {
        _mouseExit += subject;
    }
    public void SubscribeToOnMouseDown(Action subject)
    {
        _mouseDown += subject;
    }
}