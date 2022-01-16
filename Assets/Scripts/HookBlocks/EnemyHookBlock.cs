using AI;
using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class EnemyHookBlock : StickyBlock
    {
        [SerializeField] private EnemyBehaviour EnemyBehaviour;
        protected override void AddActivitiesAfterHook(Hook hook)
        {
            EnemyBehaviour.SetStunned();
        }
    }
}