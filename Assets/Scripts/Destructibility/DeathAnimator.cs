using UnityEngine;

namespace Destructibility
{
    public class DeathAnimator : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private string HitTag;
        [SerializeField] private string DeathTag;
        [SerializeField] private string RespawnTag;

        public virtual void PlayHitAnimation()
        {
           if(Animator!=null) Animator.SetTrigger(HitTag);
        }

        public virtual void PlayDeathAnimation()
        {
            if(Animator!=null)  Animator.SetTrigger(DeathTag);
        }

        public virtual void PlayRespawnAnimation()
        {
            if(Animator!=null)  Animator.SetTrigger(RespawnTag);
        }
    }
}