using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParticleHandler : EntityComponent
{
    [SerializeField] SurfaceToParticle particleColorGetter;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int colorCount;
    [SerializeField] float colorCheckFrequency = 0.5f;
    Color[] colors;

    Dictionary<ParticleSystem, ParticleSystemInstance> _instances = new();

    private void Start()
    {
        colors = new Color[colorCount];
    }

    public void SpawnParticle(Vector2 CheckDirection, ParticleSystem particles, Quaternion rotation)
    {
        if (!particleColorGetter.GetPixelColors(transform.position + Vector3.up, CheckDirection.normalized, groundLayer, ref colors, out var hitPos)) return;

        var mainModule = particles.main;
        var gradient = mainModule.startColor.gradient;
        if (gradient?.colorKeys?.Length != colorCount)
        {
            gradient = new Gradient();
        }

        var colorKeys = new GradientColorKey[colorCount];

        for (int i = 0; i < colorCount; i++)
        {
            colorKeys[i] = new GradientColorKey(colors[i], (float)i / colorCount);
        }

        gradient.colorKeys = colorKeys;

        mainModule.startColor = new ParticleSystem.MinMaxGradient(gradient);
        Instantiate(particles, hitPos, rotation);
    }

    public void SpawnLoopedParticle(Vector2 checkDirection, ParticleSystem particles, Quaternion rotation)
    {
        if (_instances.TryGetValue(particles, out var psInstance))
        {
            if (!psInstance.instance.isPlaying || !psInstance.IsSame(checkDirection, rotation))
                UpdateLoopedParticle(psInstance, checkDirection, rotation);
            return;
        }

        var particle = Instantiate(particles, transform.position, rotation, transform);
        var routine = StartCoroutine(ColorizeLoopedParticle(particle, checkDirection, colorCheckFrequency));
        var particleSystemInstance = new ParticleSystemInstance(particle, routine, checkDirection, rotation);
        _instances.Add(particles, particleSystemInstance);
    }


    public void StopLoopedParticle(ParticleSystem particles)
    {
        if (!_instances.TryGetValue(particles, out var instance)) return;

        StopLoopedParticle(instance);
    }

    private void StopLoopedParticle(ParticleSystemInstance psInstance)
    {
        psInstance.instance.Stop();
        if (psInstance.coroutine != null)
        {
            StopCoroutine(psInstance.coroutine);
            psInstance.coroutine = null;
        }
    }

    private void UpdateLoopedParticle(ParticleSystemInstance psInstance, Vector2 checkDirection, Quaternion rotation)
    {
        psInstance.SetRotation(rotation);
        psInstance.rotation = rotation;

        StopLoopedParticle(psInstance);
        psInstance.coroutine = StartCoroutine(ColorizeLoopedParticle(psInstance.instance, checkDirection, colorCheckFrequency));
        psInstance.instance.Play();
    }

    IEnumerator ColorizeLoopedParticle(ParticleSystem instance, Vector2 checkDirection, float checkFrequency)
    {
        var mainModule = instance.main;
        var startColor = mainModule.startColor.color;
        var modifiedColor = startColor;

        while (instance.isPlaying)
        {
            if (!particleColorGetter.GetPixelColor(transform.position + Vector3.up, checkDirection.normalized, groundLayer, ref modifiedColor, out var hitPos))
                break;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(modifiedColor);
            instance.transform.position = hitPos;

            yield return new WaitForSeconds(checkFrequency);
        }
    }
}

public class ParticleSystemInstance
{
    public ParticleSystem instance;
    public Coroutine coroutine;

    public Vector2 checkDir;
    public Quaternion rotation;

    public ParticleSystemInstance(ParticleSystem instance, Coroutine coroutine, Vector2 checkDirection, Quaternion rotation)
    {
        this.instance = instance;
        this.coroutine = coroutine;
        checkDir = checkDirection;
        this.rotation = rotation;
    }

    public bool IsSame(Vector2 checkDirection, Quaternion rotation)
    {
        return checkDir == checkDirection && this.rotation == rotation;
    }

    public void SetRotation(Quaternion rotation)
    {
        instance.transform.rotation = rotation;
        this.rotation = rotation;
    }
}
