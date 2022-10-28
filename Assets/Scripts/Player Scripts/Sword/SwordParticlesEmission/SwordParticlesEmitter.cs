using System;
using Assets.Scripts;
using UnityEngine;

namespace Player_Scripts.Sword.SwordParticlesEmission
{
    public class SwordParticlesEmitter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem.MinMaxGradient ParticlesColorScheme;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword))
            {
                if (!sword.IsAttacking) return;
                ParticleSystem.MainModule settings = sword.EmitParticles.main;
                ParticlesColorScheme.mode = ParticleSystemGradientMode.RandomColor;
                settings.startColor = ParticlesColorScheme;
                sword.EmitParticles.Emit(10);
            }
        }
    }
}