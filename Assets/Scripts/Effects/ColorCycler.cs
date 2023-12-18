using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColorCycler : MonoBehaviour
{
    private HashSet<Color> _colors;
    private Material _material;
    private float _waitTime = 1f;
    private Coroutine _colorCycleCoroutine;
    private bool _isCycling = true;

    void Awake()
    {
        // Get the Renderer component and its material
        _material = GetComponent<Renderer>().material;
    }

    public void StartCycle(HashSet<Color> colors)
    {
        _colors = colors;
        _isCycling = true;
        _colorCycleCoroutine = StartCoroutine(CycleColors());
    }

    IEnumerator CycleColors()
    {
        while (_isCycling)
        {
            foreach (var color in _colors)
            {
                // Set material color
                _material.color = color;
                
                // Wait for the specified time
                yield return new WaitForSeconds(_waitTime);
            }
        }
    }

    public bool IsRunning()
    {
        return _isCycling;
    }

    public void StopCycle()
    {
        _isCycling = false;
        if (_colorCycleCoroutine != null)
        {
            StopCoroutine(_colorCycleCoroutine);
        }
    }
}