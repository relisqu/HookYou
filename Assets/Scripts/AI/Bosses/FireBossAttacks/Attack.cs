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
        

        protected Animator Animator;

        private void Start()
        {
            Animator = GetComponentInParent<Animator>();
        }

        protected void PlayAttackAnimation()
        {
            if (AttackNameTrigger != null) Animator.SetTrigger(AttackNameTrigger);
        }
        protected void PlayAttackAnimation(string attack)
        {
            if (AttackNameTrigger != null) Animator.SetTrigger(attack);
        }
    }
}