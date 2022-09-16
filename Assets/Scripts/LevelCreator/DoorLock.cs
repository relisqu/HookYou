using System;
using System.Collections;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.LevelCreator;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace LevelCreator
{
    public class DoorLock : MonoBehaviour
    {
        [SerializeField] private Vector3 Offset;
        [Min(0.1f)] [SerializeField] private float Speed;
        public Action LockDestroyed;

        TweenerCore<Vector3, Vector3, VectorOptions> transformSeq;
        TweenerCore<Color, Color, ColorOptions> colorSeq;
        public void DropLock()
        {
            if (!_isBlocked) return;
            transformSeq = transform.DOLocalMove(2 * Offset, 2 / Speed).SetEase(Ease.InSine);
            colorSeq = _spriteRenderer.DOColor(Color.clear, 1 / Speed).OnComplete(() => { gameObject.SetActive(false); });
            _isBlocked = false;
            LockDestroyed();
        }

        public void SetLock()
        {
            transform.position = _defaultPosition;
            transformSeq.Kill();
            colorSeq.Kill();
            _spriteRenderer.color = Color.white;
            gameObject.SetActive(true);
            _isBlocked = true;
        }

        private SpriteRenderer _spriteRenderer;

        private Vector3 _defaultPosition;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            print(_spriteRenderer);
            _defaultPosition = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TakeSwordDamage(other);
        }

        private void TakeSwordDamage(Collider2D other)
        {
            if (other.TryGetComponent(out SwordAttack sword) && _canBeAttacked && _isBlocked)
            {
                if (sword.IsAttacking)
                {
                    sword.Hit();
                    DropLock();
                    StartCoroutine(MakeIFrame());
                }
            }
        }


        private IEnumerator MakeIFrame()
        {
            _canBeAttacked = false;
            yield return new WaitForSeconds(0.2f);
            _canBeAttacked = true;
        }

        private bool _canBeAttacked = true;
        private bool _isBlocked = false;
    }
}