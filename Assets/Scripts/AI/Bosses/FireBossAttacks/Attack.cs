using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Attack : MonoBehaviour
    {
        [SerializeField] protected string AttackNameTrigger;
        public abstract IEnumerator StartAttack();
        public abstract Attack GetCurrentAttack();

        protected Animator Animator;
        protected FireBoss Boss;

        private void Start()
        {
            Animator = GetComponentInParent<Animator>();
            Boss = GetComponentInParent<FireBoss>();
        }

        protected void PlayAttackAnimation()
        {
            if (AttackNameTrigger != null && IsAttackAnimationValid()) Animator.SetTrigger(AttackNameTrigger);
        }

        protected void PlayAttackAnimation(string attack)
        {
            if (AttackNameTrigger != null && IsAttackAnimationValid()) Animator.SetTrigger(attack);
        }

        public bool IsAttackAnimationValid()
        {
            return (Boss.GetCurrentAttack() == null || this == Boss.GetCurrentAttack().GetCurrentAttack()  );
        }
    }
}