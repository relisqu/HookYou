using System;
using DG.Tweening;
using UnityEngine;

namespace LevelCreator
{
    public class DoorLock : MonoBehaviour
    {
        [SerializeField] private Vector3 Offset;
        [Min(0.1f)] [SerializeField] private float Speed;

        public void DropLock()
        {
            transform.DOLocalMove(Offset, 1 / Speed).SetEase(Ease.InSine);
            _spriteRenderer.DOColor(Color.clear, 1 / Speed).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void SetLock()
        {
            transform.position = _defaultPosition;
            _spriteRenderer.color = Color.white;
            gameObject.SetActive(true);
        }

        private SpriteRenderer _spriteRenderer;

        private Vector3 _defaultPosition;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            print(_spriteRenderer);
            _defaultPosition = transform.position;
        }

    }
}