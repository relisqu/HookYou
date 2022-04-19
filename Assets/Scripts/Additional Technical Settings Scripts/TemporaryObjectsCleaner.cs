using System;
using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class TemporaryObjectsCleaner : MonoBehaviour
    {
        public static TemporaryObjectsCleaner Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static void AddObject(GameObject obj)
        {
            obj.transform.parent = Instance.transform;
        }

        public static void ClearObjects()
        {
            foreach (Transform item in Instance.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
}