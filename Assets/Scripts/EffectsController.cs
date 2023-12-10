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
    private GameObject _firework;

    void Awake()
    {
        _fog = transform.Find(FileStrings.Fog).gameObject;
        _glitter = transform.Find(FileStrings.Glitter).gameObject;
        _pushButtonLight = transform.Find(FileStrings.PushButtonLight).gameObject;
        _sceneryLight = transform.Find(FileStrings.SceneryLight).gameObject;
        _firework = transform.Find(FileStrings.Firework).gameObject;
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

    public void TogglePushButtonLight(bool active)
    {
        _pushButtonLight.SetActive(active);
    }

    public void ToggleSceneryLight(bool isPending)
    {
        _sceneryLight.SetActive(isPending);
    }
    public void ToggleFireworks(bool isFiring)
    {
        _firework.GetComponent<Firework>().ToggleFireworks(isFiring);
    }
}
