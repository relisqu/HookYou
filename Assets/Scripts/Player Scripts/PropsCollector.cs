using System;
using AI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace Player_Scripts
{
    public class PropsCollector : MonoBehaviour
    {
        [SerializeField] private PopupVFX Gem;
        [SerializeField] private UpdateGemText UpdateGemText;
        public static Action OnGemCollect;

        public void CollectGem()
        {
            Gem.InitiateObject();
            PlayerStats.Instance.AddGemsCount(1);
            UpdateGemText.UpdateText();
            OnGemCollect?.Invoke();

            _gemLight.intensity = _gemLightDefaultIntensity;
        }

        private Light2D _gemLight;
        private float _gemLightDefaultIntensity;

        private void Start()
        {
            _gemLight = Gem.GetCurrentObject().GetComponentInChildren<Light2D>();
            _gemLightDefaultIntensity = _gemLight.intensity;
            Gem.OnHideStart += RemoveLight;
        }

        private void OnDestroy()
        {
            Gem.OnHideStart -= RemoveLight;
        }

        private void RemoveLight()
        {
            DOTween.To(() => _gemLight.intensity,
                x => _gemLight.intensity = x, 0, 0.05f);
        }
    }
}