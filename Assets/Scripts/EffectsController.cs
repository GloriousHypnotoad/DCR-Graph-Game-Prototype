using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private GameObject _fog;
    private GameObject _glitter;

    void Awake()
    {
        _fog = transform.Find(FileStrings.FogPath).gameObject;
        _glitter = transform.Find(FileStrings.GlitterPath).gameObject;
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
}
