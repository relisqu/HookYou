using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace AI.Bosses.Attacks
{
    public class GroundAttack : Attack
    {
        [SerializeField] private float AnimationWaitTime;
        public override IEnumerator StartAttack()
        {
            yield return new WaitForSeconds(0.2f);
            print("Started ground attack");
            PlayAttackAnimation();
            yield return new WaitForSeconds(AnimationWaitTime);
        }

    }
}