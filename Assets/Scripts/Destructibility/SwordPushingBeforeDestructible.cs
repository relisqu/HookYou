using AI;
using Assets.Scripts;
using DG.Tweening;
using Player_Scripts;
using UnityEngine;

namespace Destructibility
{
    public class SwordPushingBeforeDestructible : SwordDestructible
    {
        [SerializeField] private float DeathImpulseRange;
        [SerializeField] private float DeathImpulseSpeed;
        private PlayerMovement Player;
        private BatMovementAnimator animator;

        protected override void ReactToSwordHit(SwordAttack sword)
        {
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (position-playerPosition) * DeathImpulseRange + position;
            Health.SetFakelyDied();
            StartCoroutine(animator.GetDamage());
            transform.DOMove(newPosition, 1/DeathImpulseSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                animator.SetNormalSprite();
                Health.TakeDamage(sword.GetDamage);
                StartCoroutine(MakeIFrame());
            });

        }
        private void Start()
        {
            Player = FindObjectOfType<PlayerMovement>();
            animator = GetComponent<BatMovementAnimator>();
        }
    }
}