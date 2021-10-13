using UnityEngine;

namespace Destructibility
{
    public class DeathAnimator : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private string HitTag;
        [SerializeField] private string DeathTag;

        public void PlayHitAnimation()
        {
            Animator.SetTrigger(HitTag);
        }

        public void PlayDeathAnimation()
        {
            Animator.SetTrigger(DeathTag);
        }
        
    }
}