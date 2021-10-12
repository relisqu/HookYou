using System.Collections.Generic;
using Grappling_Hook.Test;
using UnityEngine;

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

        [SerializeField] private List<Enemy> Enemies;
        [SerializeField] private List<Door> Doors;
        [SerializeField] private Transform TeleportationPoint;
        [SerializeField] private LevelType Type;
        private int currentEnemiesCount;
        public bool IsCompleted { get; private set; }

        private int defaultEnemiesAmount => Enemies.Count;
        public Player Player { get; private set; }

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
            foreach (var enemy in Enemies) enemy.EnemyDied -= CheckEnemiesAmount;

            foreach (var door in Doors) door.EnteredDoor -= EnterLevel;
        }

        public LevelType GetLevelType()
        {
            return Type;
        }

        public void Restart()
        {
            foreach (var enemy in Enemies) enemy.EnableEnemy();

            currentEnemiesCount = defaultEnemiesAmount;
            foreach (var door in Doors) door.TryClose();

            Player.transform.position = TeleportationPoint.position;
        }

        private void LeaveLevel(Player _)
        {
            Player = null;
            IsCompleted = true;
        }

        private void CheckEnemiesAmount()
        {
            currentEnemiesCount--;
            if (currentEnemiesCount < 1)
            {
                foreach (var door in Doors) door.Open();

                IsCompleted = true;
            }
        }

        public void EnterLevel(Player player)
        {
            this.Player = player;
            if (!IsCompleted) Restart();
        }

        public Vector3 GetDefaultTeleportLocation()
        {
            return TeleportationPoint.position;
        }
    }
}