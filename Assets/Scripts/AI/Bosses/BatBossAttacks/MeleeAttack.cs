using System.Collections;
using Player_Scripts;
using UnityEngine;

namespace AI.Bosses.BatBossAttacks
{
    public class MeleeAttack : Attack
    {
        [SerializeField] private Transform HandsModule;
        [SerializeField] private Animator HandsModuleAnimator;
        [SerializeField] private PlayerMovement Player;

        public override IEnumerator StartAttack()
        {
            print("AAAAAAA");
            animationStopped = true;
            var lookRotation = (Player.transform.position - HandsModule.position).normalized;
            HandsModule.rotation =
                Quaternion.Euler(new Vector3(0f, 0f, -Vector2.SignedAngle(lookRotation, Vector3.right)));
            HandsModuleAnimator.SetTrigger(Attack);
            print("Triggered");
            yield return WaitForAnimation();
        }

        public void StopAnimation()
        {
            animationStopped = true;
        }

        private IEnumerator WaitForAnimation()
        {
            do
            {
                yield return null;
            } while (!animationStopped);
        }

        private bool animationStopped;
        private static readonly int Attack = Animator.StringToHash("Attack");
    }
}