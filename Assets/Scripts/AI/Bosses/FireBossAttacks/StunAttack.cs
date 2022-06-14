using System.Collections;
using UnityEngine;

namespace AI
{
    public class StunAttack : Attack
    {
        [SerializeField] private EnemyBehaviour Boss;
        [SerializeField] private float StunDuration;
        [SerializeField] private GrappleZone GrappleZone;
        public override IEnumerator StartAttack()
        {
            Boss.SetStunned();
            GrappleZone.EnableCollider();
            yield return new WaitForSeconds(StunDuration);
            GrappleZone.DisableCollider();
        }
    }
}