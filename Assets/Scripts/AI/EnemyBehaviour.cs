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
        [SerializeField]protected float StunDuration;
        
        [BoxGroup("References")] [SerializeField]
        protected EnemyHealth Health;
        [BoxGroup("References")] [SerializeField]
        private PopupVFX StunEffect;
        public void SetStunned()
        {
            StunEffect.InitiateObject();
            Stunned?.Invoke();
        }
    }
}