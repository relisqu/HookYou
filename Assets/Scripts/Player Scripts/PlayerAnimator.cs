using System;
using Assets.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player_Scripts
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
        private static readonly int XDirection = Animator.StringToHash("xDirection");
        private static readonly int YDirection = Animator.StringToHash("yDirection");
        private static readonly int SwordAttacked = Animator.StringToHash("swordAttacked");
        
        [SerializeField] private Animator Animator;
        [SerializeField] private Animator TransformAnimator;
        [SerializeField] private PlayerMovement PlayerMovement;
        [SerializeField] private Player Player;
        [SerializeField] private Hook Hook;
        [SerializeField] private SwordAttack Sword;
        [SerializeField] private Transform SwordRotator;
        
        [BoxGroup("Death animation")]
        [SerializeField] private Color DeathColor;
        [BoxGroup("Death animation")]
        [SerializeField] private float DeathFlickDuration;
        [BoxGroup("Death animation")]
        [SerializeField] private int DeathFlicksCount;
        private void OnEnable()
        {
            Sword.Attacked += PlayAttackAnimation;
            Hook.HookTouchedWall += PlayHookShrinkAnimation;
            Player.OnDied += PlayDieAnimation;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnDisable()
        {
            Sword.Attacked -= PlayAttackAnimation;
            Hook.HookTouchedWall -= PlayHookShrinkAnimation;
            Player.OnDied -= PlayDieAnimation;
        }

        void PlayAttackAnimation()
        {
             
            PlayerMovement.CreateSwordTrust(SwordRotator.right.normalized);
            TransformAnimator.SetTrigger(SwordAttacked);
        }

        void PlayHookShrinkAnimation()
        {
            TransformAnimator.SetTrigger(SwordAttacked);
        }

        void PlayDieAnimation()
        {
            _spriteRenderer.DOColor(DeathColor, DeathFlickDuration).SetLoops(DeathFlicksCount, LoopType.Yoyo).SetEase(Ease.OutQuint);
        }

        private void Update()
        {
            var isHooking = Hook.CurrentHookState == Hook.HookState.Hooking || Hook.CurrentHookState == Hook.HookState.OnWall;
            if (PlayerMovement.IsMoving && !Sword.IsAttackingVisually && !isHooking )
            {
                Animator.SetFloat(XDirection, PlayerMovement.GetMovement.x);
                Animator.SetFloat(YDirection, PlayerMovement.GetMovement.y);
            }

            if (isHooking)
            {
                Animator.SetFloat(XDirection, Hook.GetHookDirection().x);
                Animator.SetFloat(YDirection, Hook.GetHookDirection().y);
            }

            if (Sword.IsAttackingVisually)
            {
                Animator.SetFloat(XDirection, SwordRotator.right.x);
                Animator.SetFloat(YDirection, SwordRotator.right.y);
            }
            
            Animator.SetBool(IsMovingHash, PlayerMovement.IsMoving);
        }
        private SpriteRenderer _spriteRenderer;
    }
}