using UnityEngine;

namespace Destructibility
{
    public class EnemyDestructible : MonoBehaviour
    {
        [SerializeField]private Health Health;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyHealth enemy))
            {
                if (!enemy.IsDangerous) return;
                Health.TakeDamage(1);
            }
        }
    }
}