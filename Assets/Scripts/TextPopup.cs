using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class TextPopup : MonoBehaviour
    {
        [BoxGroup("References")][SerializeField] private Transform TextTransform;
        [BoxGroup("Show text options")][SerializeField] private float TextShowSpeed;
        [BoxGroup("Hide text options")][SerializeField] private float TextHideSpeed;
        [BoxGroup("Show text options")][SerializeField] private Ease ShowEasing;
        [BoxGroup("Hide text options")][SerializeField] private Ease HideEasing;

        public void ShowText()
        {
            TextTransform.DOScaleY(1f, TextShowSpeed).SetSpeedBased().SetEase(ShowEasing);
        }

        public void HideText()
        {
            TextTransform.DOScaleY(0f, TextHideSpeed).SetSpeedBased().SetEase(HideEasing);
        }

        public float GetTextSize()
        {
            return TextTransform.transform.localScale.y;
        }

    }

  
}