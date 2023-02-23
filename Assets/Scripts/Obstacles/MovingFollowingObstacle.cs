using System;
using System.Collections;
using System.Collections.Generic;
using Destructibility;
using Pathfinding;
using UnityEngine;

namespace Obstacles
{
    public class MovingFollowingObstacle : MonoBehaviour
    {
        public Transform targetPosition;

        private List<Vector3> _path;

        [SerializeField] private Health Health;
        [SerializeField] private MovingSpikesVisual MovingVisual;
        private bool _hasPath;
        private bool _searchesPath;

        public void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p.vectorPath;
                _hasPath = true;
                _searchesPath = false;
            }
        }

        private Seeker _seeker;
        private void OnEnable()
        {
            _seeker = GetComponent<Seeker>();
            StartMovementActions();
            StartCoroutine(StartMovementPathfinding());
        }

        public void StartMovementActions()
        {
            _seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            _searchesPath = true;
        }

        private IEnumerator StartMovementPathfinding()
        {
            while (Health.IsAlive)
            {
                if (_searchesPath) yield return null;
                if (_hasPath)
                {
                    if (!MovingVisual.IsMoving)
                    {
                        MovingVisual.IsMoving = true;
                        _hasPath = false;
                        _searchesPath = false;
                        MovingVisual.MoveToPoint(_path[0]);
                    }
                }
                else
                {
                    StartMovementActions();
                }

                yield return null;
            }
        }
    }
    
}