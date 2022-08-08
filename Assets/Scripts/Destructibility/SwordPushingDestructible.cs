using Assets.Scripts;
using Destructibility;
using Player_Scripts;
using UnityEngine;

namespace AI
{
    public class SwordPushingDestructible : SwordDestructible
    {
        [SerializeField] private float ImpulseForce;
        [SerializeField] private Rigidbody2D Rigidbody2D;
        private PlayerMovement _player;

        protected override void ReactToSwordHit(SwordAttack sword)
        {
            Health.TakeDamage(sword.GetDamage);
            var playerPosition = _player.transform.position;
            var newPosition = (transform.position - playerPosition).normalized;
            Rigidbody2D.AddForce(ImpulseForce * newPosition, ForceMode2D.Impulse);
            StartCoroutine(MakeIFrame());
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>();
        }
    }
}