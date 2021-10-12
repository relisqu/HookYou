using UnityEngine;

namespace HookBlocks
{
    public class LoosePushableBlock : PushableBlock
    {
        public override Vector2 CalculatePushDirection(Vector3 playerPosition)
        {
            
            return transform.position - playerPosition;
        
        }
    }
}