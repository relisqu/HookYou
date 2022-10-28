using System;
using System.Collections;
using Destructibility;
using Grappling_Hook.Test;
using Player_Scripts;
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
        [SerializeField]private SwordHitVFX SwordHitVFX;

        private bool isOddSwing;
        private bool isVisuallyAttacking;

        public bool IsAttacking => isAttacking;

        public bool StartedAttack => startedAttack;
        public bool IsAttackingVisually => isVisuallyAttacking;

        public int GetDamage => 1;

        public Action<float> Attacked;
        public ParticleSystem EmitParticles;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var time = Time.time - AttackStartTime;
                switch (startedAttack)
                {
                    case true when (SwordReload-time<0.25f && !_createdCombo):
                        Animator.SetTrigger(IsHitting);
                        Animator.SetBool(IsOddSwing,isOddSwing);
                        isOddSwing = !isOddSwing;
                        StartCoroutine(SwingSword(3f));
                        print("Combo");
                        _createdCombo = true;
                        break;
                    case false:
                        _createdCombo = false;
                        Animator.SetTrigger(IsHitting);
                        Animator.SetBool(IsOddSwing,isOddSwing);
                        isOddSwing = !isOddSwing;
                        StartCoroutine(SwingSword(1));
                        break;
                }
            }

        }

        private bool _createdCombo;
        private void OnDrawGizmosSelected()
        {
            if (AttackPoint == null) return;
            //Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
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

        public void StopVisualAttack()
        {
            isVisuallyAttacking = false;
        }

        public void Hit()
        {
            SwordHitVFX.Hit();
        }

        private IEnumerator SwingSword(float thrust)
        {
            Attacked?.Invoke(thrust);
            isVisuallyAttacking = true;
            startedAttack = true;
            AttackStartTime = Time.time;
            yield return new WaitForSeconds(SwordReload);
            startedAttack = false;
        }

        private float AttackStartTime;
        private bool startedAttack;
        private bool isAttacking;
        private static readonly int IsHitting = Animator.StringToHash("IsHitting");
        private static readonly int IsOddSwing = Animator.StringToHash("IsOddSwing");

        public void GetAttackDirection()
        {
            throw new System.NotImplementedException();
        }

        public bool IsInAttackRange(Transform componentTransform)
        {
            return Vector2.Distance(componentTransform.position, transform.position) <= AttackRange*1.5f;
        }
    }
}