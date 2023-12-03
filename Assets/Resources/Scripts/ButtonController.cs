using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool ButtonEnabled {get; private set;}
    public Action<ButtonController> _onButtonPressed;
    // Start is called before the first frame update

    public void SetButtonEnabled(bool buttonEnabled)
    {
        ButtonEnabled = buttonEnabled;
    }
    public void OnSelected()
    {
        _onButtonPressed?.Invoke(this);
    }
    public void SubscribeToOnButtonPressed(Action<ButtonController> subscriber)
    {
        _onButtonPressed += subscriber;
    }
}
