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
        private Color colorDefault;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            shaderGUItext = Shader.Find("GUI/Text Shader");
            var material = spriteRenderer.material;
            shaderSpritesDefault = material.shader;
            colorDefault = spriteRenderer.color; // or whatever sprite shader is being used
        }

        private void OnEnable()
        {
            previousPosition = transform.position;
            spriteRenderer.color =colorDefault;
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

        public IEnumerator GetDamage()
        {
            SetWhiteSprite();
            yield return new WaitForSeconds(0.2f);
            SetNormalSprite();
        }

        void SetWhiteSprite()
        {
            spriteRenderer.material.shader = shaderGUItext;
            spriteRenderer.color = Color.white;
        }

        public void SetNormalSprite()
        {
            spriteRenderer.material.shader = shaderSpritesDefault;
            spriteRenderer.color = colorDefault;
            
        }

        public void Dash()
        {
            animator.SetTrigger(Dash1);
        }

        public void StopAttack()
        {
            animator.SetBool(IsAttacking, false);
        }

        public void StartAttack()
        {
            animator.SetBool(IsAttacking, true);
        }
    }
}