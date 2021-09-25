using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Old_Scripts;
using Grappling_Hook.Test;
using Player_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Door = Assets.Scripts.LevelCreator.Door;
using LevelManager = Assets.Scripts.LevelCreator.LevelManager;
using Object = UnityEngine.Object;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool isTeleporting;

    [FormerlySerializedAs("movementSpeed")]
    public float MovementSpeed;

    public bool IsMoving;
    public float LerpSpeed;

    public bool IsInAir => Hook.CurrentHookState == Hook.HookState.Hooking ||
                           Hook.CurrentHookState == Hook.HookState.OnWall;

    public float FlyingSpeed;
    public Animator Animator;
    public Rigidbody2D rigidbody2D;


    [SerializeField] private LevelManager Manager;
    Vector2 movement;
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    public Door LastVisitedDoor { get; set; }

    [SerializeField] private WallFinder WallFinder;
    [SerializeField] private AbyssColliderChanger AbyssColliderChanger;
    private static readonly int XDirection = Animator.StringToHash("xDirection");
    private static readonly int YDirection = Animator.StringToHash("yDirection");

    private bool previousMovement = false;
    public Hook Hook;

    private void Start()
    {
        Manager.EnterTheFloor(this);
    }

    public void Teleport()
    {
        StartCoroutine(StopMovement());
    }

    void Move(Vector2 movementVector)
    {
        var previousPosition = transform.position;
        var movementDelta = new Vector3(movementVector.x * MovementSpeed * Time.deltaTime,
            movementVector.y * MovementSpeed * Time.deltaTime, 0);
        print(movementDelta);

        if (Hook.CurrentHookState == Hook.HookState.Hooking)
        {
            movementDelta *= FlyingSpeed;
        }

        var newPosition = previousPosition + movementDelta;

        transform.position = newPosition;
    }


    void FixedUpdate()
    {
        if (Hook.HookState.Hooking == Hook.CurrentHookState)
        {
            movement *= FlyingSpeed;
        }

        rigidbody2D.velocity = new Vector2(movement.x * MovementSpeed, movement.y * MovementSpeed);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        Hook.SetPlayerMovement(movement);
        IsMoving = movement.sqrMagnitude > 0;
        if (IsMoving && Hook.CurrentHookState == Hook.HookState.NotHooking)
        {
            Animator.SetFloat(XDirection, movement.x);
            Animator.SetFloat(YDirection, movement.y);
        }

        if (Hook.CurrentHookState != Hook.HookState.NotHooking)
        {
            print(Hook.GetHookDirection());

            Animator.SetFloat(XDirection, Hook.GetHookDirection().x);
            Animator.SetFloat(YDirection, Hook.GetHookDirection().y);
        }

        Animator.SetBool(IsMovingHash, IsMoving);
        switch (IsMoving)
        {
            case true:
                if (!previousMovement)
                {
                    AudioManager.instance.Play("walk");
                }

                break;
            case false:
                AudioManager.instance.StopPlaying("walk");
                break;
        }

        previousMovement = IsMoving;
        AbyssColliderChanger.SetAbyssTrigger(Hook.CurrentHookState != Hook.HookState.NotHooking);
    }

    IEnumerator StopMovement()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(0.5f);
        isTeleporting = false;
    }

    public void Die()
    {
        Manager.RestartCurrentRoom(this);
    }
}