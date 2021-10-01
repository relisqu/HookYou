
using Player_Scripts;
using UnityEngine;

namespace HookBlocks
{
    public abstract class HookBlock : MonoBehaviour
    {
        [SerializeField] private float SwingSpeed;

        public float GetSwingSpeed()
        {
            return SwingSpeed;
        }

        public abstract void AddActivitiesAfterHook(Hook hook);
    }
}