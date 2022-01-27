using System;
using System.Collections;
using UnityEngine;

namespace AI.Bullets
{
    public class TransparentBullet : Bullet
    {
        [SerializeField] private float MaxFlyDuration;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (WallsLayerMask == (WallsLayerMask | (1 << other.gameObject.layer)))
            {
                Health.TakeDamage(1);
            }
        }
        private void Start()
        {
            StartCoroutine(DestroyAfterCd());
        }

        private IEnumerator DestroyAfterCd()
        {
            yield return new WaitForSeconds(MaxFlyDuration);
            gameObject.SetActive(false);
        }
    }
}