using System;
using DG.Tweening;
using UnityEngine;

namespace Player_Scripts
{
    public class StatsText : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody;
        [SerializeField] private Transform TextTransform;
        [SerializeField] private float TextSpeed;
        [SerializeField] private float WaitDuration;
        [SerializeField] private Ease Easing;
        [Range(0,2f)][SerializeField] private float Sensitivity;
        private bool _isHidden;

        private void Start()
        {
            HideText();
        }

        private float _waitStartTime;
        private void FixedUpdate()
        {
            if (Rigidbody.velocity.magnitude < Sensitivity)
            {
                _waitStartTime += Time.fixedDeltaTime;
            }
            else
            {
                _waitStartTime = 0;
                HideText();
            }

            if (_waitStartTime >= WaitDuration)
            {
                ShowText();
            }
        }

        private void ShowText()
        {
            if (!_isHidden) return;
            TextTransform.DOScaleY(1f, TextSpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _isHidden = false;
            });
        }

        private void HideText()
        {
            if (_isHidden) return;
            TextTransform.DOScaleY(0f, TextSpeed).SetEase(Easing).OnComplete(() =>
            {
                _isHidden = true;
            });
        }

    }
}