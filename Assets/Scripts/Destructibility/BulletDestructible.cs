using System;
using UnityEngine;

namespace Destructibility
{
    public class BulletDestructible : MonoBehaviour
    {
        [SerializeField]private Health Health;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                Health.TakeDamage(bullet.GetDamage);
            }
        }
    }
}