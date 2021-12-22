using System;
using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class CameraShift : MonoBehaviour
    {
        [SerializeField] private Vector3 Offset;
        public static CameraShift Instance;
        private void Start()
        {
            Instance = this;
        }

        public void ShiftToNewLevel(Vector3 position)
        {
            transform.position = position + Offset;
        }

    }
}