using System;
using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class ReplaceSprite : MonoBehaviour
    {
        private SpriteRenderer Renderer;
        [SerializeField]private Sprite NewSprite;
        [SerializeField]private Material NewMaterial;


        private void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeSprite()
        {
            Renderer.sprite=NewSprite;
            Renderer.material = NewMaterial;
        }
    }
}