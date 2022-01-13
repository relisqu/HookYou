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
        private DashingAI dashingAI;

        
        protected override void ReactToSwordHit(SwordAttack sword)
        {
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (position-playerPosition) * DeathImpulseRange + position;
            StartCoroutine(animator.GetDamage());
            dashingAI.DestroyMovingAction();
            transform.DOMove(newPosition, 1/DeathImpulseSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                
                print("Took damage. Health: "+Health.CurrentHealth);
                animator.SetNormalSprite();
                Health.TakeDamage(sword.GetDamage);
                StartCoroutine(MakeIFrame());
            });

        }

        public void SetSafeForPlayer()
        {
            ((EnemyHealth) Health).MarkAsDangerous(false);
        }

        private void Start()
        {
            Player = FindObjectOfType<PlayerMovement>();
            animator = GetComponent<BatMovementAnimator>();
            dashingAI = GetComponent<DashingAI>();
        }
    }
}