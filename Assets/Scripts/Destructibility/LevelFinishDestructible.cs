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
            print("AAAAAAAAAAA");
            Level.CompletedLevel += Health.Died;
            Level.RestartedLevel += Health.Respawned;
        }

        private void OnDestroy()
        {
            Level.CompletedLevel -= Health.Died;
            Level.RestartedLevel -= Health.Respawned;
        }
    }
}