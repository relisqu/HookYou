using System.Collections.Generic;
using Destructibility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public abstract class BossStage : MonoBehaviour
    {
        [SerializeField] private BossStage NextStage;

        public abstract void Attack();
        public abstract void StopCurrentAttack();

        public void MoveToNextStage()
        {
            StopCurrentAttack();
            if (NextStage != null) NextStage.Attack();
        }
    }
}