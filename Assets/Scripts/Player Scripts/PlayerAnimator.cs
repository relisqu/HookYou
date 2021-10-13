using System;
using UnityEngine;

namespace Player_Scripts
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
        private static readonly int XDirection = Animator.StringToHash("xDirection");
        private static readonly int YDirection = Animator.StringToHash("yDirection");
        [SerializeField] private Animator Animator;
        [SerializeField] private PlayerMovement PlayerMovement;
        [SerializeField] private Hook Hook;

        private void Update()
        {
            if (PlayerMovement.IsMoving && Hook.CurrentHookState == Hook.HookState.NotHooking)
            {
                Animator.SetFloat(XDirection, PlayerMovement.GetMovement.x);
                Animator.SetFloat(YDirection, PlayerMovement.GetMovement.y);
            }

            if (Hook.CurrentHookState != Hook.HookState.NotHooking)
            {
                Animator.SetFloat(XDirection, Hook.GetHookDirection().x);
                Animator.SetFloat(YDirection, Hook.GetHookDirection().y);
            }

            Animator.SetBool(IsMovingHash, PlayerMovement.IsMoving);
        }
    }
}