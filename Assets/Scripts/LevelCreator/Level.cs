using System;
using System.Collections.Generic;
using Grappling_Hook.Test;
using UnityEngine;

namespace Assets.Scripts.LevelCreator
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Enemy> Enemies;
        [SerializeField] private List<Door> Doors;
        [SerializeField] private Transform TeleportationPoint;
        [SerializeField] private LevelType Type;
        public bool IsCompleted => isCompleted;

        public enum LevelType
        {
            Boss,
            Enemy,
            Runner,
            Checkpoint
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            foreach (var enemy in Enemies)
            {
                enemy.EnableEnemy();
            }

            currentEnemiesCount = defaultEnemiesAmount;
            foreach (var door in Doors)
            {
                door.TryClose();
            }

            player.transform.position = TeleportationPoint.position;
        }

        private void OnEnable()
        {
            foreach (var enemy in Enemies)
            {
                enemy.EnemyDied += CheckEnemiesAmount;
                enemy.DisableEnemy();
            }

            foreach (var door in Doors)
            {
                door.EnteredDoor += EnterLevel;
                door.ExitedDoor += LeaveLevel;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in Enemies)
            {
                enemy.EnemyDied -= CheckEnemiesAmount;
            }

            foreach (var door in Doors)
            {
                door.EnteredDoor -= EnterLevel;
            }
        }

        private void LeaveLevel(Player _)
        {
            player = null;
            isCompleted = true;
        }

        void CheckEnemiesAmount()
        {
            currentEnemiesCount--;
            if (currentEnemiesCount < 1)
            {
                foreach (var door in Doors)
                {
                    door.Open();
                }

                isCompleted = true;
            }
        }

        public void EnterLevel(Player player)
        {
            this.player = player;
            if (!isCompleted)
            {
                Restart();
            }
        }

        public Vector3 GetDefaultTeleportLocation()
        {
            return TeleportationPoint.position;
        }
        
        private int defaultEnemiesAmount => Enemies.Count;
        private int currentEnemiesCount;
        private bool isCompleted;
        private Player player;
        public Player Player => player;
    }
}