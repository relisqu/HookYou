using System;
using UnityEngine;

namespace Destructibility
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private int InitialHealth;
        [SerializeField] private DeathAnimator Animator;
        private int currentHealth;

        public int CurrentHealth
        {
            get => currentHealth;
        }

        public bool IsAlive => currentHealth>0;
        public Action Died { get; set; }
        public Action Respawned { get; set; }
        public Action TookDamage { get; set; }

        private void OnEnable()
        {
            currentHealth = InitialHealth;
        }

        public void TakeDamage(int damage)
        {
            if(!IsAlive) return;
            
            currentHealth -= Math.Abs(damage);
            if (currentHealth <= 0)
            {
                Animator.PlayDeathAnimation();
                Die();
                
            }
            else
            {
               Animator?.PlayHitAnimation();
               TookDamage?.Invoke();    
            }
        }

        public abstract void Die();

        public void Respawn()
        {
            currentHealth = InitialHealth;
            Respawned?.Invoke();
            Animator.PlayRespawnAnimation();
        }

    }
}