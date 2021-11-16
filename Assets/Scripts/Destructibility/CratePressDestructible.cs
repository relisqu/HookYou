using System;
using HookBlocks;
using UnityEngine;

namespace Destructibility
{
    public class CratePressDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;


      

        private void OnTriggerEnter2D(Collider2D other)
        { 
            if (other.gameObject.TryGetComponent(out PushableBlock block))
            {
                Health.TakeDamage(Int32.MaxValue);
                block.Drake(0.1f);
            }
        }
    }
}