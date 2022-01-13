using System.Collections;
using UnityEngine;

namespace Player_Scripts
{
    public class SwordHitVFX : MonoBehaviour
    {
        [SerializeField]private ParticleSystem ParticleSystem;
        [SerializeField]private float Cooldown;
        private bool isAbleHit = true;
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
            ParticleSystem.Emit(1);
        }
    }
}