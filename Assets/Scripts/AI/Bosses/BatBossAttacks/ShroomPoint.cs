using System;
using Destructibility;
using UnityEngine;

namespace AI.Bosses.BatBossAttacks
{
    public class ShroomPoint : MonoBehaviour
    {
        [SerializeField] private EnemyHealth Mushroom;

        public void SpawnShroom()
        {
            shroom.gameObject.SetActive(true);
        }

        public void RespawnShroom()
        {
            shroom.Respawn();
        }

        private void Start()
        {
            shroom = Instantiate(Mushroom, transform);
            shroom.transform.localPosition=Vector3.zero;
            
            shroom.gameObject.SetActive(false);
        }

        public bool IsMushroomAlive()
        {
            return shroom.CurrentHealth > 0  && shroom.gameObject.activeInHierarchy;
        }

        public bool IsMushroomActive()
        {
            return shroom.gameObject.activeInHierarchy;
        }

        private EnemyHealth shroom;
    }
}