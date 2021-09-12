using System;
using Assets.Scripts.Old_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grappling_Hook.Test
{
    public abstract class Enemy : MonoBehaviour
    {
        [FormerlySerializedAs("health")] public float Health;
        public Action EnemyDied;
        private Vector3 firstPosition;
        

        private void OnEnable()
        {
            firstPosition = transform.position;
        }
        
        public void GetDamage(float damage)
        {
            
            Health -= damage;
            if (Health <= 0)
            {
                EnemyDied?.Invoke();
                Die();
            }

        }
        public void Die()
        {
            print("Mouse died");
            gameObject.SetActive(false);
        }

        public void EnableEnemy()
        {
            transform.position = firstPosition;
            gameObject.SetActive(true);
            
        }

        public void DisableEnemy()
        {
            gameObject.SetActive(false);
        }
    }
}