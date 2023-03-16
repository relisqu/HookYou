
using System;
using Destructibility;
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
        private bool _isHooked;
        private static Hook _hook;
        private SwordDestructible _swordHit;
        private bool _needsToRemoveHookOnDamage;
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
        public virtual void AddActivitiesAtHookStart()
        { 
            _isHooked = true;
        }

        public virtual void OnHookBreak()
        {
            _isHooked = false;
        }
        public void TouchTheBlock(Hook hook)
        {
            _isHooked = false;
            HookTouchedBlock?.Invoke();
            AddActivitiesAfterHook( hook);
        }

        private void RemoveHook()
        {
            if (_isHooked)
            {
                _isHooked = false;
                _hook.ClearHook(); 
            }
        }

        private void Start()
        {
            if (_hook == null) _hook = FindObjectOfType<Hook>();
            _swordHit = GetComponent<SwordDestructible>();
            _needsToRemoveHookOnDamage = _swordHit != null;
            if (_needsToRemoveHookOnDamage) _swordHit.TookSwordHit += RemoveHook;
        }

        private void OnDestroy()
        {
            if (_needsToRemoveHookOnDamage) _swordHit.TookSwordHit -= RemoveHook;
        }
        protected abstract void AddActivitiesAfterHook(Hook hook);
    }
}