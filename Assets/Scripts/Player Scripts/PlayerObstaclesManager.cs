using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerObstaclesManager : MonoBehaviour
    {
        [SerializeField] private Player Player;
        [SerializeField] private LayerMask ObstaclesMask;
        [SerializeField] private LayerMask AbyssMask;
        [SerializeField] private Collider2D AbyssCollider;
        private bool WasEnteringFromEdge;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (ObstaclesMask == (ObstaclesMask | (1 << other.gameObject.layer))) Player.Die();

            if (other.gameObject.TryGetComponent(out BossBullet bullet) && bullet.isDamaging)
            {
                bullet.gameObject.SetActive(false);
                Player.Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ObstaclesMask == (ObstaclesMask | (1 << other.gameObject.layer))) Player.Die();

            if (other.TryGetComponent(out BossBullet bullet) && bullet.isDamaging)
            {
                bullet.gameObject.SetActive(false);
                Player.Die();
            }

            if (AbyssMask == (AbyssMask | (1 << other.gameObject.layer)) && AbyssCollider.isTrigger)
            {
                if (Player.IsInAir) return;
                Player.Die();
            }
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (AbyssMask == (AbyssMask | (1 << other.gameObject.layer)) && AbyssCollider.isTrigger)
            {
                if (Player.IsInAir) return;
                Player.Die();
            }
        }
    }
}