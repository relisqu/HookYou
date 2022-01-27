using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ShootingModule : MonoBehaviour
    {
        [SerializeField] private List<Bullet> Bullet;
        [SerializeField] private Transform ShootingPosition;
        [SerializeField] private Transform BulletParent;
        [SerializeField] private int BulletCount;

        private void Start()
        {
            _bullets = new List<Bullet>();
            Bullet tmp;
            foreach (var bullet in Bullet)
            {
                for (var i = 0; i < BulletCount; i++)
                {
                    tmp = Instantiate(bullet, BulletParent);
                    tmp.gameObject.SetActive(false);
                    _bullets.Add(tmp);
                }
            }
        }

        public Bullet GetPooledBullet<B>() where B : Bullet
        {
            for (var i = 0; i < _bullets.Count; i++)
            {
                if (!_bullets[i].gameObject.activeInHierarchy && _bullets[i].GetType() == typeof(B))
                    return _bullets[i];
            }

            return null;
        }

        public void Shoot<B>(float shotSpeed, float shotSize, Quaternion rotation) where B : Bullet
        {
            var bullet = GetPooledBullet<B>();
            bullet.Health.Respawn();
            if (bullet == null) return;

            bullet.transform.position = ShootingPosition.position;
            bullet.gameObject.SetActive(true);
            bullet.transform.rotation = rotation;
            bullet.SetStats(shotSpeed, shotSize);
            print("Bullet health: " + bullet.Health.CurrentHealth);
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