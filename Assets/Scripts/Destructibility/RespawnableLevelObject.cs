using System;
using UnityEngine;

namespace Destructibility
{
    public class RespawnableLevelObject : MonoBehaviour
    {
        protected Vector3 spawnPosition;
        [SerializeField] protected Health Health;

        private void Awake()
        {
            spawnPosition = transform.position;
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            transform.position = spawnPosition;
            GetHealth().Respawn();
        }

        public void Despawn()
        {
            gameObject.SetActive(false);
        }

        public Health GetHealth()
        {
            return Health;
        }
    }
}