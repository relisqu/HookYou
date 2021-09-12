using Grappling_Hook.Test;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Old_Scripts
{/*
    public class DoorManager : MonoBehaviour
    {
        [FormerlySerializedAs("levelManager")] public LevelManager LevelManager;
        [FormerlySerializedAs("currentDoors")] public Door[] CurrentDoors;

        private void OnEnable()
        {
            Boss.ActivateBoss += CloseDoor;
            Enemy.EnemyDied += CheckEnemiesAmount;
            Boss.KillBoss += OpenDoor;
        }

        private void OnDisable()
        {
            Boss.ActivateBoss -= CloseDoor;
            Boss.KillBoss -= OpenDoor;
            Enemy.EnemyDied -= CheckEnemiesAmount;
        }


        public void SetRoomsDoorsOpen(int levelIndex, bool value)
        {
            CurrentDoors = new Door[8];
            foreach (var door in LevelManager.Doors)
            {
                if (levelIndex != door.data.fromLevelIndex && !door.gameObject.activeInHierarchy) continue;
                door.SetDoorOpened(value);
                break;
            }
        }

        private void CloseDoor(LevelData data)
        {
            SetRoomsDoorsOpen(data.index, false);
        }

        private void OpenDoor(LevelData data)
        {
            SetRoomsDoorsOpen(data.index, true);
        }


        private void CheckEnemiesAmount(LevelData data)
        {
            var room = LevelManager.GetLevelFromIndex(data.index);
            room.currentAmountOfEnemies -= 1;
            if (room.currentAmountOfEnemies <= 0) OpenDoor(data);
        }
    }*/
}