using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Old_Scripts;
using Grappling_Hook.Test;
using UnityEngine;

public class MoodChange : MonoBehaviour
{
    public GameObject calmMode;
    public GameObject angryMode;
    public GameObject ring;
    public ParticleSystem angryParticles;
    public ParticleSystem sadParticles;
    
    private float previousX;
    private float size;

    private void OnEnable()
    {
        Boss.ActivateBoss += WakeUp;
    }

    private void OnDisable()
    {
        Boss.ActivateBoss -= WakeUp;
    }

    private void Start()
    {
        sadParticles.Play();
        angryParticles.Stop();
        size = transform.localScale.x;
        angryMode.gameObject.SetActive(false);
        previousX = transform.position.x;
        ring.SetActive(false);
    }

    void WakeUp(LevelData _)
    {
        calmMode.gameObject.SetActive(false);
        angryMode.gameObject.SetActive(true);
        ring.SetActive(true);
        sadParticles.Stop();
        angryParticles.Play();
    }

    private void FixedUpdate()
    {
        var currentX = angryMode.transform.position.x;
        transform.localScale = previousX - currentX < 0 ? new Vector3(-size, size, size) : Vector3.one* size;
        previousX = currentX;
    }
}