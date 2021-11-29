using System;
using System.Collections;
using Destructibility;
using UnityEngine;

namespace LevelCreator
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float Seconds;
        [SerializeField] private TMPro.TMP_Text UIText;
        public Action TimeIsOver;
        private float currentTime;
        private bool isActivated;


        public void Restart()
        {
            if (isActivated) return;
            StopAllCoroutines();
            StartCoroutine(TickTimer());
        }

        public void Disable()
        {
            UIText.SetText("");
            isActivated = true;
            StopAllCoroutines();
        }
        public void Reset()
        {
            UIText.SetText("");
            isActivated = false;
            StopAllCoroutines();
        }
        IEnumerator TickTimer()
        {
            currentTime = Seconds;
            var initialColor = UIText.color;
            while (true)
            {
                var seconds= Mathf.Floor(currentTime%60).ToString("00");
                var minutes= Mathf.Floor(currentTime/60).ToString("00");
                UIText.SetText(minutes+":"+seconds+" <sprite index=0>");
                UIText.color= Color.Lerp(initialColor, _redColor,(Seconds-currentTime)/Seconds);
                yield return new WaitForSecondsRealtime(1);
                currentTime--;
                if (currentTime < 0)
                {
                    TimeIsOver?.Invoke();
                    yield break;
                }
            }
            yield return null;
        }
        private readonly Color _redColor = new Color(243,151,106);
    }
}