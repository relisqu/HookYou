using System;
using Destructibility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class EnemyBehaviour : MonoBehaviour
    {
        protected bool isStunned;
        protected Action Stunned;
        [SerializeField] protected float StunDuration;

        [BoxGroup("References")] [SerializeField]
        protected EnemyHealth Health;

        [BoxGroup("References")] [SerializeField]
        private PopupVFX StunEffect;

        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public Rigidbody2D GetRigidbody2D()
        {
            return _rigidbody2D;
        }

        public void SetStunned()
        {
            if (!Health.IsAlive) return;
            ShowStunEffect();
            Stunned?.Invoke();
        }

        public void InvokeEvent()
        {
            Stunned?.Invoke();
        }

        public virtual void ShowStunEffect()
        {
            if (StunEffect != null) StunEffect.InitiateObject();
        }

        public void StopStunEffect()
        {
            if (StunEffect != null) StunEffect.HideObject();
        }

        public Health GetHealth()
        {
            return Health;
        }
    }
}