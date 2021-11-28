using System;
using UnityEngine;

namespace Destructibility
{
    public class RespawnableLevelObject : MonoBehaviour
    {   
        private Vector3 spawnPosition;
        [SerializeField]private Health Health;
        
        private void Awake()
        {
            spawnPosition = transform.position;
        }
        
        public void Spawn()
        {
            gameObject.SetActive(true);
            transform.position = spawnPosition;
            if (!GetHealth().IsAlive) GetHealth().Respawn();
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