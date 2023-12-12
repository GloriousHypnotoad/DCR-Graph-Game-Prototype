using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private GameObject _fog;
    private GameObject _glitter;
    private GameObject _sceneryLight;
    private GameObject _firework;
    private GameObject _glitterBurst;
    private GameObject _godray;

    void Awake()
    {
        _fog = transform.Find(FileStrings.Fog).gameObject;
        _glitter = transform.Find(FileStrings.Glitter).gameObject;
        _sceneryLight = transform.Find(FileStrings.SceneryLight).gameObject;
        _firework = transform.Find(FileStrings.Firework).gameObject;
        _glitterBurst = transform.Find(FileStrings.GlitterBurst).gameObject;
        _godray = transform.Find(FileStrings.GodRay).gameObject;
    }

    public void ChangehGlitterColor(Color color)
    {
        ParticleSystem ps = _glitter.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startColor = color;
    }

    public void ToggleFog(bool isActive)
    {
        _fog.SetActive(isActive);
    }

    public void ToggleGlitter(bool isActive)
    {
        _glitter.SetActive(isActive);
    }

    internal void ChangeSceneryLightColor(Color color)
    {
        _sceneryLight.GetComponent<Light>().color = color;
    }

    public void ToggleSceneryLight(bool isPending)
    {
        _sceneryLight.SetActive(isPending);
    }
    public void LauchFirework()
    {
        _firework.GetComponent<Firework>().LaunchOne();
    }

    public void GlitterBurst(float duration)
    {
        _glitterBurst.GetComponent<ParticleSystem>().Play();
    }

    internal void TogglePulseOnSceneryLight(bool isPulsating)
    {
        _sceneryLight.GetComponent<PulsatingLight>().TogglePulse(isPulsating);
    }

    internal void ToggleGodray(bool isActive)
    {
        GodRay godRay =_godray.GetComponent<GodRay>();
        if (godRay.GetIsActive() != isActive)
        {
            godRay.toggleActive(isActive);
        }
    }
}
