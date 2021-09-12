using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Old_Scripts;
using Grappling_Hook.Test;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool isActivated = false;
    public BossPhaseState[] states;
    public CanonModule[] modules;
    public Transform rotationModule;
    public Vector3[] positions;

    public float throwAwaySpeed;

    public LevelData level;
    private BossPhaseState currentState;
    private int сurrentPoint = 0;
    private bool isAlive = true;
    private int currentSessionBullets;
    private bool isOverheated;
    private int currentStateIndex = 0;
    public float rotationSpeed;
    private float angle = 0;
    private bool isPushing;

    public delegate void OnBossActivation(LevelData data);

    public static OnBossActivation ActivateBoss;

    public delegate void OnBossDefeat(LevelData data);

    public static OnBossDefeat KillBoss;

    public ParticleSystem explosion;
    private bool isActivating;
    void FixedUpdate()
    {
        if (isActivated) Move();
    }

    IEnumerator Overheat()
    {
        isOverheated = true;
        yield return new WaitForSeconds(currentState.overheatTime);
        isOverheated = false;
    }

    void Move()
    {
        if (isOverheated || !isAlive) return;
        var step = states[currentStateIndex].bossSpeed * Time.fixedDeltaTime;
        var neededPosition = positions[сurrentPoint];
        var newPosition = Vector2.MoveTowards(transform.position, neededPosition + level.generationPosition, step);
        transform.position = newPosition;
        if (Vector3.Distance(newPosition, neededPosition + level.generationPosition) < step)
        {
            сurrentPoint = (сurrentPoint + 1) % positions.Length;
        }
    }

    IEnumerator Shoot()
    {
        while (isAlive && isActivated)
        {
            if (!isOverheated)
            {
                foreach (var cannonModule in modules)
                {
                    cannonModule.Shoot(currentState.shotSpeed, currentState.shotSize,cannonModule.transform.rotation);
                }

                currentSessionBullets += 1;

                StartCoroutine(Rotate(currentState.rotationAngle));
                yield return new WaitForSeconds(currentState.shotDelay);
                //var rotation = Quaternion.Euler(new Vector3(0f, 0f, currentState.rotationAngle));
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1.2f * Time.deltaTime);

                if (currentSessionBullets >= currentState.shotAmount)
                {
                    StartCoroutine(Overheat());
                    currentSessionBullets = 0;
                }
            }

            yield return null;
        }

        yield return null;
    }

    IEnumerator Rotate(float endZRot)
    {
        if (isOverheated) yield break;
        Quaternion startRotation = rotationModule.rotation;

        float time = 0;
        while (time < 1f)
        {
            time = Mathf.Min(1f, time + Time.deltaTime / rotationSpeed);
            Vector3 newEulerOffset = Vector3.forward * (endZRot * time);
            // global z rotation
            rotationModule.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            // local z rotation
            // transform.rotation = startRotation * Quaternion.Euler(newEulerOffset);
            yield return null;
        }
    }

    IEnumerator CreateExplosion()
    {
        isActivating = true;
        explosion.Play();
        while (!explosion.isStopped)
        {
            yield return new WaitForSeconds(0.01f);
        }
        
        ActivateBoss?.Invoke(level);
        isActivated = true;
        currentState = states[currentStateIndex];
        angle = currentState.rotationAngle;
        StartCoroutine(Shoot());
        
        isActivating = false;
        yield return new WaitForSeconds(0.01f);

    }

    public Coroutine GetDamage()
    {
        if (isActivating) return null;
        if (!isActivated)
        {
            //ActivateBoss?.Invoke(level);
            //isActivated = true;
            //currentState = states[currentStateIndex];
            //angle = currentState.rotationAngle;
           // StartCoroutine(Shoot());
            return StartCoroutine(CreateExplosion());
        }

        currentStateIndex += 1;
        if (currentStateIndex == states.Length)
        {
            Die();
            return null;
        }

        currentState = states[currentStateIndex];
        return StartCoroutine(CreateExplosion());
    }

    void Die()
    {
        print("Boss ded");
        KillBoss?.Invoke(level);
        isAlive = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (positions == null || positions.Length < 1) return;
        foreach (var position in positions)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(position, 0.5f);
        }
    }

    public IEnumerator IsPushingAway()
    {
        isPushing = true;
        yield return new WaitForSeconds(currentState.pushTime);
        isPushing = false;
    }

    public IEnumerator ThrowAway(Coroutine explosion,GameObject player)
    {
        if (explosion == null) yield break;
        yield return explosion;
        StartCoroutine(IsPushingAway());
        var pushPowerDelta = currentState.pushPower / currentState.pushTime;
        var power = -1 * currentState.pushPower;
        while (isPushing)
        {
            power += pushPowerDelta * 0.1f;
            if (power < 0)
                player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position,
                    power * Time.fixedDeltaTime);
            yield return null;
        }

        yield return null;
    }
}