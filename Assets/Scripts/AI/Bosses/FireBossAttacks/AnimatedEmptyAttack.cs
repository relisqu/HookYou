using System.Collections;
using UnityEngine;

namespace AI.Bosses.FireBossAttacks
{
    public class AnimatedEmptyAttack : Attack
    {
        [SerializeField] private float BeforeAttackWaitTime;
        [SerializeField] private float WaitTime;
        public override IEnumerator StartAttack()
        {
            yield return new WaitForSeconds(BeforeAttackWaitTime);
            PlayAttackAnimation();
            yield return new WaitForSeconds(WaitTime);
        }

        public override Attack GetCurrentAttack()
        {
            return this;
        }
    }
}