using System;
using UnityEngine;

public class ActivityDetectionTrigger : MonoBehaviour
{
    private event Action _mouseOver;
    private event Action _mouseExit;
    private event Action _mouseDown;

    // Triggers
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

    // Subscribe to Triggers
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