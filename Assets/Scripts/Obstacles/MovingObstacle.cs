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
        [BoxGroup("Points")] [SerializeField] private List<Vector3> Points;
        [BoxGroup("Points")] [SerializeField] private Transform HelperTransform;


        private void OnEnable()
        {
            transform.position = Points[_currentPoint];
            StartCoroutine(StartMovement());
        }

        public void MoveToNextPoint()
        {
            if (_isMoving) return;
            _isMoving = true;
            _currentPoint++;
            _currentPoint %= Points.Count;
            movementTween = transform.DOMove(Points[_currentPoint], Speed).SetSpeedBased().SetEase(Easing)
                .OnComplete(() => StartCoroutine(WaitOnPoint()));
        }

        [Button]
        public void StopMovement()
        {
            movementTween?.Kill();
            StopAllCoroutines();
            _canMove = false;
            _isMoving = false;
        }

        [Button]
        public void ResetMovement()
        {
            _canMove = true;
            _currentPoint--;
            _currentPoint %= Points.Count;
            StartCoroutine(StartMovement());
        }

        TweenerCore<Vector3, Vector3, VectorOptions> movementTween;
        private bool _canMove = true;

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

        [SerializeField] private Ease Easing;

        [SerializeField] private float Speed;
        [SerializeField] private bool WaitsOnStop;

        [ShowIf("WaitsOnStop")] [SerializeField]
        private float WaitDuration;

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
    }
}