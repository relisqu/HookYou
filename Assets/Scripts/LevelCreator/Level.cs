using System;
using System.Collections.Generic;
using Additional_Technical_Settings_Scripts;
using Destructibility;
using DG.Tweening;
using Grappling_Hook.Test;
using LevelCreator;
using MoreMountains.Tools;
using Player_Scripts;
using Sirenix.OdinInspector;
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
            Standard,
            Auto
        }

        [ChildGameObjectsOnly] [SerializeField]
        private List<RespawnableLevelObject> CompletionLevelObjects;

        [ChildGameObjectsOnly] [SerializeField]
        private List<RespawnableLevelObject> AdditionalLevelObjects;

        [ChildGameObjectsOnly] [SerializeField]
        private List<Door> Doors;

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

            // transform.parent.gameObject.SetActive(false);
            IsCompleted = Type == LevelType.Auto;
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
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            TemporaryObjectsCleaner.ClearObjects();
            if (IsCompleted) return;
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

            //Player.transform.position = TeleportationPoint.position;
        }

        private void LeaveLevel(Player _)
        {
            //TODO: Check this method is bugs occur
            if (LevelType.Time == Type)
            {
                Timer.TimeIsOver -= Player.Die;
                Timer.Reset();
            }

            Player = null;

            if (!IsCompleted)
            {
                foreach (var enemy in CompletionLevelObjects)
                {
                    enemy.Despawn();
                }

                foreach (var prop in AdditionalLevelObjects)
                {
                    prop.Despawn();
                }

                currentActiveObjectsCount = defaultObjectsAmount;
                foreach (var door in Doors) door.TryClose();
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
            AudioManager.instance.Play("door");
            AudioManager.instance.Play("level_pass");
            Player.GetPropCollector().CollectGem();
        }

        private void OpenAllDoors()
        {
            foreach (var door in Doors) door.Open();
        }

        public void EnterLevel(Player player)
        {
            print("Entered level");
           // transform.parent.gameObject.SetActive(true);
            CameraShift.Instance.ShiftToNewLevel(transform.position);
            Player = player;
            if (player.LastVisitedDoor != null && !IsCompleted) player.LastVisitedDoor.SetBlocked();
            if (Type == LevelType.Time)
            {
                Timer.TimeIsOver += Player.Die;
            }

            if (!IsCompleted) Restart();
        }

        public Vector3 GetDefaultTeleportLocation()
        {
            return TeleportationPoint.position;
        }
    }
}