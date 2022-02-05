using System.Collections;
using UnityEngine;

namespace AI
{
    public class StunAttack : Attack
    {
        [SerializeField] private EnemyBehaviour Boss;
        [SerializeField] private float StunDuration;

        public override IEnumerator StartAttack()
        {
            Boss.SetStunned();
            yield return new WaitForSeconds(StunDuration);
        }
    }
}