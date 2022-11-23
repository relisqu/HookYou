using System;
using Assets.Scripts.LevelCreator;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelCreator
{
    public class GemDoor : MonoBehaviour
    {
        [SerializeField] private Door Door;
        [SerializeField] private Level Level;
        [FormerlySerializedAs("GemCount")] [SerializeField] private int RequiredGemCount;

        private void OnEnable()
        {
            Door.ManuallyClose();
        }


        public void TryOpenGemDoor()
        {
            if (RequiredGemCount <= PlayerStats.Instance.GetGemsCount())
            {
                if (Level.IsCompleted && !Door.IsCurrentlyOpened)
                {
                    Door.ManuallyOpen();
                }
            }
        }

        public int GetRequiredGemCount()
        {
            return RequiredGemCount;
        }
    }
}