
using System;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace HookBlocks
{
    public abstract class HookBlock : MonoBehaviour
    {
        [SerializeField] private float SwingSpeed;
        [FormerlySerializedAs("UsingSpecificHookSpeed")] [SerializeField] private bool RequiresUniqueHookSpeed;
        [SerializeField] private float HookShotSpeed;
        public Action HookTouchedBlock { get; set; }
        

        public float GetSwingSpeed()
        {
            return SwingSpeed;
        }

        public bool RequiresSpecificHookSpeed()
        {
            return RequiresUniqueHookSpeed;
        }

        public float GetHookShotSpeed()
        {
            return HookShotSpeed;
        }

        public void TouchTheBlock(Hook hook)
        {
            HookTouchedBlock?.Invoke();
            AddActivitiesAfterHook( hook);
        }

        protected abstract void AddActivitiesAfterHook(Hook hook);
    }
}