using System;
using System.Collections;
using Destructibility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Player_Scripts;
using UnityEngine;

namespace AI
{
    public class DashingAI : MonoBehaviour
    {
        private PlayerMovement Player;
        private BatMovementAnimator animator;
        [SerializeField] private float Speed;
        [SerializeField] private float AttackRadius;

        [Tooltip("How far the AI will fly while dashing")] [SerializeField]
        private float DashRange;

        [Tooltip("How fast the AI will fly while dashing")] [SerializeField]
        private float DashSpeed;

        [SerializeField] private float PauseDuration;
        private bool isDashing;
        [SerializeField] private EnemyHealth Health;

        private bool isInRadius;
        private void OnEnable()
        {
            isDashing = false;
            dashTween.Kill();
            Health.Respawned += DestroyMovingAction;
            Health.Respawned += SetAsDangerous;
        }

        private void OnDisable()
        {
            Health.Respawned -= DestroyMovingAction;
            Health.Respawned -= SetAsDangerous;
        }

        public  void DestroyMovingAction()
        {
            StopAllCoroutines();
            dashTween.Kill();
            isDashing = false;
            animator.SetNormalSprite();
        }

        public void SetAsDangerous()
        {
            
          Health.MarkAsDangerous(true);
        }

        private void Update()
        {
            if (isDashing || !Health.IsAlive )
            {
                return;
            }
        
            var distance = Vector2.Distance(Player.transform.position, transform.position);
            isInRadius = distance > AttackRadius;
            animator.SetDashing(isDashing);
            if (isInRadius)
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
            animator.PrepareToDash();
            isDashing = true;
            yield return new WaitForSeconds(PauseDuration);
            animator.Dash();
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (playerPosition - position) * DashRange + position;
            if (!Health.IsDangerous || !Health.IsAlive) yield break;
            dashTween = transform.DOMove(newPosition, 1 / DashSpeed).SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    isDashing = false;
                });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }

        private void Awake()
        {
            Player = FindObjectOfType<PlayerMovement>();
            animator = GetComponent<BatMovementAnimator>();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> dashTween;
    }
}