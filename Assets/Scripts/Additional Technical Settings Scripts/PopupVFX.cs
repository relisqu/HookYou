using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class PopupVFX : MonoBehaviour
    {
        [BoxGroup("Spawn")] [SerializeField] private GameObject Object;
        [BoxGroup("Spawn")] [SerializeField] private Vector3 Offset;

        [FormerlySerializedAs("ease")] [SerializeField] [BoxGroup("FlyUp")]
        private Ease Ease;

        [FormerlySerializedAs("CrystalSpeed")] [Min(0.1f)] [SerializeField] [BoxGroup("FlyUp")]
        private float Speed;

        [FormerlySerializedAs("CrystalDistance")] [SerializeField] [BoxGroup("FlyUp")]
        private float FlyDistance;

        [BoxGroup("Bounce")] [SerializeField] private bool IsBouncy;

        [BoxGroup("Bounce")] [Range(0, 2)] [SerializeField]
        private float Stiffness;

        [BoxGroup("Bounce")] [Range(0, 1f)] [SerializeField]
        private float BounceSpeed;

        [BoxGroup("Bounce")] [SerializeField] private Ease BounceEase;


        [BoxGroup("AppearDuration")] [SerializeField]
        private float AppearDuration;

        [BoxGroup("FadeOut")] [Range(0, 1)] [SerializeField]
        private float FadeDuration;

        private GameObject _prop;

        public void InitiateObject()
        {
            var prop = Instantiate(Object, transform.position + Offset, transform.rotation, transform);
            _prop = prop;
            if (IsBouncy)
            {
                prop.transform.localScale = new Vector3(0.8f * Stiffness, 1f, 1f);


                prop.transform.DOScaleX(1f, 1 / BounceSpeed * 0.08f).SetEase(BounceEase).OnComplete(() =>
                {
                    prop.transform.DOScaleY(1.3f * Stiffness, 1 / BounceSpeed * 0.08f).OnComplete(
                        () => { prop.transform.DOScaleY(0.7f, 1 / BounceSpeed * 0.02f); }
                    );
                });
            }

            prop.transform.DOLocalMoveY(FlyDistance, 1 / Speed).SetEase(Ease)
                .OnComplete(() => StartCoroutine(DestroyObject(prop)));
        }

        private IEnumerator DestroyObject(GameObject obj)
        {
            yield return new WaitForSeconds(AppearDuration);
            obj.GetComponent<SpriteRenderer>().DOColor(Color.clear, FadeDuration).OnComplete(() => Destroy(obj));
        }

        public void DestroyObject()
        {
            if (_prop != null)
            {
                StopAllCoroutines();
                _prop.GetComponent<SpriteRenderer>().DOColor(Color.clear, FadeDuration)
                    .OnComplete(() => Destroy(_prop));
            }
        }
    }
}

