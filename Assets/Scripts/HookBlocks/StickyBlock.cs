using Player_Scripts;

namespace HookBlocks
{
    public class StickyBlock : HookBlock
    {
        protected override void AddActivitiesAfterHook(Hook hook)
        {
            hook.HangOnWall();
        }
        
    }
}