using System.Collections;
using UnityEngine;

namespace Grappling_Hook.Test
{
    public class ShootingEnemy : MonoBehaviour
    {
        public Player foundPlayer;
        public float reload;
        public float shotSpeed;
        public float shotSize;
        public CanonModule shooter;
        public Animator animator;
        public LayerMask ignoreLayers;
        public int wallLayer;
        public ParticleSystem shootingParticles;
        public string shootingSoundName;

        private void Start()
        {
            //Animator.Speed = 1 / reload;
            StartCoroutine(Shoot());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player)) foundPlayer = player;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player)) foundPlayer = null;
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                if (foundPlayer != null)
                    if (PlayerIsSeen(foundPlayer))
                    {
                        AudioManager.instance.Play(shootingSoundName);
                        animator.SetTrigger("IsShooting");
                    }

                yield return new WaitForSeconds(reload);
            }

            yield return new WaitForSeconds(0.01f);
        } //

        public void ShootBullet()
        {
            if (foundPlayer != null)
            {
                var target = foundPlayer.transform.position;
                var angle = AngleBetweenTwoPoints(transform.position, target);
                var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
                shooter.Shoot(shotSpeed, shotSize, rotation);
            }
        }

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        }

        private bool PlayerIsSeen(Player player)
        {
            var distance = player.transform.position - transform.position;
            var _hit = Physics2D.Raycast(transform.position, distance.normalized,
                Mathf.Infinity, ignoreLayers);
            print(_hit.transform.gameObject.layer);
            return _hit.transform.gameObject.layer != wallLayer;
        }

        public void CreateParticles()
        {
            shootingParticles.Play();
        }
    }
}