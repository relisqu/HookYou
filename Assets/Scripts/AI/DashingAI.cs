using System;
using System.Collections;
using Destructibility;
using DG.Tweening;
using Player_Scripts;
using UnityEngine;

namespace AI
{
    public class DashingAI : MonoBehaviour
    {
        private PlayerMovement Player;
        [SerializeField] private float Speed;
        [SerializeField] private float AttackRadius;

        [Tooltip("How far the AI will fly while dashing")] [SerializeField]
        private float DashRange;

        [Tooltip("How fast the AI will fly while dashing")] [SerializeField]
        private float DashSpeed;

        [SerializeField] private float PauseDuration;
        private bool isAttacking;
        [SerializeField] private EnemyHealth Health;

        private void Update()
        {
            if (isAttacking || !Health.IsAlive)
            {
                return;
            }

            var distance = Vector2.Distance(Player.transform.position, transform.position);
            if (distance > AttackRadius)
            {
                var newDistance =
                    Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
                transform.position = newDistance;
            }
            else
            {
                StartCoroutine(Dash());
            }
        }

        IEnumerator Dash()
        {
            isAttacking = true;
            yield return new WaitForSeconds(PauseDuration);
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (playerPosition - position) * DashRange + position;
            if (!Health.IsAlive)
            {
                isAttacking = false;
                StopAllCoroutines();
                yield break;
            }

            transform.DOMove(newPosition, 1 / DashSpeed).SetEase(Ease.OutCubic)
                .OnComplete(() => { isAttacking = false; });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }

        private void Start()
        {
            Player = FindObjectOfType<PlayerMovement>();
        }
    }
}