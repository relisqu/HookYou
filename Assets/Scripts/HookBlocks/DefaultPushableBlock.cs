using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class DefaultPushableBlock : PushableBlock
    {
        public override Vector2 CalculatePushDirection(Vector3 playerPosition)
        {
            return Vector2.zero;
        }

        protected override void AddActivitiesAfterHook(Hook hook)
        {
            hook.ClearHook();
        }
    }
}