using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public class PushableBlock : HookBlock
    {
        [SerializeField] private Rigidbody2D Rigidbody2D;
        [SerializeField] private float PushSpeed;

        public override void AddActivitiesAfterHook(Hook hook)
        {
            hook.DropHook();
            if (Rigidbody2D.velocity != Vector2.zero) Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.AddForce(CalculatePushDirection(hook.GetPlayerTransform().position) * PushSpeed);
        }

        public Vector2 CalculatePushDirection(Vector3 playerPosition)
        {
            return transform.position - playerPosition;
        }
    }
}