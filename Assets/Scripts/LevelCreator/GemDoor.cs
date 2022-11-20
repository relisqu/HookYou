using System;
using Assets.Scripts.LevelCreator;
using Player_Scripts;
using UnityEngine;

namespace LevelCreator
{
    public class GemDoor : MonoBehaviour
    {
        [SerializeField]private Door Door;
        [SerializeField]private Level Level;
        [SerializeField]private int GemCount;

        private void OnEnable()
        {
            Door.ManuallyClose();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent(out Player _)) return;
            if (GemCount <= PlayerStats.Instance.GetGemsCount())
            {
                if (Level.IsCompleted && !Door.IsCurrentlyOpened)
                {
                    Door.ManuallyOpen();
                }
            }
        }
    }
}