using System;
using TMPro;
using UnityEngine;

namespace Player_Scripts
{
    public class UpdateGemText : MonoBehaviour
    {
        [SerializeField] private TMP_Text GemText;

        private void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
           GemText.SetText("{0} <sprite index=1>",PlayerStats.Instance.GetGemsCount());
        }
    }
}