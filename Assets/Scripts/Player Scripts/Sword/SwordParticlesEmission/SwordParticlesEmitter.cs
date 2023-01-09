using System;
using Assets.Scripts;
using UnityEngine;

namespace Player_Scripts.Sword.SwordParticlesEmission
{
    public class SwordParticlesEmitter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem.MinMaxGradient ParticlesColorScheme;
        [SerializeField] private float ParticlesMultiplier=1;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword))
            {
                if (!sword.IsAttacking) return;
                var settings = sword.EmitParticles.main;
                ParticlesColorScheme.mode = ParticleSystemGradientMode.RandomColor;
                settings.startColor = ParticlesColorScheme;
                sword.EmitParticles.Emit((int) (10*ParticlesMultiplier));
            }
        }
    }
}