using System;
using UnityEngine;

namespace Destructibility
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private int InitialHealth;
        [SerializeField] private DeathAnimator Animator;
        private int currentHealth;

        private void OnEnable()
        {
            currentHealth = InitialHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= Math.Abs(damage);
            if (currentHealth <= 0)
            {
                Animator.PlayDeathAnimation();
                Die();
            }
            else
            {
                Animator.PlayHitAnimation();
                
            }
        }

        public abstract void Die();
    }
}