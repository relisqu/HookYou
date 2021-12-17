using System;
using UnityEngine;

namespace Destructibility
{
    public class RespawnableLevelObject : MonoBehaviour
    {
        private Vector3 spawnPosition;
        [SerializeField] private Health Health;

        private void Awake()
        {
            spawnPosition = transform.position;
            print("setting save position of "+name+":"+spawnPosition);
        }

        public void Spawn()
        {
            print("Spawn");
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