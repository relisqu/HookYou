using System;
using Props;
using UnityEngine;

namespace Player_Scripts
{
    public class PropsCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Gem gem))
            {
                gem.Collect();
                PlayerStats.Instance.AddGemsCount(gem.GetValue);
            }
        }
    }
}