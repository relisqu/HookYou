using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class NonStickyBlock : HookBlock
    {
        protected override void AddActivitiesAfterHook(Hook hook)
        {
            hook.ClearHook();
        }
    }
}