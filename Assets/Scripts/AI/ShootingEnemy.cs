using System;
using System.Collections;
using AI.Bullets;
using Destructibility;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class ShootingEnemy : EnemyBehaviour
    {
        [SerializeField] private LayerMask IgnoreLayers;
        [SerializeField] private LayerMask WallsLayers;

        [SerializeField] private int ChargeBulletsCount;
        [SerializeField] private float SmallChargeReload;
        [SerializeField] private float BigChargeReload;

        [FormerlySerializedAs("Distance")] [SerializeField] private float DetectDistance;

        [SerializeField] private float BulletSpeed;

        [BoxGroup("References")] [SerializeField]
        private ShootingModule ShootingModule;

        [BoxGroup("References")] [SerializeField]
        private Animator Animator;

        [BoxGroup("References")] [SerializeField]
        private Health Health;

        [BoxGroup("References")] [SerializeField]
        private GrappleZone GrappleZone;


        public void StopShooting()
        {
            StopAllCoroutines();
            StartCoroutine(StunCoroutine());
        }

        private IEnumerator StunCoroutine()
        {
            isStunned = true;
            Animator.SetTrigger("Idle");
            yield return new WaitForSeconds(StunDuration);
            if (!Health.IsAlive) yield break;
            isStunned = false;
            StartCoroutine(StartShootingBehaviour());
        }

        private void OnEnable()
        {
            _player = FindObjectOfType<Player>();
            StopAllCoroutines();
            StartCoroutine(StartShootingBehaviour());
            Stunned += StopShooting;
        }

        private void OnDisable()
        {
            Stunned -= StopShooting;
        }

        private bool PlayerIsInRange(Player player)
        {
            var distance = player.transform.position - transform.position;
            if (distance.magnitude > DetectDistance)
            {
                return false;
            }

            var hit = Physics2D.Raycast(transform.position, distance.normalized,
                Mathf.Infinity, IgnoreLayers);
            if (hit.transform == null) return false;
            var layerIsWall = (WallsLayers == (WallsLayers | (1 << hit.transform.gameObject.layer)));
            return !layerIsWall;
        }

        private IEnumerator StartShootingBehaviour()
        {
            GrappleZone.DisableCollider();
            while (true)
            {
                if (PlayerIsInRange(_player))
                {
                    yield return Shoot();
                }

                yield return new WaitForSeconds(0.01f);
            }

            yield return null;
        }

        public void ShootBullet()
        {
            if (_player != null)
            {
                var target = _player.transform.position;
                var angle = GetAngleBetweenTwoPoints(transform.position, target);
                var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
                ShootingModule.Shoot<StandardBullet>(BulletSpeed, 1, rotation);
            }
        }

        private IEnumerator Shoot()
        {
            for (var i = 0; i < ChargeBulletsCount; i++)
            {
                if (PlayerIsInRange(_player))
                {
                    Animator.SetTrigger(Shoot1);
                    StartCoroutine(GrappleZone.ActivateGrappleCollider());
                }

                if (i < ChargeBulletsCount) yield return new WaitForSeconds(SmallChargeReload);
            }

            yield return new WaitForSeconds(BigChargeReload);
        }

        private float GetAngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, DetectDistance);
        }

        private Player _player;
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");
    }
}