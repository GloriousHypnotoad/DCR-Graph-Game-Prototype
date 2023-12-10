using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private GameObject _fog;
    private GameObject _glitter;
    private GameObject _pushButtonLight;
    private GameObject _sceneryLight;

    void Awake()
    {
        _fog = transform.Find(FileStrings.FogPath).gameObject;
        _glitter = transform.Find(FileStrings.Glitter).gameObject;
        _pushButtonLight = transform.Find(FileStrings.PushButtonLight).gameObject;
        _sceneryLight = transform.Find(FileStrings.SceneryLight).gameObject;
    }

    public void SwitchParticleColor(Color color)
    {
        ParticleSystem ps = _glitter.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startColor = color;
    }

    public void ToggleFog(bool active)
    {
        _fog.SetActive(active);
    }

    public void ToggleGlitter(bool active)
    {
        _glitter.SetActive(active);
    }

    internal void TogglePushButtonLight(bool active)
    {
        _pushButtonLight.SetActive(active);
    }

    internal void ToggleSceneryLight(bool isPending)
    {
        _sceneryLight.SetActive(isPending);
    }
}
