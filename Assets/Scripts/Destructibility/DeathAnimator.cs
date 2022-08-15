using System;
using UnityEngine;

namespace Destructibility
{
    public class DeathAnimator : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private string HitTag;
        [SerializeField] private string DeathTag;
        [SerializeField] private string RespawnTag;
        [SerializeField] private bool isAnimatedDifferently;


        private bool _animatorExists;
        private static readonly int Health = Animator.StringToHash("Health");

        public void UpdateHealth(int health)
        {
            if (_animatorExists)
            {
                Animator.SetInteger(Health,health);
            }
        }

        private void Start()
        {
            if (Animator != null)
            {
                _animatorExists = true;
                Animator.gameObject.SetActive(false);
                Animator.gameObject.SetActive(true);
            }
        }

        public virtual void PlayHitAnimation()
        {
            if (Animator != null) Animator.SetTrigger(HitTag);
        }

        public virtual void PlayDeathAnimation()
        {
            if (Animator != null)
            {
                Animator.SetTrigger(DeathTag);
            }
            else if(!isAnimatedDifferently)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void PlayRespawnAnimation()
        {
            if (Animator != null)
            {
                Animator.SetTrigger(RespawnTag);
            }
            else if(!isAnimatedDifferently)
            {
                gameObject.SetActive(false);
            }
        }
    }
}