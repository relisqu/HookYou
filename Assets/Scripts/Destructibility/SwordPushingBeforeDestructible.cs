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
        [SerializeField] private PlayerMovement Player;

        protected override void ReactToSwordHit(SwordAttack sword)
        {
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (position-playerPosition) * DeathImpulseRange + position;
            transform.DOMove(newPosition, 1/DeathImpulseSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                Health.TakeDamage(sword.GetDamage);
                StartCoroutine(MakeIFrame());
            });

        }
    }
}