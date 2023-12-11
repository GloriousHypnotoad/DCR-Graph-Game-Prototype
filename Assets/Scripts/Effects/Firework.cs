using System.Collections;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private float _speed = 50.0f;
    private float _height = 50.0f;
    private float relaunchFrequency = 5.0f;
    private ParticleSystem _explosionEffect;
    private ParticleSystem _plumeEffect;

    private Vector3 startPosition;
    private bool isLaunched = false;
    private bool isFiring = false;
    private Coroutine relaunchRoutine;
    
    void Awake()
    {
        startPosition = transform.position;

        _explosionEffect = transform.Find("ExplosionEffect").GetComponent<ParticleSystem>();
        _plumeEffect = transform.Find("PlumeEffect").GetComponent<ParticleSystem>();
        
        _explosionEffect.Stop();
        _plumeEffect.Stop();
    }

    public void LaunchContinuously(bool firing)
    {
        isFiring = firing;

        if (firing)
        {
            if (relaunchRoutine == null)
            {
                relaunchRoutine = StartCoroutine(RelaunchRoutine());
            }
        }
        else
        {
            if (relaunchRoutine != null)
            {
                StopCoroutine(relaunchRoutine);
                relaunchRoutine = null;
            }
        }
    }

    private IEnumerator RelaunchRoutine()
    {
        while (isFiring)
        {
            ResetFirework();
            Launch();
            yield return new WaitForSeconds(relaunchFrequency);
        }
    }

    public void LaunchOne()
    {
        ResetFirework();
        Launch();
        isFiring = false; // Ensure continuous firing is stopped after one launch
    }

    private void ResetFirework()
    {
        transform.position = startPosition;
        _explosionEffect.Stop();
        _plumeEffect.Clear();
        isLaunched = false;
    }

    public void Launch()
    {
        isLaunched = true;
        _plumeEffect.Play();
    }

    private void MoveUpwards()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void CheckHeight()
    {
        if (transform.position.y >= startPosition.y + _height)
        {
            _explosionEffect.Play();
            _plumeEffect.Stop(false);
            isLaunched = false;
        }
    }

    void Update()
    {
        if (isLaunched)
        {
            MoveUpwards();
            CheckHeight();
        }
    }
}
