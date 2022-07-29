using System;
using Destructibility;
using UnityEngine;

namespace Obstacles
{
    public class AliveObstacle : Obstacle
    {
        [SerializeField] private float ThrowbackForce;
        [SerializeField] private Health Health;
        private bool _isThrown;
        public Action OnCollisionWithObstacle;

        public void SetThrown(bool value)
        {
            _isThrown = value;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            CollideWithObstacle(other.gameObject);
        }

        private void CollideWithObstacle(GameObject collideObject)
        {
            if (_isThrown && collideObject.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.TakeDamage();
                Health.TakeDamage(1);
                print("Damaged "+obstacle.name);
                OnCollisionWithObstacle?.Invoke();
            }
        }

        public override void TakeDamage()
        {
            Health.TakeDamage(1);
            _isThrown = false;
        }
    }
}