using System.Collections;
using System.Collections.Generic;
using Destructibility;
using Pathfinding;
using UnityEngine;

namespace Obstacles
{
    public class MovingFollowingObstacle : MovingObstacle
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
/*
        public override void StartMovementActions()
        {
            Seeker seeker = GetComponent<Seeker>();
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            _searchesPath = true;
        }

        private IEnumerator StartMovementPathfinding()
        {
            while (Health.IsAlive)
            {
                if (_searchesPath) yield return null;
                if (_hasPath)
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _hasPath = false;
                        _searchesPath = false;
                        MoveToNextPoint(_path[0]);
                    }
                }
                else
                {
                    StartMovementActions();
                }
            }
        }
    }*/
    }
}