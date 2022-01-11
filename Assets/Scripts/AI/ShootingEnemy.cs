using System;
using System.Collections;
using Player_Scripts;
using UnityEngine;

namespace AI
{
    public class ShootingEnemy : MonoBehaviour
    {
        [SerializeField] private LayerMask IgnoreLayers;
        [SerializeField] private LayerMask WallsLayers;

        [SerializeField] private int ChargeBulletsCount;
        [SerializeField] private float SmallChargeReload;
        [SerializeField] private float BigChargeReload;


        [SerializeField] private float BulletSpeed;
        [SerializeField] private ShootingModule ShootingModule;
        [SerializeField] private Animator Animator;


        [SerializeField] private float Distance;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            StartCoroutine(StartShootingBehaviour());
        }

        private bool PlayerIsInRange(Player player)
        {
            var distance = player.transform.position - transform.position;
            if (distance.magnitude > Distance)
            {
                return false;
            }

            var hit = Physics2D.Raycast(transform.position, distance.normalized,
                Mathf.Infinity, IgnoreLayers);

            var layerIsWall = (WallsLayers == (WallsLayers | (1 << hit.transform.gameObject.layer)));
            return !layerIsWall;
        }

        private IEnumerator StartShootingBehaviour()
        {
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
                ShootingModule.Shoot(BulletSpeed, 1, rotation);
            }
        }

        private IEnumerator Shoot()
        {
            for (var i = 0; i < ChargeBulletsCount; i++)
            {
                if (PlayerIsInRange(_player))
                {
                    Animator.SetTrigger(Shoot1);
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
            Gizmos.color=Color.green;
            
            Gizmos.DrawWireSphere(transform.position,Distance);
        }

        private Player _player;
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");
    }
}