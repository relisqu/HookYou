using System;
using AI;
using Destructibility;
using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class EnemyHookBlock : StickyBlock
    {
        [SerializeField] private EnemyBehaviour EnemyBehaviour;

        protected override void AddActivitiesAfterHook(Hook hook)
        {
                if(EnemyBehaviour!= null) EnemyBehaviour.SetStunned();
        }

    }
}