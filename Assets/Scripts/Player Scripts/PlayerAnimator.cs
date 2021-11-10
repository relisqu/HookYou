using System;
using Assets.Scripts;
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
        [SerializeField] private SwordAttack Sword;
        [SerializeField] private Transform SwordRotator;
        private static readonly int SwordAttacked = Animator.StringToHash("swordAttacked");

        private void OnEnable()
        {
            Sword.Attacked += PlayAttackAnimation;
        }

        private void OnDisable()
        {
            Sword.Attacked -= PlayAttackAnimation;
        }

        void PlayAttackAnimation()
        {
            Animator.SetTrigger(SwordAttacked);
        }

        private void Update()
        {
            if (PlayerMovement.IsMoving && !Sword.IsAttacking&& Hook.CurrentHookState == Hook.HookState.NotHooking)
            {
                Animator.SetFloat(XDirection, PlayerMovement.GetMovement.x);
                Animator.SetFloat(YDirection, PlayerMovement.GetMovement.y);
            }

            if (Hook.CurrentHookState != Hook.HookState.NotHooking)
            {
                Animator.SetFloat(XDirection, Hook.GetHookDirection().x);
                Animator.SetFloat(YDirection, Hook.GetHookDirection().y);
            }

            if (Sword.StartedAttack)
            {
                Animator.SetFloat(XDirection, SwordRotator.right.x);
                Animator.SetFloat(YDirection, SwordRotator.right.y);
            }

            Animator.SetBool(IsMovingHash, PlayerMovement.IsMoving);
        }
    }
}