using System;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace Player_Scripts
{
    public class StatsText : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody;
        [SerializeField] private float WaitDuration;
        [SerializeField] private TextPopup TextPopup;
        [Range(0, 2f)] [SerializeField] private float Sensitivity;
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
                if (!_isHidden || TextPopup.GetTextSize() > 0.01f) HideText();
            }

            if (_waitStartTime >= WaitDuration)
            {
                if (_isHidden) ShowText();
            }
        }

        private void ShowText()
        {
            _isHidden = false;
            TextPopup.ShowText();
        }

        private void HideText()
        {
            _isHidden = true;
            TextPopup.HideText();
        }
    }
}