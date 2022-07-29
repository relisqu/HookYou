using System;
using AI;
using Assets.Scripts;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Obstacles;
using Player_Scripts;
using UnityEngine;

namespace Destructibility
{
    public class SwordPushingBeforeDestructible : SwordDestructible
    {
        [SerializeField] private float DeathImpulseRange;
        [SerializeField] private float DeathImpulseSpeed;
        [SerializeField] private AliveObstacle Obstacle;
        private PlayerMovement Player;
        private BatMovementAnimator animator;
        private DashingAI dashingAI;


        protected override void ReactToSwordHit(SwordAttack sword)
        {
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (position - playerPosition) * DeathImpulseRange + position;
            StartCoroutine(animator.GetDamage());
            dashingAI.DestroyMovingAction();
            bool isDying = Health.CurrentHealth <= 1;
            if (!isDying)
            {
                Health.TakeDamage(sword.GetDamage);
            }

            Obstacle.SetThrown(true);
            routine = transform.DOMove(newPosition, 1 / DeathImpulseSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                Obstacle.SetThrown(false);
                print("Took damage. Health: " + Health.CurrentHealth);
                animator.SetNormalSprite();
                if (isDying)
                {
                    Health.TakeDamage(sword.GetDamage);
                }

                StartCoroutine(MakeIFrame());
            });
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> routine;

        public void SetSafeForPlayer()
        {
            ((EnemyHealth) Health).MarkAsDangerous(false);
        }

        public void StopMoveRoutine()
        {
            routine?.Kill();
        }

        private void Start()
        {
            Player = FindObjectOfType<PlayerMovement>();
            animator = GetComponent<BatMovementAnimator>();
            dashingAI = GetComponent<DashingAI>();
            Obstacle.OnCollisionWithObstacle += StopMoveRoutine;
        }

        private void OnDestroy()
        {
            Obstacle.OnCollisionWithObstacle -= StopMoveRoutine;
        }
    }
}