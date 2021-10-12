using System.Collections;
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

        [FormerlySerializedAs("swordHolder")] [SerializeField]
        private Transform SwordHolder;

        [FormerlySerializedAs("enemyLayers")] [SerializeField]
        private LayerMask EnemyLayers;

        [FormerlySerializedAs("animator")] [SerializeField]
        private Animator Animator;


        public bool IsAttacking => isAttacking;
        public int GetDamage => 1;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !startedAttack)
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
        
        public void StopAttack()
        {
            isAttacking = false;
            StopAllCoroutines();
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

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        }

        private bool startedAttack;
        private Camera mainCamera;
        private bool isAttacking;
        private static readonly int IsHitting = Animator.StringToHash("IsHitting");
    }
}