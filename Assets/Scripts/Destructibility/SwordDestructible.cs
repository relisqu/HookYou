using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

namespace Destructibility
{
    public class SwordDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;
        private bool isAbleToAttack=true;

        private void OnTriggerEnter2D(Collider2D other)
        {
           if(isAbleToAttack && Health.IsAlive) TakeSwordDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(isAbleToAttack && Health.IsAlive) TakeSwordDamage(other);
        }

        private void TakeSwordDamage(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword) && Health.IsAlive)
            {
                if (sword.IsAttacking)
                {
                    Health.TakeDamage(sword.GetDamage);
                    sword.Hit();
                    StartCoroutine(MakeIFrame());
                }
            }
        }

        private IEnumerator MakeIFrame()
        {
            isAbleToAttack = false;
            yield return new WaitForFixedUpdate();
            isAbleToAttack = true;
        }
    }
}