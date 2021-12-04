using System.Collections;
using UnityEngine;

namespace Player_Scripts
{
    public class SwordHitVFX : MonoBehaviour
    {
        [SerializeField]private Animator Animator;
        private int hitAmount=0;
        private static readonly int DamageReceived = Animator.StringToHash("damageReceived");
        private static readonly int Swoosh = Animator.StringToHash("swoosh");
        [SerializeField]private float Cooldown;
        private bool isAbleHit = true;
        IEnumerator MakeIFrame()
        {
            isAbleHit = false;
            yield return new WaitForSeconds(Cooldown);
            print("Cd is over");
            isAbleHit = true;

        }

        public void Hit()
        {
            if (!isAbleHit) return;
            StartCoroutine(MakeIFrame());
            hitAmount+=1;
            hitAmount %= 3;
            print(hitAmount);
            Animator.SetInteger(Swoosh,hitAmount);
            Animator.SetTrigger(DamageReceived);
        }
    }
}