using AI;
using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class EnemyHookableBlock : NonStickyBlock
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private EnemyBehaviour EnemyBehaviour;
        private static readonly int Stunned = Animator.StringToHash("Stunned");
        
        protected override void AddActivitiesAfterHook(Hook hook)
        {
            Animator.SetBool(Stunned, false);
            EnemyBehaviour.SetStunned();
            hook.ClearHook();
            
        }
        public override void AddActivitiesAtHookStart()
        { 
            Animator.SetBool(Stunned,true);
            EnemyBehaviour.InvokeEvent();
        }

        public override void OnHookBreak()
        {
            Animator.SetBool(Stunned, false);
        }
    }
}