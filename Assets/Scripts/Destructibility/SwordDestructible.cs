using System;
using System.Collections;
using Assets.Scripts;
using HookBlocks;
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
            if (Health.IsAlive) TakeSwordDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
          //  if (Health.IsAlive) TakeSwordDamage(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Health.IsAlive) TakeSwordDamage(other.collider);
        }

        public void SetImmuneToDamage(bool value)
        {
            isImmuneToDamage = value;
        }

        private void TakeSwordDamage(Collider2D other)
        {
            if (isAbleToAttack)
                print("Hit");
            if (isAbleToAttack && other.TryGetComponent(out SwordAttack sword) && Health.IsAlive && !isImmuneToDamage)
            {
                if (sword.IsAttacking)
                {
                    

                    print("AA");
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
            yield return new WaitForSeconds(0.2f);
            isAbleToAttack = true;
        }
    }
}