using System;
using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts
{
    public class AbyssColliderChanger : MonoBehaviour
    {
        [SerializeField] private List<CompositeCollider2D> AbyssLayerCollider;
        [SerializeField] private float ChangeReactTime;
        [SerializeField] private Player Player;
        [SerializeField] private LayerMask ObstaclesMask;
        [SerializeField] private LayerMask AbyssMask;

        [Button]
        public void UpdateCollidersList()
        {
            AbyssLayerCollider.Clear();
            foreach (var abyss in GameObject.FindGameObjectsWithTag("Abyss"))
            {
                AbyssLayerCollider.Add(abyss.GetComponent<CompositeCollider2D>());
            }
        }

        public void SetAbyssTrigger(bool isTrigger)
        {
            foreach (var collider in AbyssLayerCollider)
            {
                collider.isTrigger = isTrigger;
            }
        }

        private IEnumerator ChangeCondition(bool value)
        {
            yield return new WaitForSeconds(ChangeReactTime);
            foreach (var collider in AbyssLayerCollider)
            {
                collider.isTrigger = value;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (ObstaclesMask == (ObstaclesMask | (1 << other.gameObject.layer))) Player.Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ObstaclesMask == (ObstaclesMask | (1 << other.gameObject.layer))) Player.Die();
            if (AbyssMask == (AbyssMask | (1 << other.gameObject.layer)))
            {
                if (Player.IsInAir) return;
                Player.Die();
            }
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (AbyssMask == (AbyssMask | (1 << other.gameObject.layer)))
            {
                if (Player.IsInAir) return;
                Player.Die();
            }
        }
    }
}