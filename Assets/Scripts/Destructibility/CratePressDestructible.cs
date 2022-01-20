using System;
using HookBlocks;
using UnityEngine;

namespace Destructibility
{
    public class CratePressDestructible : MonoBehaviour
    {
        [SerializeField] private Health Health;

        [Tooltip(
            "This coefficient will slowdown the blocks which are pressing the object. 0-full slowdown, 1-no effect")]
        [Range(0, 1)]
        [SerializeField]
        private float DrakeCoefficient;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PushableBlock block) )
            {

                Health.TakeDamage(Int32.MaxValue);
                block.Drake(DrakeCoefficient);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PushableBlock block))
            {
                    Health.Respawn();
                
            }

        }
    }
}