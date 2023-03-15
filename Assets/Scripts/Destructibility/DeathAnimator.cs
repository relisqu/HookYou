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
        [SerializeField] private string HealthTag = "Health";
        [SerializeField] private bool isAnimatedDifferently;


        private bool _animatorExists;

        public void UpdateHealth(int health)
        {
            if (_animatorExists && HealthTag != "")
            {
                Animator.SetInteger(HealthTag, health);
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
            if (_animatorExists) Animator.SetTrigger(HitTag);
        }

        public virtual void PlayDeathAnimation()
        {
            if (_animatorExists)
            {
                Animator.SetTrigger(DeathTag);
            }
            else if (!isAnimatedDifferently)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void PlayRespawnAnimation()
        {
            if (_animatorExists)
            {
                Animator.SetTrigger(RespawnTag);
            }
            else if (!isAnimatedDifferently)
            {
                gameObject.SetActive(true);
            }
        }
    }
}