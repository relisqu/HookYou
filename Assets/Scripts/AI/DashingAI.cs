using System;
using System.Collections;
using Destructibility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class DashingAI : MonoBehaviour
    {
        [BoxGroup("Default constrains")] [SerializeField]
        protected float Speed;

        [BoxGroup("Default constrains")] [SerializeField]
        protected float AttackRadius;

        [Tooltip("How far the AI will fly while dashing")] [SerializeField] [BoxGroup("Default constrains")]
        protected float DashRange;

        [Tooltip("How fast the AI will fly while dashing")] [SerializeField] [BoxGroup("Default constrains")]
        protected float DashSpeed;

        [BoxGroup("Default constrains")] [SerializeField]
        protected float PauseDuration;

        [BoxGroup("References")] [SerializeField]
        protected EnemyHealth Health;

        [BoxGroup("References")] [SerializeField]
        private PopupVFX BatWarningVfx;

        private void OnEnable()
        {
            _isDashing = false;
            dashTween.Kill();
            Health.Respawned += DestroyMovingAction;
            Health.Respawned += SetAsDangerous;
            Health.TookDamage += UpdateHealthSprite;
            UpdateHealthSprite();
        }

        private void UpdateHealthSprite()
        {
            _animator.UpdateHealth(Health.CurrentHealth);
        }

        private void OnDisable()
        {
            Health.Respawned -= DestroyMovingAction;
            Health.Respawned -= SetAsDangerous;
            Health.TookDamage -= UpdateHealthSprite;
        }

        public void DestroyMovingAction()
        {
            StopAllCoroutines();
            dashTween.Kill();
            UpdateHealthSprite();
            _isDashing = false;
            _animator.SetNormalSprite();
        }

        public void SetAsDangerous()
        {
            Health.MarkAsDangerous(true);
        }

        private void Update()
        {
            if (_isDashing || !Health.IsAlive)
            {
                return;
            }

            var distance = Vector2.Distance(_player.transform.position, transform.position);
            _isInRadius = distance > AttackRadius;
            _animator.SetDashing(_isDashing);
            if (_isInRadius)
            {
                var newDistance =
                    Vector2.MoveTowards(transform.position, _player.transform.position, Speed * Time.deltaTime);
                transform.position = newDistance;
            }
            else
            {
                StartCoroutine(Dash());
            }
        }

        IEnumerator Dash()
        {
            _animator.PrepareToDash();
            _isDashing = true;
            BatWarningVfx.InitiateObject();
            yield return new WaitForSeconds(PauseDuration);
            _animator.Dash();
            var position = transform.position;
            var playerPosition = _player.transform.position;

            var distance = (playerPosition - position).normalized * DashRange;
            if (!Health.IsDangerous || !Health.IsAlive) yield break;
            dashTween = transform.DOMove(position + distance, 1 / DashSpeed * 0.1f * distance.magnitude)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => { _isDashing = false; });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DashRange);
        }

        private void Awake()
        {
            _player = FindObjectOfType<PlayerMovement>();
            _animator = GetComponent<BatMovementAnimator>();
        }

        private PlayerMovement _player;
        private BatMovementAnimator _animator;
        private bool _isDashing;

        private bool _isInRadius;
        private TweenerCore<Vector3, Vector3, VectorOptions> dashTween;
    }
}