using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Destructibility
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private int InitialHealth;
        [SerializeField] public DeathAnimator Animator;
        [SerializeField] public List<UnityEvent> OnDeathMethods;
        [SerializeField] public List<UnityEvent> OnRespawnMethods;
        [SerializeField] public List<UnityEvent> OnDamageMethods;
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

        private void Update()
        {
            Animator.UpdateHealth(currentHealth);
        }

        private void Start()
        {
            foreach (var method in OnRespawnMethods)
            {
                Respawned += method.Invoke;
            }
            foreach (var method in OnDeathMethods)
            {
                Died += method.Invoke;
            }
            foreach (var method in OnDamageMethods)
            {
                TookDamage += method.Invoke;
            }
        }

        public void TakeDamage(int damage)
        {
            //Died += Respawn;
            if(!IsAlive) return;
            
            currentHealth -= Math.Abs(damage);
            if (currentHealth <= 0)
            {
                Die();
                Animator.PlayDeathAnimation();
                
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