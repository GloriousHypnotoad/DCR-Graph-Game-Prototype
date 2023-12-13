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
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;
    private float _initialEmissionRate;

    void Awake()
    {
        _fog = transform.Find(FileStrings.Fog).gameObject;
        _glitter = transform.Find(FileStrings.Glitter).gameObject;
        _sceneryLight = transform.Find(FileStrings.SceneryLight).gameObject;
        _firework = transform.Find(FileStrings.Firework).gameObject;
        _glitterBurst = transform.Find(FileStrings.GlitterBurst).gameObject;
        _godray = transform.Find(FileStrings.GodRay).gameObject;
        _particleSystem =_glitter.GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
        _initialEmissionRate = _emissionModule.rateOverTime.constant;
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

    public void ChangeGlitterColor(Color color)
    {
        var mainModule = _glitter.GetComponent<ParticleSystem>().main;
        mainModule.startColor = color;
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

    internal void SetGlitterRate(float v)
    {
        _emissionModule.rateOverTime = v;
    }

    internal void ResetGlitterRate()
    {
        _emissionModule.rateOverTime = _initialEmissionRate;
    }

    internal void StartPushButtonColorCycle(HashSet<Color> colors)
    {
        _glitter.GetComponent<ColorCycler>().StartCycle(colors);
    }
}
