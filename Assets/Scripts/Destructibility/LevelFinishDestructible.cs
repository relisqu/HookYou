using System;
using Assets.Scripts.LevelCreator;
using UnityEngine;

namespace Destructibility
{
    public class LevelFinishDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;
        [SerializeField] private Level Level;


        public void RespawnOnUncompletedLevel()
        {
            if (!Level.IsCompleted)
            {
                Health.Respawn();
            }
        }


        private void Start()
        {
            Level.CompletedLevel += Health.TakeMaxDamage;
            Level.RestartedLevel += RespawnOnUncompletedLevel;
        }

        private void OnDestroy()
        {
            Level.CompletedLevel -= Health.TakeMaxDamage;
            Level.RestartedLevel -= RespawnOnUncompletedLevel;
        }
    }
}