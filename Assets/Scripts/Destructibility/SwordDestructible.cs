using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

namespace Destructibility
{
    public class SwordDestructible : MonoBehaviour
    {
        [SerializeField] protected Health Health;
        private bool isAbleToAttack = true;
        private bool isImmuneToDamage = false;

        private void OnEnable()
        {
            isAbleToAttack = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            print(isAbleToAttack);
            if (isAbleToAttack && Health.IsAlive) TakeSwordDamage(other);
        }

        public void SetImmuneToDamage(bool value)
        {
            isImmuneToDamage = value;
        }

        private void TakeSwordDamage(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword) && Health.IsAlive && !isImmuneToDamage)
            {
                if (sword.IsAttacking)
                {
                    sword.Hit();
                    ReactToSwordHit(sword);
                }
            }
        }

        protected virtual void ReactToSwordHit(SwordAttack sword)
        {
            StartCoroutine(MakeIFrame());
            Health.TakeDamage(sword.GetDamage);
        }


        protected IEnumerator MakeIFrame()
        {
            isAbleToAttack = false;
            yield return new WaitForSeconds(0.5f);
            isAbleToAttack = true;
        }
    }
}