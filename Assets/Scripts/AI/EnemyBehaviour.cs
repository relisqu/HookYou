using System;
using UnityEngine;

namespace AI
{
    public class EnemyBehaviour : MonoBehaviour
    {
        protected bool isStunned;
        protected Action Stunned;
        [SerializeField]protected float StunDuration;
        
        public void SetStunned()
        {
            Stunned?.Invoke();
        }
    }
}