using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class UpgradedDashingAI : DashingAI
    {
        [BoxGroup("After Damage")] [SerializeField]
        private float DamagedSpeed;

        [BoxGroup("After Damage")] [SerializeField]
        private float DamagedAttackRadius;

        [BoxGroup("After Damage")] [Tooltip("How far the AI will fly while dashing")] [SerializeField]
        private float DamagedDashRange;

        [BoxGroup("After Damage")] [Tooltip("How fast the AI will fly while dashing")] [SerializeField]
        private float DamagedDashSpeed;

        [BoxGroup("After Damage")] [SerializeField]
        private float DamagedPauseDuration;

        void ChangeStats()
        {
            Speed = DamagedSpeed;
            AttackRadius = DamagedAttackRadius;
            DashRange = DamagedDashRange;
            DashSpeed = DamagedDashSpeed;
            PauseDuration = DamagedPauseDuration;
        }

        void ResetStats()
        {
            Speed = _defaultSpeed;
            AttackRadius = _defaultAttackRadius;
            DashRange = _defaultDashRange;
            DashSpeed = _defaultDashSpeed;
            PauseDuration = _defaultDuration;
        }

        private void Start()
        {
            _defaultSpeed = Speed;
            _defaultAttackRadius = AttackRadius;
            _defaultDashRange = DashRange;
            _defaultDashSpeed = DashSpeed;
            _defaultDuration = PauseDuration;
            Health.TookDamage+=ChangeStats;
            Health.Respawned += ResetStats;
        }

        private void OnDestroy()
        {
            Health.TookDamage-=ChangeStats;
            Health.Respawned -= ResetStats;
        }

        private float _defaultSpeed;
        private float _defaultAttackRadius;
        private float _defaultDashRange;
        private float _defaultDashSpeed;
        private float _defaultDuration;
    }
}