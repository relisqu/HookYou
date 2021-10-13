using System.Collections;
using Assets.Scripts;
using Assets.Scripts.LevelCreator;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{


    public bool IsMoving;
    public Animator Animator;
    [SerializeField] private PlayerMovement PlayerMovement;


    [SerializeField] private LevelManager Manager;

    [SerializeField] private WallFinder WallFinder;
    [SerializeField] private AbyssColliderChanger AbyssColliderChanger;
    public Hook Hook;


    public bool IsInAir => Hook.CurrentHookState == Hook.HookState.Hooking ||
                           Hook.CurrentHookState == Hook.HookState.OnWall;

    public Door LastVisitedDoor { get; set; }

    private void Start()
    {
        Manager.EnterTheFloor(this);
    }

    private void Update()
    {
        AbyssColliderChanger.SetAbyssTrigger(Hook.CurrentHookState != Hook.HookState.NotHooking);
    }

    public void Teleport()
    {
        StartCoroutine(StopMovement());
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
    
    private bool isTeleporting;
}