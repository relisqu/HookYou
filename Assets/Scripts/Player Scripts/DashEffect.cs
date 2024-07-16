using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Player_Scripts
{
    public class DashEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer PlayerSprite;
        [SerializeField] private int PlayerSpriteCount;
        [SerializeField] private float DelayBetweenFrames;

        [SerializeField] private Color DefaultColor;
        [SerializeField] private Color OffsetColor;
        private SpriteRenderer[] _dashes;

        private bool _dashIsActive;
        private void Awake()
        {
            _dashes = new SpriteRenderer[PlayerSpriteCount];
            for (var index = 0; index < _dashes.Length; index++)
            {
                _dashes[index] = Instantiate(PlayerSprite, PlayerSprite.gameObject.transform.position,
                    Quaternion.identity, null);

                _dashes[index].sortingOrder = PlayerSprite.sortingOrder - 1;
                _dashes[index].gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (PlayerSpriteCount <= 1) return;
            _dashes[0].transform.position = PlayerSprite.transform.position;
            for (var index = 1; index < _dashes.Length; index++)
            {
                var _dash = _dashes[index - 1];
                _dashes[index].transform.position = Vector3.Lerp(_dashes[index].transform.position,
                    _dash.transform.position, Time.deltaTime * DelayBetweenFrames);
            }
        }

        public void StartDash()
        {
            for (var index = 0; index < _dashes.Length; index++)
            {
                var offsetPercent = (index + 1.0f) / _dashes.Length;
                _dashes[index].color = DefaultColor;
                _dashes[index].color -= OffsetColor * offsetPercent;
                _dashes[index].color *= new Vector4(1, 1, 1, 1 - offsetPercent);

                _dashes[index].material.SetInt("IsEnabled", 1);
                _dashes[index].transform.position = PlayerSprite.gameObject.transform.position;
                _dashes[index].material.SetColor("FlashColor", _dashes[index].color);
                _dashes[index].DOKill();
            }

            foreach (var dash in _dashes)
            {
                dash.gameObject.SetActive(true);
                _dashIsActive = true;
            }
        }

        public void StopDashImmediate()
        {
            foreach (var dash in _dashes)
            {
                print("AAAAAAAA");
                dash.gameObject.SetActive(false);
                _dashIsActive = false;
            }
        }

        public void StopDash()
        {
            foreach (var dash in _dashes)
            {
                dash.DOColor(Color.clear, 0.5f).OnPlay(
                    () =>
                    {
                        
                        if (!_dashIsActive) return;
                        dash.material.SetColor("FlashColor", dash.color);
                    }).OnComplete(() =>
                {
                    _dashIsActive = false;
                    dash.gameObject.SetActive(false);
                });
            }
        }
    }
}