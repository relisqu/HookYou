using System;
using System.Collections;
using System.Collections.Generic;
using Destructibility;
using DG.Tweening;
using HookBlocks;
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
        [SerializeField] private float DashCoefficient;
        [SerializeField] private LayerMask CollisionMask;


        public void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                if (!Health.IsAlive) return;
                _path = p.vectorPath;
                _hasPath = true;
                _searchesPath = false;
                var point = CalculatePoint(_path[2], true);
                var overlap = Physics2D.OverlapCircle(point, 2f);
                var position = transform.position;
                var direction = point - (Vector2)position;
                RaycastHit2D hit = Physics2D.Raycast(position, direction, direction.magnitude, CollisionMask.value);
                if (hit.collider != null)
                {
                    print(hit.collider.name);
                    point = hit.collider.ClosestPoint(point);
                    point = CalculatePoint(point, false);
                    MovingVisual.MoveToPoint(point, false);
                    return;
                }

                MovingVisual.MoveToPoint(point, true);
            }
        }

        private Seeker _seeker;

        private void OnEnable()
        {
            _seeker = GetComponent<Seeker>();
            StartMovementActions();
            MovingVisual.StoppedPause += StartMovementActions;
            _hasPath = false;
            _searchesPath = false;
            MovingVisual.IsMoving = false;
        }

        private void OnDisable()
        {
            MovingVisual.StoppedPause -= StartMovementActions;
        }

        public void StartMovementActions()
        {
            if (!Health.IsAlive) return;
            _seeker.StartPath(transform.position, TargetPosition.position, OnPathComplete);
            _searchesPath = true;
        }


        private Vector2 CalculatePoint(Vector2 path, bool _needDashForce)
        {
            var targetPosition = (Vector2)TargetPosition.position;
            var position = (Vector2)transform.position;
            var directionToTarget = (targetPosition - position);
            var directionToPath = (path - position).normalized;

            float maxDistance;
            if (_needDashForce)
            {
                maxDistance = directionToTarget.magnitude + DashCoefficient < DashDistance
                    ? directionToTarget.magnitude
                    : DashDistance;
            }
            else
            {
                maxDistance = 1;
            }

            return position + maxDistance * directionToPath;
        }

        private void OnDrawGizmos()
        {
            if (_path is { Count: > 1 })
            {
                Gizmos.DrawSphere(CalculatePoint(_path[1], true), 1f);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(CalculatePoint(_path[1], false), 1f);
            }
        }


        private bool _hasPath;
        private bool _searchesPath;
        private List<Vector3> _path;
    }
}