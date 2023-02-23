using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Obstacles
{
    public class MovingSpikesVisual : MonoBehaviour
    {
        private bool _isMoving;
        [BoxGroup("MovementSettings")] [SerializeField]
        private Ease Easing;

        [BoxGroup("MovementSettings")] [SerializeField]
        private float Speed;

        [BoxGroup("MovementSettings")] [SerializeField]
        public bool WaitsOnStop;

        [BoxGroup("MovementSettings")] [ShowIf("WaitsOnStop")] [SerializeField]
        public float WaitDuration;

        private bool _hasParticles;
        private ParticleSystem _particleSystem;
        public bool HasParticles => _hasParticles;
        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        private void OnEnable()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _hasParticles = _particleSystem != null;
        }

        public void MoveToPoint(Vector3 point)
        {
            if (_isMoving) return;
            _isMoving = true;
            MovementTween = transform.DOMove(point, Speed).SetSpeedBased().SetEase(Easing)
                .OnComplete(() => StartCoroutine(WaitOnPoint()));
        }
        public IEnumerator WaitOnPoint()
        {
            if (!WaitsOnStop)
            {
                _isMoving = false;
                yield break;
            }

            yield return new WaitForSeconds(WaitDuration);
            _isMoving = false;
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> MovementTween;

        public void ClearParticles()
        {
            _particleSystem.Stop();
            _particleSystem.Clear();

        }

        public void PlayParticles()
        {
            _particleSystem.Play();
        }
    }
}