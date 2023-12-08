using System;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool ButtonEnabled { get; private set; }
    private ObjectRotation _objectRotation;
    public event Action<float> _onPressed;
    private ObjectBobbing _objectBobbing;
    private ObjectJumpAndSpin _objectJumpAndSpin;
    
    private ObjectShake _objectShake;
    private ObjectRotation _rotation;

    // Animation state tracking
    private bool stoppedBounceAndSomersault = false;
    private bool stoppedRotation = false;
    void Awake()
    {
        _objectRotation = GetComponent<ObjectRotation>();
        //_objectBobbing = GetComponent<ObjectBobbing>();
        _objectJumpAndSpin = GetComponent<ObjectJumpAndSpin>();
        //_objectShake = GetComponent<ObjectShake>();
        _rotation = GetComponent<ObjectRotation>();
    }
    public void TogglePlayerIsNearby(bool playerIsNearby)
    {
        if (playerIsNearby)
        {
            if (_objectJumpAndSpin.IsAnimationRunning())
            {
                _objectJumpAndSpin.StopAnimation();
                stoppedBounceAndSomersault = true;
            }
            if (_rotation.IsAnimationRunning())
            {
                _rotation.StopAnimation();
                stoppedRotation = true;
            }
        } 
        else
        {    
            if (stoppedBounceAndSomersault && !_objectJumpAndSpin.IsAnimationRunning())
            {
                _objectJumpAndSpin.StartAnimation();
                stoppedBounceAndSomersault = false;
            }
            if (stoppedRotation && !_rotation.IsAnimationRunning())
            {
                _rotation.StartAnimation();
                stoppedRotation = false;
            }
        }
    }
    public void SetButtonEnabled(bool buttonEnabled)
    {
        ButtonEnabled = buttonEnabled;
    }

    public void StartRotation()
    {
        _objectRotation.StartAnimation();       
    }

    public void StopRotation()
    {
        _objectRotation.StopAnimation();
    }
    /*
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

    public void StartBobbing()
    {
        //_objectBobbing.StartAnimation();       
    }

    public void StopBobbing()
    {
        //_objectBobbing.StopAnimation();
    }

    public void PerformQuickRotation()
    {
        //_objectRotation.PerformQuickRotation();
    }

    public void PerformShake()
    {
        //_objectShake.StartShake();
    }
*/
    public void StartJumping()
    {
        _objectJumpAndSpin.StartAnimation();
    }
    public void StopJumping()
    {
        _objectJumpAndSpin.StopAnimation();
    }
/*

    public void SubscribeToOnPressed(Action<float> subscriber)
    {
        _onPressed += subscriber;
    }

    void OnDestroy()
    {
    }
    */
}
