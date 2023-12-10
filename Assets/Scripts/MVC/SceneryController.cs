using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryController : MonoBehaviour
{
    GameObject _sceneryOpaque;
    GameObject _sceneryTransparent;

    void Start()
    {
        _sceneryOpaque = transform.Find(FileStrings.SceneryOpaque).gameObject;
        _sceneryTransparent = transform.Find(FileStrings.SceneryTransparent).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAnimatedElement(bool isAnimated)
    {

    }

    public void SetOpaque(bool opaque)
    {
        _sceneryOpaque.SetActive(opaque);
        _sceneryTransparent.SetActive(!opaque);
    }
}