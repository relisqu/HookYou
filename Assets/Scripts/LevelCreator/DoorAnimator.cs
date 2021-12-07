using System;
using UnityEngine;

namespace LevelCreator
{
    public class DoorAnimator : MonoBehaviour
    {
        private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");
        private static readonly int CloseDoor = Animator.StringToHash("CloseDoor");
        private static readonly int IsOpenedFromBeginning = Animator.StringToHash("IsOpenedFromBeginning");
        private static readonly int DirectionXIndex = Animator.StringToHash("xDirection");
        private static readonly int DirectionYIndex = Animator.StringToHash("yDirection");
        [SerializeField] private DoorDirectionType Direction;

        [Header("Reference: ")] [SerializeField]
        private Animator Animator;

        private void OnEnable()
        {
            var xDirection = 0;
            var yDirection = 0;
            switch (Direction)
            {
                case DoorDirectionType.Left:
                    xDirection = 1;
                    break;
                case DoorDirectionType.Right:

                    xDirection = -1;
                    break;
                case DoorDirectionType.Top:
                    yDirection = 1;
                    break;
                case DoorDirectionType.Down:
                    yDirection = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Animator.SetFloat(DirectionXIndex, xDirection);
            Animator.SetFloat(DirectionYIndex, yDirection);
        }

        public void SetClosed()
        {
            Animator.SetTrigger(CloseDoor);
        }

        public void SetOpened()
        {
            Animator.SetTrigger(OpenDoor);
        }

        public void SetupDoor(bool isOpenedFromBeginning)
        {
            Animator.SetBool(IsOpenedFromBeginning, isOpenedFromBeginning);
        }

        private enum DoorDirectionType
        {
            Left,
            Right,
            Top,
            Down
        }
    }
}