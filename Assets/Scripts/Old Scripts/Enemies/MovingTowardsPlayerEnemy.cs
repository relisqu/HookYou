using UnityEngine;

namespace Grappling_Hook.Test
{
    public class MovingTowardsPlayerEnemy : Enemy
    {
        public float Speed;
        public LayerMask PlayerMask;
        public LayerMask SpikesLayer;
        private Vector3 lastPosition;
        private Transform Player;
        private float size;

        private void Awake()
        {
            lastPosition = transform.position;
            size = transform.localScale.x;
            var players = Physics2D.OverlapCircleAll(transform.position, 100f, PlayerMask);
            print("Trying to attack");
            foreach (var playerCollider in players)
                if (playerCollider.TryGetComponent(out Player _))
                    Player = playerCollider.transform;
        }

        private void FixedUpdate()
        {
            if (Player == null) return;
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Speed * Time.fixedDeltaTime);

            transform.localScale = lastPosition.x < transform.position.x
                ? new Vector3(-size, size, size)
                : new Vector3(size, size, size);
            lastPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player)) player.Die();
            if (SpikesLayer == (SpikesLayer | (1 << other.gameObject.layer))) GetDamage(2);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player)) player.Die();
            if (SpikesLayer == (SpikesLayer | (1 << other.gameObject.layer))) GetDamage(2);
        }
    }
}