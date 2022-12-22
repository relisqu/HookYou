using System;
using LevelCreator;
using Player_Scripts;
using UnityEngine;

namespace Assets.Scripts.LevelCreator
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Door ConnectedDoor;
        [SerializeField] private DoorType Type;
        [SerializeField] private Transform PlayerTeleportationPoint;
        [SerializeField] private DoorAnimator DoorAnimator;
        [SerializeField] private DoorLock DoorLock;
        public Action<Player> EnteredDoor;
        public Action<Player> ExitedDoor;

        public bool IsCurrentlyOpened => isCurrentlyOpened;

        private bool isCurrentlyOpened;

        private void OnEnable()
        {
            if (Type == DoorType.AlwaysOpened)
                Open();
            else
                ManuallyClose();
            DoorLock.gameObject.SetActive(false);
            DoorLock.LockDestroyed += RemoveLock;
            DoorAnimator.SetupDoor(Type == DoorType.AlwaysOpened);
        }

        public void RemoveLock()
        {
            Open();
            hadLock = true;
        }

        bool hadLock = false;

        private void OnDisable()
        {
            DoorLock.LockDestroyed -= RemoveLock;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!isCurrentlyOpened || !other.gameObject.TryGetComponent(out Player player)) return;
            GoThroughDoor(player);
        }


        public Vector3 GetTeleportationPoint()
        {
            return PlayerTeleportationPoint.position;
        }

        public void Open()
        {
            if (Type != DoorType.AlwaysClosed && Type != DoorType.Manual)
            {
                ManuallyOpen();
            }
        }

        public void ManuallyOpen()
        {
            isCurrentlyOpened = true;
            DoorAnimator.SetupDoor(isCurrentlyOpened);
            DoorAnimator.SetOpened();
            SetUnblocked();
        }

        public void ManuallyClose()
        {
            isCurrentlyOpened = false;
            DoorAnimator.SetClosed();
            DoorAnimator.SetupDoor(isCurrentlyOpened);
            if (hadLock)
                SetBlocked();
        }

        public void GoThroughDoor(Player player)
        {
            player.LastVisitedDoor = ConnectedDoor;
            player.transform.position = ConnectedDoor.PlayerTeleportationPoint.position;
            player.Hook.ClearHook();
            ConnectedDoor.EnteredDoor?.Invoke(player);
            ExitedDoor?.Invoke(player);
        }

        public void TryClose()
        {
            if (Type != DoorType.AlwaysOpened && Type != DoorType.Manual)
            {
                ManuallyClose();
            }
        }

        public enum DoorType
        {
            Deadend,
            Enemy,
            Manual,
            AlwaysOpened,
            AlwaysClosed
        }

        public void SetBlocked()
        {
            DoorLock.SetLock();
            isBlocked = true;
        }

        public void SetUnblocked()
        {
            if (!isBlocked) return;
            DoorLock.DropLock();
            isBlocked = false;
        }

        private bool isBlocked;

        public DoorAnimator GetDoorAnimator()
        {
            return DoorAnimator;
        }

        public Door GetConnectedDoor()
        {
            return ConnectedDoor;
        }

        public void SetConnectedDoor(Door doorConnectedDoor)
        {
            ConnectedDoor = doorConnectedDoor;
        }

        public void SetType(DoorType doorType)
        {
            Type = doorType;
        }
    }
}