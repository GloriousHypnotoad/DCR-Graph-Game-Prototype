using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsController : MonoBehaviour
{
    GameObject _lock;
    GameObject _key;

void Awake()
{
    _lock = transform.Find(FileStrings.Lock).gameObject;
    _key = transform.Find(FileStrings.Key).gameObject;
}

    internal void StartLockColorCycle(HashSet<Color> colors)
    {
        _lock.GetComponent<ColorCycler>().StartCycle(colors);        
    }

    internal void StopLockColorCycle()
    {
        if(_lock.GetComponent<ColorCycler>().IsRunning())
        {
            _lock.GetComponent<ColorCycler>().StopCycle();
        }
    }

    internal void SetLockColor(Color color)
    {
        if (color != Color.white)
        {
            Material lockMaterial = _lock.GetComponent<Renderer>().material;
            lockMaterial.color = color;
            lockMaterial.EnableKeyword("_EMISSION");
            lockMaterial.SetColor("_EmissionColor", color);
        }
    }

    internal void ToggleLock(bool isActive)
    {
        _lock.SetActive(isActive);
    }

    internal void ToggleKey(bool isActive)
    {
        _key.SetActive(isActive);
    }

    internal void SetKeyColor(Color color)
    {
        if (_key.activeSelf)
        {
            Material lockMaterial = _lock.GetComponent<Renderer>().material;
            lockMaterial.color = color;
            lockMaterial.EnableKeyword("_EMISSION");
            lockMaterial.SetColor("_EmissionColor", color);
        }
    }
}