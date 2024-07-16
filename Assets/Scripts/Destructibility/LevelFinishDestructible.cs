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
            if (Level == null) Level = GetComponentInParent<Level>();
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