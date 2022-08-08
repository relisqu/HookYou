using System;
using System.Collections;
using UnityEngine;

namespace Destructibility
{
    public class TimelyRespawnableLevelObject : RespawnableLevelObject
    {
        [SerializeField] private float ResurrectionTimer;

        private void Start()
        {
            Health.Died += SetRespawnTimer;
        }

        void SetRespawnTimer()
        {
            StopAllCoroutines();
            _canRespawn = false;
            StartCoroutine(RespawnTimer());
        }

        private void FixedUpdate()
        {
            if (_canRespawn && !Health.IsAlive)
            {
                if (Physics2D.OverlapCircle(spawnPosition, 0.2f)==null)
                {
                    _canRespawn = false;
                    Spawn();
                }
            }
        }

        IEnumerator RespawnTimer()
        {
            _canRespawn = false;
            yield return new WaitForSeconds(ResurrectionTimer);
            _canRespawn = true;
        }

        private bool _canRespawn;
    }
}