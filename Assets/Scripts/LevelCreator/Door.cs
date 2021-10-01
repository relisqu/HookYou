using System;
using LevelCreator;
using UnityEngine;

namespace Assets.Scripts.LevelCreator
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Door ConnectedDoor;
        [SerializeField] private DoorType Type;
        [SerializeField] private Transform PlayerTeleportationPoint;
        [SerializeField] private DoorAnimator DoorAnimator;
        public Action<Player> EnteredDoor;
        public Action<Player> ExitedDoor;


        private bool isCurrentlyOpened;

        private void OnEnable()
        {
            if (Type == DoorType.AlwaysOpened)
                Open();
            else
                TryClose();

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
            Boss,
            Enemy,
            Runner,
            AlwaysOpened,
            AlwaysClosed
        }
    }
}