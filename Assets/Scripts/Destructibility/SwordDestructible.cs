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
        private int SwordAttackId;
        public Action TookSwordHit;


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
            if (Health.IsAlive) TakeSwordDamage(other);
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
            if (isAbleToAttack && other.TryGetComponent(out SwordAttack sword) && Health.IsAlive && !isImmuneToDamage)
            {
                if (sword.IsAttacking)
                {
                    if (sword.SwingId != SwordAttackId)
                    {
                        TookSwordHit?.Invoke();
                        sword.Hit();
                        ReactToSwordHit(sword);
                    }

                    SwordAttackId = sword.SwingId;
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