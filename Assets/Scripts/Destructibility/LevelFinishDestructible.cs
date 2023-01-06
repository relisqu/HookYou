using System;
using Assets.Scripts.LevelCreator;
using UnityEngine;

namespace Destructibility
{
    public class LevelFinishDestructible : MonoBehaviour
    {
        [SerializeField]private Health Health;
        [SerializeField]private Level Level;

        private void Start()
        {
            Level.CompletedLevel += Health.Die;
            Level.RestartedLevel += Health.Respawn;
        }

        private void OnDestroy()
        {
            Level.CompletedLevel -=  Health.Die;
            Level.RestartedLevel -= Health.Respawn;
        }
    }
}