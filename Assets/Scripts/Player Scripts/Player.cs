using System.Collections;
using Assets.Scripts;
using Assets.Scripts.LevelCreator;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int XDirection = Animator.StringToHash("xDirection");
    private static readonly int YDirection = Animator.StringToHash("yDirection");
    [HideInInspector] public bool isTeleporting;

    [FormerlySerializedAs("movementSpeed")]
    public float MovementSpeed;

    public bool IsMoving;
    public float LerpSpeed;
    public Animator Animator;
    public Rigidbody2D rigidbody2D;


    [SerializeField] private LevelManager Manager;

    [SerializeField] private WallFinder WallFinder;
    [SerializeField] private AbyssColliderChanger AbyssColliderChanger;
    public Hook Hook;
    private Vector2 movement;

    private bool previousMovement;

    public bool IsInAir => Hook.CurrentHookState == Hook.HookState.Hooking ||
                           Hook.CurrentHookState == Hook.HookState.OnWall;

    public Door LastVisitedDoor { get; set; }

    private void Start()
    {
        Manager.EnterTheFloor(this);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        Hook.SetPlayerWalkingMovement(movement);
        IsMoving = movement.sqrMagnitude > 0;
        if (IsMoving && Hook.CurrentHookState == Hook.HookState.NotHooking)
        {
            Animator.SetFloat(XDirection, movement.x);
            Animator.SetFloat(YDirection, movement.y);
        }

        if (Hook.CurrentHookState != Hook.HookState.NotHooking)
        {
            Animator.SetFloat(XDirection, Hook.GetHookDirection().x);
            Animator.SetFloat(YDirection, Hook.GetHookDirection().y);
        }

        Animator.SetBool(IsMovingHash, IsMoving);
        switch (IsMoving)
        {
            case true:
                if (!previousMovement) AudioManager.instance.Play("walk");

                break;
            case false:
                AudioManager.instance.StopPlaying("walk");
                break;
        }

        previousMovement = IsMoving;
        AbyssColliderChanger.SetAbyssTrigger(Hook.CurrentHookState != Hook.HookState.NotHooking);
    }


    private void FixedUpdate()
    {
        if (Hook.HookState.Hooking == Hook.CurrentHookState) movement *= Hook.GetFlySpeed();

        rigidbody2D.velocity = new Vector2(movement.x * MovementSpeed, movement.y * MovementSpeed);
    }

    public void Teleport()
    {
        StartCoroutine(StopMovement());
    }

    private void Move(Vector2 movementVector)
    {
        var previousPosition = transform.position;
        var movementDelta = new Vector3(movementVector.x * MovementSpeed * Time.deltaTime,
            movementVector.y * MovementSpeed * Time.deltaTime, 0);
        print(movementDelta);

        
        if (Hook.CurrentHookState == Hook.HookState.Hooking) movementDelta *= Hook.GetFlySpeed();

        var newPosition = previousPosition + movementDelta;

        transform.position = newPosition;
    }

    private IEnumerator StopMovement()
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