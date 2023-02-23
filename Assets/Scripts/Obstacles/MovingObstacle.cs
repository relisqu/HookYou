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

        [SerializeField] private MovingSpikesVisual MovingVisual;

        private void OnEnable()
        {
            transform.position = Points[_currentPoint];
            StartCoroutine(StartMovement());
        }

        public void MoveToNextPoint()
        {
            _currentPoint++;
            _currentPoint %= Points.Count;
            MovingVisual.MoveToPoint(Points[_currentPoint]);
        }
    

        public void StopMovement()
        {
            MovingVisual.MovementTween?.Kill();
            StopAllCoroutines();
            _canMove = false;
            MovingVisual.IsMoving = false;
            if (MovingVisual.HasParticles)
            {
                MovingVisual.ClearParticles();
            }
        }

        public void RestartMovement()
        {
            StopMovement();
            _currentPoint = 0;
            transform.position = Points[_currentPoint];
            _canMove = true;
            StartCoroutine(StartMovement());
            MovingVisual.PlayParticles();
        }


        public IEnumerator StartMovement()
        {
            while (true)
            {
                if (!_canMove) yield break;
                if (MovingVisual.IsMoving)
                    yield return null;
                MoveToNextPoint();
                yield return null;
            }
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
        private bool _canMove = true;
    }
}