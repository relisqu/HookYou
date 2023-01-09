using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class PopupVFX : MonoBehaviour
    {
        [BoxGroup("Spawn")] [SerializeField] private SpriteRenderer Object;
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

        private SpriteRenderer _popupObj;

        public void Start()
        {
            _popupObj = Instantiate(Object, transform.position + Offset, transform.rotation, transform);
            _popupObj.gameObject.SetActive(false);
        }

        public void InitiateObject()
        {
            _popupObj.transform.position = transform.position + Offset;
            _popupObj.color = Color.white;
            _popupObj.gameObject.SetActive(true);
            if (IsBouncy)
            {
                _popupObj.transform.localScale = new Vector3(0.8f * Stiffness, 1f, 1f);


                _popupObj.transform.DOScaleX(1f, 1 / BounceSpeed * 0.08f).SetEase(BounceEase).OnComplete(() =>
                {
                    _popupObj.transform.DOScaleY(1.3f * Stiffness, 1 / BounceSpeed * 0.08f).OnComplete(
                        () => { _popupObj.transform.DOScaleY(0.7f, 1 / BounceSpeed * 0.02f); }
                    );
                });
            }

            _popupObj.transform.DOLocalMoveY(FlyDistance, 1 / Speed).SetEase(Ease)
                .OnComplete(() => StartCoroutine(RemoveObject(_popupObj)));
        }

        private IEnumerator RemoveObject(SpriteRenderer obj)
        {
            yield return new WaitForSeconds(AppearDuration);
            HideObject();
        }

        public void HideObject()
        {
            StopAllCoroutines();
            _popupObj.DOColor(Color.clear, FadeDuration)
                .OnComplete(() => _popupObj.gameObject.SetActive(false));
        }
    }
}