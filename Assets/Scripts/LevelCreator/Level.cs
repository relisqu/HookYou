using System;
using System.Collections.Generic;
using Destructibility;
using DG.Tweening;
using Grappling_Hook.Test;
using LevelCreator;
using MoreMountains.Tools;
using Player_Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.LevelCreator
{
    public class Level : MonoBehaviour
    {
        public enum LevelType
        {
            Boss,
            Time,
            Checkpoint
        }

        [SerializeField] private List<RespawnableLevelObject> CompletionLevelObjects;
        [SerializeField] private List<RespawnableLevelObject> AdditionalLevelObjects;
        [SerializeField] private List<Door> Doors;
        [SerializeField] private Transform TeleportationPoint;
        [SerializeField] private LevelType Type;
        [SerializeField] private Timer Timer;
        private int currentActiveObjectsCount;
        public bool IsCompleted { get; private set; }

        private int defaultObjectsAmount;
        public Player Player { get; private set; }

        private void Awake()
        {
            defaultObjectsAmount = CompletionLevelObjects.Count;
            foreach (var respawnableLevelObject in CompletionLevelObjects)
            {
                respawnableLevelObject.GetHealth().Died += ReduceObjectsAmount;
                respawnableLevelObject.GetHealth().Respawned += RespawnObject;
                respawnableLevelObject.Despawn();
            }

            foreach (var prop in AdditionalLevelObjects)
            {
                prop.Despawn();
            }
            foreach (var door in Doors)
            {
                door.EnteredDoor += EnterLevel;
                door.ExitedDoor += LeaveLevel;
            }
            
        }

        private void Start()
        {
            if (Type == LevelType.Time)
            {
                Timer.TimeIsOver += Player.Die;
            }
        }

        private void RespawnObject()
        {
            currentActiveObjectsCount++;
        }

        private void OnDestroy()
        {
            foreach (var levelObject in CompletionLevelObjects)
            {
                levelObject.GetHealth().Died -= ReduceObjectsAmount;
                levelObject.GetHealth().Respawned -= RespawnObject;
            }

            foreach (var door in Doors) door.EnteredDoor -= EnterLevel;
            if (Type == LevelType.Time)
            {
                Timer.TimeIsOver  -= Player.Die;
            }
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            if(IsCompleted) return;
            foreach (var enemy in CompletionLevelObjects)
            {
                enemy.Spawn();
            }

            foreach (var prop in AdditionalLevelObjects)
            {
                prop.Spawn();
            }
            currentActiveObjectsCount = defaultObjectsAmount;
            if (Type == LevelType.Time)
            {
                Timer.Restart();
            }

            foreach (var door in Doors) door.TryClose();

            Player.transform.position = TeleportationPoint.position;
        }

        private void LeaveLevel(Player _)
        {
            Player = null;
            //TODO: Check this method is bugs occur
            if (LevelType.Time == Type)
            {
                Timer.Reset();
            }
        }

        private void ReduceObjectsAmount()
        {
            currentActiveObjectsCount--;
            print(currentActiveObjectsCount);
            if (currentActiveObjectsCount >= 1) return;
            CompleteLevel();
            OpenAllDoors();
        }

        private void CompleteLevel()
        {
            if (IsCompleted) return;
            if (LevelType.Time == Type)
            {
                Timer.Disable();
            }
            IsCompleted = true;
            Player.GetPropCollector().CollectGem();
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