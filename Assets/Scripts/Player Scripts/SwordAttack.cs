﻿using System.Collections;
using Grappling_Hook.Test;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    public class SwordAttack : MonoBehaviour
    {
        private static readonly int IsHitting = Animator.StringToHash("IsHitting");
        [FormerlySerializedAs("damage")] public float Damage;
        [FormerlySerializedAs("swordReload")] public float SwordReload;
        [FormerlySerializedAs("attackRange")] public float AttackRange;

        [Header("References: ")] [FormerlySerializedAs("attackPoint")]
        public Transform AttackPoint;

        [FormerlySerializedAs("swordHolder")] public Transform SwordHolder;
        [FormerlySerializedAs("enemyLayers")] public LayerMask EnemyLayers;
        [FormerlySerializedAs("animator")] public Animator Animator;
        private readonly Collider2D[] collidedEnemies = new Collider2D[20];

        private bool isAttacking;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                Animator.SetTrigger(IsHitting);
                StartCoroutine(SwingSword());
            }

            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var angle = AngleBetweenTwoPoints(SwordHolder.position, mousePosition);
            SwordHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        private void OnDrawGizmosSelected()
        {
            if (AttackPoint == null) return;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        }

        private IEnumerator SwingSword()
        {
            isAttacking = true;
            yield return new WaitForSeconds(SwordReload);
            isAttacking = false;
        }

        public void Attack()
        {
            Physics2D.OverlapCircleNonAlloc(AttackPoint.position, AttackRange, collidedEnemies, EnemyLayers);
            AudioManager.instance.Play("sword_attack");
            if (collidedEnemies[0] == null) return;
            foreach (var enemy in collidedEnemies)
            {
                if (enemy == null) return;

                if (enemy.TryGetComponent(out Boss boss))
                {
                    StartCoroutine(boss.ThrowAway(boss.GetDamage(), gameObject));
                }
                else if (enemy.TryGetComponent(out Enemy towardsPlayerEnemy))
                {
                    towardsPlayerEnemy.GetDamage(Damage);
                }
                else if (enemy.TryGetComponent(out BossBullet bullet))
                {
                    bullet.transform.rotation = Quaternion.Inverse(bullet.transform.rotation);
                    bullet.isDamaging = false;
                }
            }
        }
    }
}