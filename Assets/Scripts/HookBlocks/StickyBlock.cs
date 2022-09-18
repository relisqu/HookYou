using Player_Scripts;

namespace HookBlocks
{
    public class StickyBlock : DefaultPushableBlock
    {
        protected override void AddActivitiesAfterHook(Hook hook)
        {
           // hook.HangOnWall();
           hook.ClearHook();
        }
        
    }
}