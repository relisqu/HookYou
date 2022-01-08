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


        private bool isCurrentlyOpened;

        private void OnEnable()
        {
            if (Type == DoorType.AlwaysOpened)
                Open();
            else
                TryClose();
            DoorLock.gameObject.SetActive(false);
            DoorAnimator.SetupDoor(Type == DoorType.AlwaysOpened);
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
            if (Type != DoorType.AlwaysClosed)
            {
                isCurrentlyOpened = true;
                DoorAnimator.SetupDoor(isCurrentlyOpened);
                DoorAnimator.SetOpened();
                SetUnblocked();
            }
        }


        public void GoThroughDoor(Player player)
        {
            player.LastVisitedDoor = ConnectedDoor;
            player.transform.position = ConnectedDoor.PlayerTeleportationPoint.position;
            ConnectedDoor.EnteredDoor?.Invoke(player);
            ExitedDoor?.Invoke(player);
        }

        public void TryClose()
        {
            if (Type != DoorType.AlwaysOpened)
            {
                isCurrentlyOpened = false;
                DoorAnimator.SetClosed();
                DoorAnimator.SetupDoor(isCurrentlyOpened);
            }
        }

        private enum DoorType
        {
            Deadend,
            Enemy,
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
    }
}