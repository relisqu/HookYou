using System.Collections.Generic;
using Destructibility;
using Grappling_Hook.Test;
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.LevelCreator
{
    public class Level : MonoBehaviour
    {
        public enum LevelType
        {
            Boss,
            Enemy,
            Runner,
            Checkpoint
        }

        [FormerlySerializedAs("Enemies")] [SerializeField] private List<RespawnableLevelObject> LevelObjects;
        [SerializeField] private List<Door> Doors;
        [SerializeField] private Transform TeleportationPoint;
        [SerializeField] private LevelType Type;
        private int currentActiveObjectsCount;
        public bool IsCompleted { get; private set; }

        private int defaultObjectsAmount => LevelObjects.Count;
        public Player Player { get; private set; }

        private void OnEnable()
        {
            foreach (var respawnableLevelObject in LevelObjects)
            {
                respawnableLevelObject.GetHealth().Died += ReduceObjectsAmount;
                respawnableLevelObject.Despawn();
            }

            foreach (var door in Doors)
            {
                door.EnteredDoor += EnterLevel;
                door.ExitedDoor += LeaveLevel;
            }
        }

        private void OnDisable()
        {
           foreach (var levelObject in LevelObjects) levelObject.GetHealth().Died -= ReduceObjectsAmount;

           foreach (var door in Doors) door.EnteredDoor -= EnterLevel;
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            foreach (var enemy in LevelObjects) enemy.Spawn();

            currentActiveObjectsCount = defaultObjectsAmount;
            foreach (var door in Doors) door.TryClose();

            Player.transform.position = TeleportationPoint.position;
        }

        private void LeaveLevel(Player _)
        {
            Player = null;
            IsCompleted = true;
        }

        private void ReduceObjectsAmount()
        {
            currentActiveObjectsCount--;
            
            if (currentActiveObjectsCount < 1)
            {
                CompleteLevel();
                OpenAllDoors();
            }
        }

        private void CompleteLevel()
        {
                IsCompleted = true;
        }

        private void OpenAllDoors()
        {
            foreach (var door in Doors) door.Open();
        }

        public void EnterLevel(Player player)
        {
            Player = player;
            if (!IsCompleted) Restart();
        }

        public Vector3 GetDefaultTeleportLocation()
        {
            return TeleportationPoint.position;
        }
    }
}