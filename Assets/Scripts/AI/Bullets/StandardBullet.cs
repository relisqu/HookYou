using UnityEngine;

namespace AI.Bullets
{
    public class StandardBullet : Bullet
    {
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (WallsLayerMask == (WallsLayerMask | (1 << other.gameObject.layer)))
            {
                Health.TakeDamage(1);
            }
        }
    }
}