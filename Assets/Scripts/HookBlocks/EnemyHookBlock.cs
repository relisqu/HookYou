using System;
using AI;
using Destructibility;
using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class EnemyHookBlock : DefaultPushableBlock
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private EnemyBehaviour EnemyBehaviour;

        public override void AddActivitiesAtHookStart()
        {
            Animator.SetBool(Stunned, true);
            EnemyBehaviour.InvokeEvent();
            _isHooked = true;
        }

        public override void OnHookBreak()
        {
            Animator.SetBool(Stunned, false);
            _isHooked = false;
        }

        protected override void AddActivitiesAfterHook(Hook hook)
        {
            _isHooked = false;
            Animator.SetBool(Stunned, false);
            EnemyBehaviour.SetStunned();
            hook.ClearHook();
        }


        private static Hook _hook;
        private SwordDestructible _enemySwordHit;
        private bool _needsToRemoveHookOnDamage;
        private static readonly int Stunned = Animator.StringToHash("Stunned");
        private bool _isHooked;

        private void RemoveHook()
        {
            if (_isHooked)
            {
                _isHooked = false;
                _hook.ClearHook();

                EnemyBehaviour.GetRigidbody2D().velocity /= -7f; //To save momentum from sword hit, we give a bit velocity in opposite direction to the hook. Just some magic number, sorry
            }
        }

        private void Start()
        {
            if (_hook == null) _hook = FindObjectOfType<Hook>();
            _enemySwordHit = EnemyBehaviour.GetComponent<SwordDestructible>();
            _needsToRemoveHookOnDamage = _enemySwordHit != null;
            if (_needsToRemoveHookOnDamage) _enemySwordHit.TookSwordHit += RemoveHook;
        }

        private void OnDestroy()
        {
            if (_needsToRemoveHookOnDamage) _enemySwordHit.TookSwordHit -= RemoveHook;
        }
    }
}