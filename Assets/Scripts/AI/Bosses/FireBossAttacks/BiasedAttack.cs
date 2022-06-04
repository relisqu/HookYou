using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Subtegral.WeightedRandom;
using UnityEngine;

namespace AI.Bosses.FireBossAttacks
{
    public class BiasedAttack : Attack
    {
        [Tooltip("The minimum distance between player and object that considers as far")] [SerializeField]
        private float RequiredDistanceBetweenObjects;

        [SerializeField] private Transform PlayerTransform;
        [SerializeField] private Transform BossTransform;
        [OdinSerialize] public List<LocationBiasAttack> AttackIngredients;
        WeightedRandom<Attack> _random = new WeightedRandom<Attack>();

        public override IEnumerator StartAttack()
        {
            yield return GetRandomBiasedAttack();
        }

        public float GetAttackBias(LocationBiasAttack attack)
        {
            var distance = Vector3.Distance(PlayerTransform.position, BossTransform.position);
            DistanceBias distanceBias = DistanceBias.ObjectClose;
            if (distance > RequiredDistanceBetweenObjects)
            {
                distanceBias = DistanceBias.ObjectFar;
            }

            return distanceBias == attack.BiasType ? attack.BiasBonus : 0;
        }

        public Attack GetRandomBiasedAttack()
        {
            _random.Clear();

            foreach (var locationAttack in AttackIngredients)
            {
                _random.Add(locationAttack.Attack, GetAttackBias(locationAttack));
            }

            return _random.Next();
        }

        [Button]
        public void DrawCloseDistance()
        {
            _askedToDraw = !_askedToDraw;
        }

        private void OnDrawGizmos()
        {
            if (!_askedToDraw) return;
            Gizmos.color = Color.red;
            var position = transform.position;
            var direction = (PlayerTransform.position - position).normalized;
            Gizmos.DrawLine(position,position+direction*RequiredDistanceBetweenObjects);
        }

        private bool _askedToDraw;
    }

    [Serializable]
    public class LocationBiasAttack
    {
        [SerializeField] public Attack Attack;
        [SerializeField] public DistanceBias BiasType;
        [SerializeField] public float BiasBonus = 0;
    }

    public enum DistanceBias
    {
        ObjectFar,
        ObjectClose
    }
}