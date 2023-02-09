using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Obstacles
{
    public class MovingObstacle : MonoBehaviour
    {
        [BoxGroup("MovementSettings")] [SerializeField]
        private Ease Easing;

        [BoxGroup("MovementSettings")] [SerializeField]
        private float Speed;

        [BoxGroup("MovementSettings")] [SerializeField]
        private bool WaitsOnStop;

        [BoxGroup("MovementSettings")] [ShowIf("WaitsOnStop")] [SerializeField]
        private float WaitDuration;

        [BoxGroup("Points")] [SerializeField] private List<Vector3> Points;
        [BoxGroup("Points")] [SerializeField] private Transform HelperTransform;


        private bool _hasParticles;
        private ParticleSystem _particleSystem;

        private void OnEnable()
        {
            transform.position = Points[_currentPoint];
            StartCoroutine(StartMovement());
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _hasParticles = _particleSystem != null;
        }

        public void MoveToNextPoint()
        {
            if (_isMoving) return;
            _isMoving = true;
            _currentPoint++;
            _currentPoint %= Points.Count;
            _movementTween = transform.DOMove(Points[_currentPoint], Speed).SetSpeedBased().SetEase(Easing)
                .OnComplete(() => StartCoroutine(WaitOnPoint()));
        }

        public void StopMovement()
        {
            _movementTween?.Kill();
            StopAllCoroutines();
            _canMove = false;
            _isMoving = false;
            if (_hasParticles)
            {
                _particleSystem.Stop();
                _particleSystem.Clear();
                
            }
        }

        public void RestartMovement()
        {
            StopMovement();
            _currentPoint = 0;
            transform.position = Points[_currentPoint];
            _canMove = true;
            StartCoroutine(StartMovement());
            _particleSystem.Play();
        }


        public IEnumerator StartMovement()
        {
            while (true)
            {
                if (!_canMove) yield break;
                if (_isMoving)
                    yield return null;
                MoveToNextPoint();
                yield return null;
            }
        }

        private IEnumerator WaitOnPoint()
        {
            if (!WaitsOnStop)
            {
                _isMoving = false;
                yield break;
            }

            yield return new WaitForSeconds(WaitDuration);
            _isMoving = false;
        }


        [Button]
        [BoxGroup("Points")]
        [PropertyTooltip(
            "If you tired of moving points close to structure - this button moves there close to the current position of object in the distance of 5 metres")]
        public void MovePointsCloseToObstacle()
        {
            var position = transform.position;
            for (var index = 0; index < Points.Count; index++)
            {
                var point = Points[index];
                var direction = (point - position).normalized;
                Points[index] = position + direction * 5f;
            }
        }

        [BoxGroup("Points")]
        [Button]
        public void GeneratePointAtHelperPosition()
        {
            Points.Add(HelperTransform.position);
        }

        private void OnDrawGizmos()
        {
            foreach (var point in Points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(point, 0.3f);
            }

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(HelperTransform.position, 0.3f);
        }

        private int _currentPoint = 0;
        private bool _isMoving;
        private TweenerCore<Vector3, Vector3, VectorOptions> _movementTween;
        private bool _canMove = true;
    }
}