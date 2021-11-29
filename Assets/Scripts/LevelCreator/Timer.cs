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
            UIText.color = _initialColor;
            while (true)
            {
                var seconds= Mathf.Floor(currentTime%60).ToString("00");
                var minutes= Mathf.Floor(currentTime/60).ToString("00");
                UIText.SetText(minutes+":"+seconds+" <sprite index=0>");
                UIText.color = Color.Lerp(_initialColor, _redColor,(Seconds-currentTime)/Seconds);
                print(Color.Lerp(_initialColor, _redColor,(Seconds-currentTime)/Seconds)+" "+_redColor.linear+" "+(Seconds-currentTime)/Seconds);
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

        private void OnEnable()
        {
            _redColor = new Color(243,151,106,255)/255f;
            _initialColor = new Color(241,231,219,255)/255f;
        }

        private Color _redColor;
        private Color _initialColor;
    }
}