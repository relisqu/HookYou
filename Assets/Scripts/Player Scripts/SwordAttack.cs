﻿using System.Collections;
using Destructibility;
using Grappling_Hook.Test;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    public class SwordAttack : MonoBehaviour
    {
        [FormerlySerializedAs("damage")] [SerializeField]
        private float Damage;

        [FormerlySerializedAs("swordReload")] [SerializeField]
        private float SwordReload;

        [FormerlySerializedAs("attackRange")] [SerializeField]
        private float AttackRange;

        [Header("References: ")] [FormerlySerializedAs("attackPoint")]
        public Transform AttackPoint;


        [FormerlySerializedAs("enemyLayers")] [SerializeField]
        private LayerMask EnemyLayers;

        [FormerlySerializedAs("animator")] [SerializeField]
        private Animator Animator;

        private bool isOddSwing;

        public bool IsAttacking => isAttacking;

        public bool StartedAttack => startedAttack;

        public int GetDamage => 1;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !startedAttack)
            {
                Animator.SetTrigger(IsHitting);
                Animator.SetBool(IsOddSwing,isOddSwing);
                isOddSwing = !isOddSwing;
                StartCoroutine(SwingSword());
            }

        }

        private void OnDrawGizmosSelected()
        {
            if (AttackPoint == null) return;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }
        
        public void StopAttack()
        {
            isAttacking = false; 
        }

        public void Attack()
        {
            AudioManager.instance.Play("sword_attack");
            isAttacking = true;
        }

        private IEnumerator SwingSword()
        {
            startedAttack = true;
            yield return new WaitForSeconds(SwordReload);
            startedAttack = false;
        }

        private bool startedAttack;
        private bool isAttacking;
        private static readonly int IsHitting = Animator.StringToHash("IsHitting");
        private static readonly int IsOddSwing = Animator.StringToHash("IsOddSwing");
    }
}