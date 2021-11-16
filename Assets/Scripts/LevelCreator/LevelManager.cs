using System.Collections.Generic;
using Player_Scripts;
using UnityEngine;

namespace Assets.Scripts.LevelCreator
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Level> Levels;


        public void EnterTheFloor(Player player)
        {
            Levels[0].EnterLevel(player);
        }

        public void RestartFloor(Player player)
        {
            foreach (var room in Levels) room.Restart();
            

            player.LastVisitedDoor = null;
            var checkpoint = Levels.FindLast(level =>
                level.GetLevelType() == Level.LevelType.Checkpoint && level.IsCompleted);
            player.transform.position = checkpoint.GetDefaultTeleportLocation();
        }

        public void RestartCurrentRoom(Player player)
        {
            var lastRoom = Levels.FindLast(level => level.Player == player);
            if (lastRoom == null)
            {
                Levels[0].Restart();
                return;
            }

            if (lastRoom.GetLevelType() == Level.LevelType.Boss)
            {
            }
            else
            {
                lastRoom.Restart();
                player.transform.position = player.LastVisitedDoor != null
                    ? player.LastVisitedDoor.GetTeleportationPoint()
                    : lastRoom.GetDefaultTeleportLocation();
            }
        }
    }
}