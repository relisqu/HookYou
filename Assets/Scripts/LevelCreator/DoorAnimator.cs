using System;
using UnityEngine;

namespace Assets.Scripts.LevelCreator
{
    public class DoorAnimator : MonoBehaviour
    {
        [SerializeField] private float Direction;

        [Header("Reference: ")] [SerializeField]
        private Animator Animator;

        private void OnEnable()
        {
            Animator.SetFloat(DirectionIndex,Direction);
        }

        public void SetClosed()
        {
            print("Closing");
            Animator.SetTrigger(CloseDoor);
        }

        public void SetOpened()
        {
            print("Openning");
            Animator.SetTrigger(OpenDoor);
        }

        public void SetupDoor(bool isOpenedFromBeginning)
        {
            print("Door is opened: "+isOpenedFromBeginning);
            Animator.SetBool(IsOpenedFromBeginning, isOpenedFromBeginning);
        }

        private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");
        private static readonly int CloseDoor = Animator.StringToHash("CloseDoor");
        private static readonly int IsOpenedFromBeginning = Animator.StringToHash("IsOpenedFromBeginning");
        private static readonly int DirectionIndex = Animator.StringToHash("Direction");
    }
}