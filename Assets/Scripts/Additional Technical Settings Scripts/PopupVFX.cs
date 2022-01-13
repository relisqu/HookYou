using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class PopupVFX : MonoBehaviour
    {
        [BoxGroup("Spawn")][SerializeField] private GameObject Object;
        [BoxGroup("Spawn")][SerializeField] private Vector3 Offset;

        [FormerlySerializedAs("ease")] [SerializeField]
        [BoxGroup("FlyUp")] private Ease Ease;

        [FormerlySerializedAs("CrystalSpeed")] [Min(0.1f)] [SerializeField]
        [BoxGroup("FlyUp")] private float Speed;

        [FormerlySerializedAs("CrystalDistance")] [SerializeField]
        [BoxGroup("FlyUp")] private float FlyDistance;

        [BoxGroup("Bounce")] [SerializeField] private bool IsBouncy;

        [BoxGroup("Bounce")] [Range(0, 2)] [SerializeField]
        private float Stiffness;

        [BoxGroup("Bounce")] [Range(0, 1f)] [SerializeField]
        private float BounceSpeed;

        [BoxGroup("Bounce")] [SerializeField] private Ease BounceEase;

        [BoxGroup("FadeOut")] [Range(0,1)][SerializeField] private float FadeDuration;
        public void InitiateObject()
        {
            var prop = Instantiate(Object, transform.position + Offset, transform.rotation, transform.root);
            prop.transform.localScale = new Vector3(0.8f * Stiffness, 1f, 1f);
            if (IsBouncy)
                prop.transform.DOScaleX(1f, 1/BounceSpeed*0.08f).SetEase(BounceEase).OnComplete(() =>
                {
                    prop.transform.DOScaleY(1.3f * Stiffness, 1/BounceSpeed*0.08f).OnComplete(
                        () => { prop.transform.DOScaleY(1f, 1/BounceSpeed*0.08f); }
                    );
                });
            var flyValue = prop.transform.position.y + FlyDistance;
            prop.transform.DOMoveY(flyValue, 1 / Speed).SetEase(Ease)
                .OnComplete(() => DestroyObject(prop));
        }

        private void DestroyObject(GameObject obj)
        {
            obj.GetComponent<SpriteRenderer>().DOColor(Color.clear, FadeDuration).OnComplete(() => Destroy(obj));
        }
    }
}