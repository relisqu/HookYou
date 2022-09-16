using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public abstract class PushableBlock : HookBlock
    {
        [SerializeField] protected Rigidbody2D Rigidbody2D;
        [SerializeField] private float PushSpeed;

        protected override void AddActivitiesAfterHook(Hook hook)
        {
            // Rigidbody2D.velocity = Vector2.zero;
            hook.DropHook();
            //Rigidbody2D.AddForce(CalculatePushDirection(hook.GetPlayerTransform().position) * (PushSpeed * Rigidbody2D.mass),ForceMode2D.Impulse);
        }
        public abstract Vector2 CalculatePushDirection(Vector3 playerPosition);

        public void RemovePushForce()
        {
            Rigidbody2D.velocity=Vector2.zero;
        }

        public void Drake(float speed)
        {
            Rigidbody2D.velocity *= speed;
        }
    }
}