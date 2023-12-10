using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private GameObject _fog;
    internal void ToggleFog(bool isDisabled)
    {
        _fog.SetActive(isDisabled);
    }

    void Awake()
    {
        _fog = transform.Find(FileStrings.FogPath).gameObject;

    }
}
