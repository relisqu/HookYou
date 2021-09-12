using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Old_Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [FormerlySerializedAs("floor")] public FloorData Floor;
        [FormerlySerializedAs("Player")] public Player Player;
        [FormerlySerializedAs("rooms")] public Level[] Rooms;
        [FormerlySerializedAs("doors")] public Door[] Doors;

        private void Awake()
        {
            RestartFloor();
        }

        public Level GetLevelFromIndex(int index)
        {
            foreach (var room in Rooms)
                if (room.data.index == index)
                    return room;

            return null;
        }

        private void RestartFloor()
        {
            Rooms = new Level[Floor.levels.Length];
            Doors = new Door[Floor.levels.Length * 4];
            var index = 0;
            var doorIndex = 0;
            foreach (var room in Floor.levels)
            {
                var roomObject = Instantiate(room.level, room.generationPosition, Quaternion.identity);
                Rooms[index] = roomObject.AddComponent<Level>();
                Rooms[index].level = roomObject;
                Rooms[index].data = room;
                Rooms[index].currentAmountOfEnemies = room.enemiesAmount;
                doorIndex = InstantiateDoors(room, roomObject, doorIndex);
                index++;
                roomObject.gameObject.SetActive(false);
            }

            Rooms[0].level.gameObject.SetActive(true);
            //Player.currentRoomIndex = Floor.levels[0].index;
        }

        public void RestartRoom(int index, bool teleportPlayerToRoom)
        {
            for (var i = 0; i < Floor.levels.Length; i++)
            {
                var room = Floor.levels[i];
                if (index == room.index)
                {
                    print(i);
                    Destroy(Rooms[i].gameObject);
                    var j = 0;
                    while (Doors[j] != null) j++;

                    var roomObject = Instantiate(room.level, room.generationPosition, Quaternion.identity);
                    Rooms[i] = roomObject.AddComponent<Level>();
                    Rooms[i].level = roomObject;
                    Rooms[i].data = room;
                    Rooms[i].currentAmountOfEnemies = room.enemiesAmount;
                    InstantiateDoors(room, Rooms[i].level, j - 1);
                    Player.transform.position = room.positionOfPlayerOnTeleportation;
                }
            }
        }

        public int InstantiateDoors(LevelData room, GameObject roomObj, int index)
        {
            var doorsData = room.doors;

            foreach (var doorData in doorsData)
            {
                print(doorData.isOpenedFromBeginning);
                var doorObject = Instantiate(room.door, doorData.doorPosition, Quaternion.identity, roomObj.transform);
                var door = doorObject.GetComponent<Door>();
                door.data = doorData;
                door.transform.position = doorData.doorPosition + room.generationPosition;
                door.SetDoorOpened(doorData.isOpenedFromBeginning);
                door.SetStats(doorData.type);
                door.manager = this;
                Doors[index] = door;
                index++;
            }

            return index;
        }
    }
}