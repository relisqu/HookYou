using System;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraShake : MonoBehaviour
    {
        private static CinemachineBasicMultiChannelPerlin _shakeNoise;
        private static TweenerCore<float, float, FloatOptions> _seq;

        private void Awake()
        {
            _shakeNoise = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public static void ShakeCamera(float duration, float power)
        {
            if (_seq != null) DOTween.Kill(_seq);
            _shakeNoise.m_AmplitudeGain = power;
            var amplitude = 0f;

            _seq = DOTween.To(() => amplitude, x => amplitude = x, power, duration / 2)
                .OnUpdate(() => UpdateAmplitude(amplitude)).OnComplete(() =>
                {
                    DOTween.To(() => amplitude, x => amplitude = x, 0, duration / 2)
                        .OnUpdate(() => UpdateAmplitude(amplitude));
                });
        }

        public static void UpdateAmplitude(float amplitude)
        {
            _shakeNoise.m_AmplitudeGain = amplitude;
        }
    }
}