using System;
using UnityEngine;

namespace Destructibility
{
    public class EnemyDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;

        private void OnTriggerEnter2D(Collider2D other)
        {
            DieFromEnemy(other.gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            
            DieFromEnemy(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            DieFromEnemy(other.gameObject);
        }

        public void DieFromEnemy(GameObject obj)
        {
            if (obj.TryGetComponent(out EnemyHealth enemy))
            {
                if (!enemy.IsDangerous) return;
                Health.TakeDamage(1);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            DieFromEnemy(other.gameObject);
        }
    }
}