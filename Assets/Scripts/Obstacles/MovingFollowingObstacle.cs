using System;
using System.Collections;
using System.Collections.Generic;
using Destructibility;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Obstacles
{
    public class MovingFollowingObstacle : MonoBehaviour
    {
        [FormerlySerializedAs("targetPosition")] [SerializeField]
        private Transform TargetPosition;

        [SerializeField] private float DashDistance;
        [SerializeField] private Health Health;
        [SerializeField] private MovingSpikesVisual MovingVisual;
        private bool _hasPath;
        private bool _searchesPath;
        private List<Vector3> _path;

        public void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p.vectorPath;
                _hasPath = true;
                _searchesPath = false;
                MovingVisual.MoveToPoint(CalculatePoint(_path[1]));
            }
        }

        private Seeker _seeker;

        private void OnEnable()
        {
            _seeker = GetComponent<Seeker>();
            StartMovementActions();
            StartCoroutine(StartMovementPathfinding());
            _hasPath = false;
            _searchesPath = false;
            MovingVisual.IsMoving = false;
        }
        
        
        public void StartMovementActions()
        {
            _seeker.StartPath(transform.position, TargetPosition.position, OnPathComplete);
            _searchesPath = true;
        }

        private IEnumerator StartMovementPathfinding()
        {
            while (Health.IsAlive)
            {
                if (!_searchesPath)
                {
                    StartMovementActions();
                    
                }


                yield return null;
            }
        }

        private Vector3 CalculatePoint(Vector3 target)
        {
            return target;
            var position = transform.position;
            var direction = (target - position).normalized;
            return position + direction * DashDistance;
        }

        private void OnDrawGizmos()
        {
            if (_path is { Count: > 1 })
            {
                Gizmos.DrawSphere(CalculatePoint(_path[1]), 1f);
            }
        }
    }
}