using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace AI
{
    public class BatMovementAnimator : MonoBehaviour
    {
        private Vector3 previousPosition;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private static readonly int Dash1 = Animator.StringToHash("dash");
        private static readonly int ToDash = Animator.StringToHash("prepareToDash");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private Shader shaderGUItext;
        private Shader shaderSpritesDefault;
        private static readonly int Fly = Animator.StringToHash("fly");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            shaderGUItext = Shader.Find("GUI/Text Shader");
            var material = spriteRenderer.material;
            shaderSpritesDefault = material.shader;
        }

        private void OnEnable()
        {
            previousPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var currentPosition = transform.position;
            spriteRenderer.flipX = previousPosition.x < currentPosition.x;
            previousPosition = currentPosition;
        }

        public void PrepareToDash()
        {
            animator.SetTrigger(ToDash);
        }

        public void FlyAgain()
        {
            animator.SetTrigger(Fly);
        }

        public IEnumerator GetDamage()
        {
            SetWhiteSprite();
            yield return new WaitForSeconds(0.3f);
            SetNormalSprite();
        }

        void SetWhiteSprite()
        {
            spriteRenderer.material.shader = shaderGUItext;
        }

        public void SetNormalSprite()
        {
            spriteRenderer.material.shader = shaderSpritesDefault;
            
        }

        public void Dash()
        {
            animator.SetTrigger(Dash1);
        }

      

        public void SetDashing(bool value)
        {
            animator.SetBool(IsAttacking, value);
        }
    }
}