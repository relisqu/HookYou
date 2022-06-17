using System.Collections;
using UnityEngine;

namespace AI
{
    public class StunAttack : Attack
    {
        [SerializeField] private float StunDuration;
        [SerializeField] private GrappleZone GrappleZone;
        [SerializeField] private PopupVFX StunEffect;
        public override IEnumerator StartAttack()
        {
            StunEffect.InitiateObject();
            PlayAttackAnimation();
            Boss.SetStunned();
            GrappleZone.EnableCollider();
            yield return new WaitForSeconds(StunDuration);
            GrappleZone.DisableCollider();
            StunEffect.DestroyObject();
        }
        

        public override Attack GetCurrentAttack()
        {
            return this;
        }
    }
}