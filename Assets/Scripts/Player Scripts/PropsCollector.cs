using System;
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
        [SerializeField] private GameObject GemObject;
        [SerializeField] private Vector3 Offset;
        [FormerlySerializedAs("ease")] [SerializeField] private Ease Easing;
        [Min(0.1f)] [SerializeField] private float CrystalSpeed;
        [SerializeField] private float CrystalDistance;
        [SerializeField] private UpdateGemText UpdateGemText;
        public static Action OnGemCollect;

        public void CollectGem()
        {
            var gem = Instantiate(GemObject, transform.position + Offset, transform.rotation, transform.root);
            gem.transform.DOLocalMoveY(CrystalDistance, 1 / CrystalSpeed).SetEase(Easing)
                .OnComplete(() => CrystalDisappear(gem));

            PlayerStats.Instance.AddGemsCount(1);
            UpdateGemText.UpdateText();
            OnGemCollect?.Invoke();
        }

        private void CrystalDisappear(GameObject gem)
        {
            DOTween.To(() => gem.GetComponentInChildren<Light2D>().intensity,
                x => gem.GetComponentInChildren<Light2D>().intensity = x, 0, 0.05f);
            gem.GetComponentInChildren<SpriteRenderer>().DOColor(Color.clear, 0.1f).OnComplete(() => Destroy(gem));
        }
    }
}