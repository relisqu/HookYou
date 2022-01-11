using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ShootingModule : MonoBehaviour
    {
        [SerializeField] private Bullet Bullet;
        [SerializeField] private Transform ShootingPosition;
        [SerializeField] private Transform BulletParent;
        [SerializeField] private int BulletCount;

        private void Start()
        {
            _bullets = new List<Bullet>();
            Bullet tmp;
            for (var i = 0; i < BulletCount; i++)
            {
                tmp = Instantiate(Bullet,BulletParent);
                tmp.gameObject.SetActive(false);
                _bullets.Add(tmp);
            }
        }

        public Bullet GetPooledBullet()
        {
            for (var i = 0; i < BulletCount; i++)
                if (!_bullets[i].gameObject.activeInHierarchy)
                    return _bullets[i];

            return null;
        }
        public void Shoot(float shotSpeed, float shotSize, Quaternion rotation)
        {
            var bullet = GetPooledBullet();
            bullet.Health.Respawn();
            if (bullet == null) return;

            bullet.transform.position = ShootingPosition.position;
            bullet.transform.rotation = rotation;
            bullet.SetStats(shotSpeed, shotSize);
            bullet.gameObject.SetActive(true);
            print("Bullet health: "+ bullet.Health.CurrentHealth);
        }
        private void OnDestroy()
        {
            if (_bullets == null) return;
            foreach (var bullet in _bullets)
                if (bullet != null)
                    Destroy(bullet.gameObject);
            
        }

        private List<Bullet> _bullets;
    }
}