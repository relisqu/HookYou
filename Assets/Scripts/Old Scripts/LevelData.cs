using System;
using UnityEngine;

namespace Assets.Scripts.Old_Scripts
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/Levels", order = 0)]
    public class LevelData : ScriptableObject
    {
        public int index;
        public GameObject level;
        public GameObject door;
        public DoorData[] doors;
        public int enemiesAmount;
        public DoorLevelType doorType;
        public Vector3 generationPosition;
        public Vector3 positionOfPlayerOnTeleportation;
    }

    public enum DoorLevelType
    {
        Runner,
        FullyOpen,
        Enemy,
        Boss
    }

    [Serializable]
    public class DoorData
    {
        public bool isOpenedFromBeginning;
        public bool isAlwaysOpened;
        public int toLevelIndex;
        public int fromLevelIndex;
        public Vector3 doorPosition;
        public Vector3 newPlayerPosition;
        public DoorType type;
        public DoorTeleportType teleportType;
        public int roomIndex;
        public string nextScene;
    }

    [Serializable]
    public enum DoorType
    {
        Left,
        Right,
        Vertical
    }

    [Serializable]
    public enum DoorTeleportType
    {
        NewScene,
        AnotherRoom
    }
}