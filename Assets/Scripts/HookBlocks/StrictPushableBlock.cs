using System;
using UnityEngine;

namespace HookBlocks
{
    public class StrictPushableBlock : PushableBlock
    {
        public override Vector2 CalculatePushDirection(Vector3 playerPosition)
        {
            var playerPushDirection = (transform.position - playerPosition).normalized;
            return GetRoundedDirection(playerPushDirection);
        }

        private void FixedUpdate()
        {
            if (Rigidbody2D.velocity.sqrMagnitude < 0.01f) return;
            var currentDirection = Rigidbody2D.velocity;
            var newVelocity = GetRoundedDirection(currentDirection);
            Rigidbody2D.velocity = new Vector2(newVelocity.x*Math.Abs(currentDirection.x),newVelocity.y*Math.Abs(currentDirection.y));
        }

        Vector2 GetRoundedDirection( Vector2 direction)
        {
            Vector2 roundDirection;
            if (Math.Abs(direction.x) > Math.Abs(direction.y))
            {
                roundDirection = Vector2.right;
                roundDirection *= Math.Sign(direction.x);
            }
            else
            {
                roundDirection = Vector2.up;
                roundDirection *= Math.Sign(direction.y);
            }
            
            return roundDirection;  
        }
    }
}