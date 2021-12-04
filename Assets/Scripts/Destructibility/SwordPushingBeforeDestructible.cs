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

        protected override void ReactToSwordHit(SwordAttack sword)
        {
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (position-playerPosition) * DeathImpulseRange + position;
            Health.TakeDamage(sword.GetDamage);
            transform.DOMove(newPosition, 1/DeathImpulseSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                StartCoroutine(MakeIFrame());
            });

        }
        private void Start()
        {
            Player = FindObjectOfType<PlayerMovement>();
        }
    }
}