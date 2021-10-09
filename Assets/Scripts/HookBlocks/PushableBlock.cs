using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public abstract class PushableBlock : HookBlock
    {
        [SerializeField] protected Rigidbody2D Rigidbody2D;
        [SerializeField] private float PushSpeed;

        public override void AddActivitiesAfterHook(Hook hook)
        {
            if (Rigidbody2D.velocity.sqrMagnitude>=0.1f) Rigidbody2D.velocity = Vector2.zero;
            hook.DropHook();
            Rigidbody2D.AddForce(CalculatePushDirection(hook.GetPlayerTransform().position) * PushSpeed,ForceMode2D.Impulse);
        }
        public abstract Vector2 CalculatePushDirection(Vector3 playerPosition);
        
    }
}