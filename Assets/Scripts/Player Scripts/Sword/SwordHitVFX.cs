using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_Scripts
{
    public class SwordHitVFX : MonoBehaviour
    {
        [SerializeField]private List<ParticleSystem> ParticleSystem;
        [SerializeField]private float Cooldown;
        private bool isAbleHit = true;
        private int hitAmount=0;
        IEnumerator MakeIFrame()
        {
            isAbleHit = false;
            yield return new WaitForSeconds(Cooldown);
            isAbleHit = true;

        }

        
        public void Hit()
        {
            if (!isAbleHit) return;
            StartCoroutine(MakeIFrame());
            hitAmount+=1;
            hitAmount %= ParticleSystem.Count;
            ParticleSystem[hitAmount].Emit(1);
        }
    }
}