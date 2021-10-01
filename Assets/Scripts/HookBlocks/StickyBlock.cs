using Player_Scripts;

namespace HookBlocks
{
    public class StickyBlock : HookBlock
    {
        public override void AddActivitiesAfterHook(Hook hook)
        {
            hook.HangOnWall();
        }
        
    }
}