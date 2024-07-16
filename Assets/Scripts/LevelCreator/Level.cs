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
using UnityEngine.Events;
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

        public Action CompletedLevel;

        public Action RestartedLevel;
        [SerializeField] public List<UnityEvent> OnCompleteMethods;
        [SerializeField] public List<UnityEvent> OnRestartMethods;


        public void AddAllEventsToAction(List<UnityEvent> events, ref Action action)
        {
            foreach (var unityEvent in events)
            {
                action += unityEvent.Invoke;
            }
        }

        public void RemoveAllEventsFromAction(List<UnityEvent> events, ref Action action)
        {
            foreach (var unityEvent in events)
            {
                action -= unityEvent.Invoke;
            }
        }

        private void Awake()
        {
            defaultObjectsAmount = CompletionLevelObjects.Count;
            foreach (var respawnableLevelObject in CompletionLevelObjects)
            {
                respawnableLevelObject.GetHealth().Died += ReduceObjectsAmount;
                respawnableLevelObject.GetHealth().Respawned += UpdateObjectsCount;
                respawnableLevelObject.Despawn();
            }

            foreach (var prop in AdditionalLevelObjects)
            {
                prop.Despawn();
            }

            foreach (var door in Doors)
            {
                if (door == null) continue;
                door.EnteredDoor += EnterLevel;
                door.ExitedDoor += LeaveLevel;
            }

            // transform.parent.gameObject.SetActive(false);
            IsCompleted = Type == LevelType.Auto;
            AddAllEventsToAction(OnCompleteMethods, ref CompletedLevel);
            AddAllEventsToAction(OnRestartMethods, ref RestartedLevel);
        }

        public void CompleteLevelAutomatically()
        {
            foreach (var obj in CompletionLevelObjects)
            {
                obj.GetHealth().TakeDamage(1000);
            }
        }

        private void UpdateObjectsCount()
        {
            currentActiveObjectsCount = GetActiveObjectsCount();
        }

        private void OnDestroy()
        {
            foreach (var levelObject in CompletionLevelObjects)
            {
                levelObject.GetHealth().Died -= ReduceObjectsAmount;
                levelObject.GetHealth().Respawned -= UpdateObjectsCount;
            }

            foreach (var door in Doors)
            {
                if (door != null)
                    door.EnteredDoor -= EnterLevel;
            }

            RemoveAllEventsFromAction(OnCompleteMethods, ref CompletedLevel);
            RemoveAllEventsFromAction(OnRestartMethods, ref RestartedLevel);
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            RestartedLevel?.Invoke();
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


            foreach (var door in Doors)
            {
                if (door == null) continue;
                door.TryClose();
            }

            //Player.transform.position = TeleportationPoint.position;
        }

        private void LeaveLevel(Player _)
        {
            //TODO: Check this method is bugs occur

            if (LevelType.Time == Type && !IsCompleted)
            {
                Timer.Disable();
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
                foreach (var door in Doors)
                {
                    if (door == null) continue;
                    door.TryClose();
                }
            }
        }

        public int GetActiveObjectsCount()
        {
            int count = 0;
            foreach (var levelObject in CompletionLevelObjects)
            {
                if (levelObject.GetHealth().IsAlive) count++;
            }

            return count;
        }

        private void ReduceObjectsAmount()
        {
            UpdateObjectsCount();
            if (currentActiveObjectsCount >= 1) return;
            CompleteLevel();
            OpenAllDoors();
        }

        private void CompleteLevel()
        {
            if (IsCompleted) return;
            CompletedLevel?.Invoke();
            if (LevelType.Time == Type)
            {
                Timer.Disable();
                Timer.TimeIsOver -= Player.Die;
                Timer.Reset();
            }

            IsCompleted = true;
            AudioManager.instance.Play("door");
            AudioManager.instance.Play("level_pass");
            Player.GetPropCollector().CollectGem();
        }

        private void OpenAllDoors()
        {
            foreach (var door in Doors)
            {
                if (door == null) continue;
                door.Open();
            }
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

        public void AddDoorToList(Door door)
        {
            if (door == null) return;
            Doors.Add(door);
        }

        public void RemoveDoorFromList(Door door)
        {
            if (door == null) return;
            Doors.Remove(door);
        }

        public List<Door> GetDoors()
        {
            return Doors;
        }

        private bool HasDoorComponent(GameObject obj)
        {
            return obj.transform.parent == transform && obj.TryGetComponent(out Door _);
        }
    }
}