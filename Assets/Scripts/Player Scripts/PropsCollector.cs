using System;
using System.Numerics;
using DG.Tweening;
using Props;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player_Scripts
{
    public class PropsCollector : MonoBehaviour
    {
        /* private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Gem gem))
            {
                gem.Collect();
                PlayerStats.Instance.AddGemsCount(gem.GetValue);
            }
        }*/
        [SerializeField]private GameObject GemObject;
        [SerializeField]private Vector3 Offset;
        [SerializeField]private Ease ease;
        [Min(0.1f)][SerializeField]private float CrystalSpeed;
        [SerializeField]private float CrystalDistance;

        public void CollectGem()
        {
            var gem=Instantiate(GemObject,transform.position+Offset,transform.rotation, transform.root);
            gem.transform.DOLocalMoveY(CrystalDistance, 1 / CrystalSpeed).SetEase(ease).OnComplete(()=>CrystalDisappear(gem));
            
            PlayerStats.Instance.AddGemsCount(1);
            
        }

        private void CrystalDisappear(GameObject gem)
        {
            DOTween.To(()=> gem.GetComponentInChildren<Light2D>().intensity, x=> gem.GetComponentInChildren<Light2D>().intensity = x, 0, 0.05f);
            gem.GetComponentInChildren<SpriteRenderer>().DOColor(Color.clear, 0.1f).OnComplete(() => Destroy(gem));
        }

    }
}