using System;
using Assets.Scripts;
using UnityEngine;

namespace Destructibility
{
    public class SwordDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;

        private void OnTriggerEnter2D(Collider2D other)
        {
            TakeSwordDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TakeSwordDamage(other);
        }

        private void TakeSwordDamage(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword))
            {
                if (sword.IsAttacking) Health.TakeDamage(sword.GetDamage);
            }
        }
    }
}