
using Player_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace HookBlocks
{
    public abstract class HookBlock : MonoBehaviour
    {
        [SerializeField] private float SwingSpeed;
        [FormerlySerializedAs("UsingSpecificHookSpeed")] [SerializeField] private bool RequiresUniqueHookSpeed;
        [SerializeField] private float HookShotSpeed;

        

        public float GetSwingSpeed()
        {
            return SwingSpeed;
        }

        public bool RequiresSpecificHookSpeed()
        {
            return RequiresUniqueHookSpeed;
        }

        public float GetHookShotSpeed()
        {
            return HookShotSpeed;
        }


        public abstract void AddActivitiesAfterHook(Hook hook);
    }
}