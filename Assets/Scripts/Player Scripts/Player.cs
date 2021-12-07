using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.LevelCreator;
using UnityEngine;

namespace Player_Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement PlayerMovement;
        [SerializeField] private LevelManager Manager;
        [SerializeField] private AbyssColliderChanger AbyssColliderChanger;
        [SerializeField] private PropsCollector PropsCollector;
        public Hook Hook;
        public Action OnDied; 

        public bool IsInAir => Hook.CurrentHookState == Hook.HookState.Hooking ||
                               Hook.CurrentHookState == Hook.HookState.OnWall;

        public Door LastVisitedDoor { get; set; }

        private void Awake()
        {
            Manager.EnterTheFloor(this);
        }

        private void Update()
        {
            AbyssColliderChanger.SetAbyssTrigger(Hook.CurrentHookState != Hook.HookState.NotHooking);
            if (Input.GetKeyDown(KeyCode.R) && !Manager.IsCurrentRoomCompleted(this))
            {
                Die();
            }
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
            Hook.ClearHook();
            OnDied?.Invoke();
            
        }

        private bool isTeleporting;

        public PropsCollector GetPropCollector()
        {
            return PropsCollector;
        }
    }
}