using UnityEngine;

namespace Assets.Scripts.Old_Scripts
{
    [CreateAssetMenu(fileName = "Floor", menuName = "ScriptableObject/Floors", order = 0)]
    public class FloorData : ScriptableObject
    {
        public LevelData[] levels;
        public GameObject player;
        private Vector3 startPoint;
    }
}