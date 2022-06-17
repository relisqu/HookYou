using System;
using UnityEngine;

namespace Destructibility
{
    public class EnemyDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyHealth enemy))
            {
                print("Died by enemy: " + enemy.IsDangerous + " " + Health.CurrentHealth);
                if (!enemy.IsDangerous) return;
                Health.TakeDamage(1);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                print("Died by enemy: " + enemy.IsDangerous + " " + Health.CurrentHealth);
                if (!enemy.IsDangerous) return;
                Health.TakeDamage(1);
            }
        }
    }
}