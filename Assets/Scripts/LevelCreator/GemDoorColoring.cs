using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player_Scripts;
using TMPro;
using UnityEngine;

namespace LevelCreator
{
    public class GemDoorColoring : MonoBehaviour
    {
        private GemDoor _gemDoor;
        [SerializeField] private Color DefaultColor;
        [SerializeField] private Color RedColor;
        [SerializeField] private Color GreenColor;
        [SerializeField] private TMP_Text Text;

        [SerializeField] [Range(0, 10)] private float ColorChangeSpeed;
        [SerializeField] [Range(0, 10)] private float DoorOpenWaitPause;

        private void Start()
        {
            _gemDoor = GetComponent<GemDoor>();
            PropsCollector.OnGemCollect += ReactToPlayerGemCount;
            Text.SetText("{0} <sprite index=1>", _gemDoor.GetRequiredGemCount());
        }

        private void OnDestroy()
        {
            PropsCollector.OnGemCollect -= ReactToPlayerGemCount;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player _)) return;
            _playerInZone = true;
            ReactToPlayerGemCount();
        }
        
        private void ReactToPlayerGemCount()
        {
            if(!_playerInZone) return;
            
            if (_gemDoor.GetRequiredGemCount() <= PlayerStats.Instance.GetGemsCount())
            {
                OpenDoor();
            }
            else
            {
                ChangeColor(RedColor);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player _)) return;
            DOTween.KillAll();
            _playerInZone = false;
            ChangeColor(DefaultColor);
        }

        private bool _playerInZone;
        public void OpenDoor()
        {
            ChangeColor(GreenColor);
            StartCoroutine(PauseDoorOpening());
        }

        private IEnumerator PauseDoorOpening()
        {
            yield return new WaitForSeconds(DoorOpenWaitPause);
            _gemDoor.TryOpenGemDoor();
        }

        public void ChangeColor(Color color)
        {
            Text.DOColor(color, ColorChangeSpeed).SetSpeedBased();
        }
    }
}