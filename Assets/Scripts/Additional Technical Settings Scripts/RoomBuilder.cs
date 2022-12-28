using Assets.Scripts;
using Assets.Scripts.LevelCreator;
using LevelCreator;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Door = Assets.Scripts.LevelCreator.Door;
using Level = Assets.Scripts.LevelCreator.Level;

namespace Additional_Technical_Settings_Scripts
{
    public class RoomBuilder : MonoBehaviour
    {
        [AssetSelector(Paths = "Assets/Prefabs/LevelBuildingObjects/Doors")] [SerializeField]
        private Door Door;

        [SerializeField] private Door.DoorType DoorType = Door.DoorType.Enemy;


        [ValidateInput("CheckIfLevelExists", "$_message", InfoMessageType.Warning)] [SerializeField]
        private int LevelNumber;

        private bool CheckIfLevelExists(int value)
        {
            var connectedLevel = GameObject.Find("Level" + value);

            _doorConnectedDoor = null;
            _doorConnectionLevel = null;
            _message = "Selected level doesn't exist";
            if (connectedLevel == null) return false;
            var level = connectedLevel.GetComponentInChildren<Level>();
            var doorDirection = DoorAnimator.GetOppositeDirection(Door.GetDoorAnimator().GetDirection());
            foreach (var door in level.GetDoors())
            {
                _message = "Selected level doesn't have doors to connect with this";
                if (door == null || door.GetDoorAnimator().GetDirection() != doorDirection) continue;

                if (door.GetConnectedDoor() == null)
                {
                    _doorConnectedDoor = door;
                    _doorConnectionLevel = level;
                    return true;
                }
            }

            return false;
        }

        public bool DoorIsNull => Door == null;

        [Button]
        [DisableIf("DoorIsNull")]
        public void AddDoor()
        {
            CheckIfLevelExists(LevelNumber);
            Level levelSettings = GetComponentInChildren<Level>();
#if UNITY_EDITOR
            Door door = PrefabUtility.InstantiatePrefab(Door, levelSettings.transform) as Door;

            if (door == null) return;
            levelSettings.AddDoorToList(door);
            door.SetType(DoorType);
            if (_doorConnectedDoor == null) return;
            _doorConnectedDoor.SetConnectedDoor(door);
            door.SetConnectedDoor(_doorConnectedDoor);
#endif
        }

        [Button]
        [DisableIf("DoorIsNull")]
        public void RemoveDoor()
        {
            Level levelSettings = GetComponentInChildren<Level>();
            var door = levelSettings.transform.Find(Door.name).GetComponent<Door>();
            levelSettings.RemoveDoorFromList(door);
            if (door.GetConnectedDoor() != null)
                door.GetConnectedDoor().SetConnectedDoor(null);

            DestroyImmediate(door.gameObject);
        }


        [Button]
        public void SaveLevel()
        {
            Level levelSettings = GetComponentInChildren<Level>();
            FindObjectOfType<LevelManager>().AddLevel(levelSettings);
            FindObjectOfType<AbyssColliderChanger>().UpdateCollidersList();
        }

        private Door _doorConnectedDoor;
        private Level _doorConnectionLevel;
        private string _message = "Dynamic ValidateInput message";
    }
}