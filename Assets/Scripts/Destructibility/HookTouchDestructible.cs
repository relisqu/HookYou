using System;
using HookBlocks;
using UnityEngine;

namespace Destructibility
{
    public class HookTouchDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;
        [SerializeField] private HookBlock HookBlock;

        private void OnEnable()
        {
            HookBlock.HookTouchedBlock += TakeHookDamage;
        }

        private void OnDisable()
        {
            HookBlock.HookTouchedBlock -= TakeHookDamage;
        }

        void TakeHookDamage()
        {
            Health.TakeDamage(1);
            if (TryGetComponent(out PushableBlock block))
            {
                block.RemovePushForce();
            }
            
        }
    }
}