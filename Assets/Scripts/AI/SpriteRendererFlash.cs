using System;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class SpriteRendererFlash : MonoBehaviour
    {
        [SerializeField] private Color FlashColor;
        [SerializeField] private float FlashDuration;
        [SerializeField] private SpriteRenderer SpriteRenderer;

        private void OnEnable()
        {
            SpriteRenderer.material.color = _defaultColor;
        }

        public void Flash()
        {
            StopAllCoroutines();
            StartCoroutine(GenerateFlash());
        }

        IEnumerator GenerateFlash()
        {
            SpriteRenderer.material.SetColor("FlashColor",FlashColor); 
            SpriteRenderer.material.SetInt("IsEnabled", 1);
            yield return new WaitForSeconds(FlashDuration);
            SpriteRenderer.material.SetInt("IsEnabled", 0);
        }

        private void Start()
        {
            _defaultColor = Color.white;
        }

        private Color _defaultColor;
    }
}